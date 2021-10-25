using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class CharacterBase : MonoBehaviour
{
    [SerializeField]
    CharacterControllerBase _controller;


    // ���O
    private string _name;

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    // ������
    private int _money;

    public int Money
    { get { return _money; } }


    // �ړ��J�[�h���X�g
    private List<int> _movingCards = new List<int>();
    public List<int> MovingCards
    { get { return _movingCards; } }

    // ���y�Y�J�[�h���X�g
    private List<Souvenir> _souvenirs = new List<Souvenir>();

    public List<Souvenir> Souvenirs
    { get { return _souvenirs; } }

    // ���ݎ~�܂��Ă���}�X
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

    // ���[�g�ۑ��p
    private Stack<SquareBase> _rootStack = new Stack<SquareBase>();

    private int _movingCount;

    public int MovingCount
    {
        get { return _movingCount; }
    }
    

    public bool IsAutomatic
    {
        get { return _controller.IsAutomatic; }
    }

    void Start()
    {
        
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

    // �X�^�[�g�̃}�X��ݒ�
    public void SetCurrentSquare(SquareBase square)
    {
        _currentSquare = square;
    }

    public void Init()
    {
        _state = CharacterState.WAIT;
        _currentSquare.RemoveCharacter(this);
        _rootStack.Clear();
    }

    public void SetWaitEnable(bool enable)
    {
        if (enable)
        {
            transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            transform.SetParent(_currentSquare.GetComponent<Transform>());
        }
        else
        {
            transform.SetParent(null);
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
    }

    public void StartMove(SquareBase square)
    {
        _state = CharacterState.MOVE;
        
        // ���
        if (_rootStack.Contains(square))
        {
            _rootStack.Pop();
            _movingCount++;
        }
        // �O�i
        else
        {
            _rootStack.Push(_currentSquare);
            _movingCount--;
        }
        
        _currentSquare = square;
        
        // �X�e�[�W��]
        FindObjectOfType<EarthMove>().MoveToPosition(_currentSquare.GetPosition(), 50.0f);
    }

    private void UpdateMove()
    {
        if (_state != CharacterState.MOVE) return;
        if (FindObjectOfType<EarthMove>().State == EarthMove.EarthMoveState.END)
        {
            _controller.SetRoot();
            _state = CharacterState.WAIT;
        }
    }

    public void Stop()
    {
        _state = CharacterState.STOP;
        _currentSquare.Stop(this);
        _currentSquare.AddCharacter(this);
    }

    public List<SquareBase> GetInConnects()
    {
        var outs = new List<SquareBase>();

        // �X�^�b�N�ɂ���}�X�̂�
        foreach (var s in _currentSquare.InConnects)
        {
            if (_rootStack.Contains(s))
            {
                outs.Add(s);
            }
        }
        
        return outs;
    }

    public List<SquareBase> GetOutConnects()
    {
        return _movingCount > 0 ? _currentSquare.OutConnects : new List<SquareBase>();
    }

    public void CompleteStopExec()
    {
        _state = CharacterState.END;
    }

    // ���݂̂��y�Y��ޏ�����
    public int GetSouvenirTypeNum()
    {
        var typeList = new bool[(int)SouvenirType.MAX_TYPE];

        // ���y�Y��S��ޑ����Ă���
        foreach (var x in _souvenirs)
        {
            typeList[(int)x.Type] = true;
        }
        return typeList.Where(x => x == true).Count();
    }
}
