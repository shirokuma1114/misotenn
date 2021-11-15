using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class CharacterBase : MonoBehaviour
{
    [SerializeField]
    CharacterControllerBase _controller;


    // 名前
    private string _name;

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    // 所持金
    private int _money;

    public int Money
    { get { return _money; } }


    // 移動カードリスト
    private List<int> _movingCards = new List<int>();
    public List<int> MovingCards
    { get { return _movingCards; } }

    // お土産カードリスト
    private List<Souvenir> _souvenirs = new List<Souvenir>();

    public List<Souvenir> Souvenirs
    { get { return _souvenirs; } }

    // 現在止まっているマス
    protected SquareBase _currentSquare;

    public SquareBase CurrentSquare
    {
        get { return _currentSquare; }
    }

    private CharacterState _state = CharacterState.WAIT;
    public CharacterState State
    {
        get { return _state; }
    }

    [SerializeField]
    private Floating_Local_Miya _floating;
    
    private int _movingCount;

    public int MovingCount
    {
        get { return _movingCount; }
    }

    public bool IsAutomatic
    {
        get { return _controller.IsAutomatic; }
    }

    // ラップ数
    public int LapCount { get; set; }

    public CharacterLog Log { get; }

    private float _nextSquareDist;

    private float _amplitude;

    private float _originPosZ;

    private bool _waitEnable;

    protected virtual void Start()
    {
        _originPosZ = transform.position.z;
    }

    void Update()
    {
        UpdateMove();
    }

    public void AddMoney(int addValue)
    {
        _money += addValue;
    }

    public void SubMoney(int subValue)
    {
        _money -= subValue;
    }

    public bool CanPay(int value)
    {
        return value <= _money ? true : false;
    }

    public void AddMovingCard(int movingValue)
    {
        _movingCards.Add(movingValue);
    }

    public void RemoveMovingCard(int cardIndex)
    {
        _movingCount = _movingCards[cardIndex];
        _movingCards.RemoveAt(cardIndex);
    }
    
    public void AddSouvenir(Souvenir souvenir)
    {
        _souvenirs.Add(souvenir);
    }

    public void RemoveSouvenir(int index)
    {
        _souvenirs.RemoveAt(index);
    }

    public void RemoveSouvenir(Souvenir souvenir)
    {
        _souvenirs.Remove(souvenir);
    }

    // スタートのマスを設定
    public void SetCurrentSquare(SquareBase square)
    {
        _currentSquare = square;
    }

    public void Init()
    {
        _state = CharacterState.WAIT;
        _currentSquare.RemoveCharacter(this);
    }

    public void SetWaitEnable(bool enable)
    {
        if (enable)
        {
            // 縮小
            transform.SetParent(_currentSquare.GetComponent<Transform>());
            transform.localScale = new Vector3(15.5f, 15.5f, 15.5f);

            // 回転
            transform.eulerAngles = new Vector3(0.0f, -90.0f, 90.0f);

            _currentSquare.AddCharacter(this);
        }
        else
        {
            _currentSquare.RemoveCharacter(this);

            // 移動
            transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            transform.Translate(0, 0.65f, 0);

            // 拡大
            transform.SetParent(null);
            transform.localScale = new Vector3(10.0f, 10.0f, 10.0f);
        }
        _waitEnable = enable;
    }

    public void InitAlignment(int index)
    {
        transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        transform.Translate(0, 0.65f, 0);
        AlignmentByMove(index);
    }

    private void AlignmentByMove(int index)
    {
        transform.localPosition = new Vector3(0.0f, 1.2f, 0.0f);
        transform.Translate(-((index / 2) * 0.35f - 0.5f * 0.35f), 0, -((index % 2) * 0.35f - 0.5f * 0.35f));
    }

    public void Alignment()
    {
        if (!_waitEnable) return;
        //Debug.Log(_currentSquare.GetStoppedCharacterNum());

        AlignmentByMove(_currentSquare.GetAlignmentIndexByCharacter(this));
    }

    public void StartMove(SquareBase square)
    {
        _state = CharacterState.MOVE;
        _floating.Set_Using(false);
        _movingCount--;

        _currentSquare = square;

        // 距離を計算
        CalcDistToNextSquare();

        // ステージ回転
        FindObjectOfType<EarthMove>().MoveToPosition(_currentSquare.GetPosition(), 50.0f);
    }

    private void UpdateMove()
    {
        if (_state != CharacterState.MOVE) return;
        UpdateAngleToSquare();
        if (FindObjectOfType<EarthMove>().State == EarthMove.EarthMoveState.END)
        {
            _state = CharacterState.WAIT;
            _floating.Set_Using(true);
        }
    }

    public void Stop()
    {
        _state = CharacterState.STOP;
        _currentSquare.Stop(this);
    }

    public List<SquareBase> GetInConnects()
    {
        var outs = new List<SquareBase>();

        // スタックにあるマスのみ
        foreach (var s in _currentSquare.InConnects)
        {
            outs.Add(s);
        }
        
        return outs;
    }

    public List<SquareBase> GetOutConnects()
    {
        return _movingCount > 0 ? _currentSquare.OutConnects : new List<SquareBase>();
    }

    public void CompleteStopExec()
    {
        if (_movingCount > 0)
        {
            _state = CharacterState.WAIT;
            return;
        }
        _state = CharacterState.END;
    }

    // 現在のお土産種類所持数
    public int GetSouvenirTypeNum()
    {
        var typeList = new bool[(int)SouvenirType.MAX_TYPE];

        // お土産を全種類揃えている
        foreach (var x in _souvenirs)
        {
            typeList[(int)x.Type] = true;
        }
        return typeList.Where(x => x == true).Count();
    }

    private void UpdateAngleToSquare()
    {
        var targetPos = _currentSquare.gameObject.transform.position;
        var position = transform.position;
        if (Vector3.Distance(targetPos, position) < 0.7f) return;

        //var position = new Vector3(transform.position.x, transform.position.y, _originPosZ);
        
        Vector3 direction = (targetPos - position).normalized;
        Vector3 xAxis = Vector3.Cross(new Vector3(0, 0, 1), direction).normalized;
        Vector3 zAxis = Vector3.Cross(xAxis, new Vector3(0, 0, 1)).normalized;
        transform.rotation = Quaternion.LookRotation(zAxis, new Vector3(0, 0, 1));
        transform.Rotate(0.0f, -90.0f, 180.0f);

        // 心がぴょんぴょんしない
        //float dist = (targetPos - position).magnitude;
        //transform.position = new Vector3(transform.position.x, transform.position.y, originPosZ - Mathf.Sin(Mathf.PI / _nextSquareDist * (_nextSquareDist - dist)) * _amplitude);
    }

    private void CalcDistToNextSquare()
    {
        _nextSquareDist = (_currentSquare.gameObject.transform.position - transform.position).magnitude;
        _amplitude = Mathf.Min(_nextSquareDist * 0.1f, 0.5f);
    }

    public void SetDefaultAngle()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);
    }
}
