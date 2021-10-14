using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGameManager : MonoBehaviour
{
    enum Phase
    {
        INIT,
        WAIT_TURN_END,
        FADE_OUT,
        MOVE_CAMERA,
    }

    [SerializeField]
    private int _cardMinValue;

    [SerializeField]
    private int _cardMaxValue;

    [SerializeField]
    private int _initCardValue;

    [SerializeField]
    private List<CharacterControllerBase> _entryPlugs;

    [SerializeField]
    EarthMove _camera;

    private Phase _phase;

    private int _turnIndex;

    [SerializeField]
    SimpleFade _fade;

    [SerializeField]
    StatusWindow _statusWindow;

    private int _turnCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        _fade.FadeStart(30);

        UpdateTurn();

        // お小遣い移動カード初期化
        InitStatus();

        // 初回ターン
        InitTurn();
    }
    
    void UpdateTurn()
    {
        _turnCount++;
        _statusWindow.SetTurn(_turnCount);
    }

    // Update is called once per frame
    void Update()
    {
        if(_phase == Phase.INIT)
        {
            PhaseInit();
        }
        if(_phase == Phase.WAIT_TURN_END)
        {
            PhaseWaitTurnEnd();
        }
        if(_phase == Phase.FADE_OUT)
        {
            PhaseFadeOut();
        }
        if(_phase == Phase.MOVE_CAMERA)
        {
            PhaseMoveCamera();
        }
    }

    void PhaseInit()
    {
        _entryPlugs[_turnIndex].Character.AddMovingCard(GetRandomRange());
        //_entryPlugs[_turnIndex].Character.AddMovingCard(5);
        _entryPlugs[_turnIndex].Character.SetWaitEnable(false);
        _entryPlugs[_turnIndex].Move();
        _phase = Phase.WAIT_TURN_END;
    }

    void PhaseWaitTurnEnd()
    {
        if (_entryPlugs[_turnIndex].Character.State == CharacterState.END)
        {
            _phase = Phase.FADE_OUT;
            _fade.FadeStart(30, true);
        
        }
    }

    void PhaseFadeOut()
    {
        if (!_fade.IsFade)
        {
            _phase = Phase.MOVE_CAMERA;

            // 止まっているキャラクターの整列
            _entryPlugs[_turnIndex].Character.SetWaitEnable(true);
            _entryPlugs[_turnIndex].Character.CurrentSquare.AlignmentCharacters();

            _turnIndex++;
            if (_turnIndex >= _entryPlugs.Count)
            {
                _turnIndex = 0;

                // 合計ターン加算
                UpdateTurn();
            }

            //次の人の止まっているマス座標
            _camera.MoveToPosition(_entryPlugs[_turnIndex].Character.CurrentSquare.GetPosition(), 300);
        }
    }

    void PhaseMoveCamera()
    {
        if(_camera.State == EarthMove.EarthMoveState.END)
        {
            _phase = Phase.INIT;
            _fade.FadeStart(30);
        }
    }

    void InitTurn()
    {
        //_entryPlugs[_turnIndex].Character.AddMovingCard(GetRandomRange());
        _entryPlugs[_turnIndex].Character.AddMovingCard(3);
        _entryPlugs[_turnIndex].Character.Name = "こまち社長";
        _entryPlugs[_turnIndex].Move();
        _phase = Phase.WAIT_TURN_END;
    }

    void InitStatus()
    {
        var startSquare = GameObject.Find("Japan").GetComponent<SquareBase>();

        foreach (var p in _entryPlugs)
        {
            var chara = p.Character;
            //chara.AddMovingCard(5);
            //chara.AddMovingCard(4);
            //chara.AddMovingCard(3);
            
            for (int i = 0; i < _initCardValue; i++)
            {
                chara.AddMovingCard(GetRandomRange());
            }
            chara.Name = "敵" + Random.Range(1, 100) + "号";
            chara.SetCurrentSquare(startSquare);
        }

        _turnIndex = 0;

        
    }

    int GetRandomRange()
    {
        return Random.Range(_cardMinValue, _cardMaxValue);
    }
}
