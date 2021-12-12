using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareEnforcedGoal : SquareBase
{
    public enum SquareEnforcedGoalState
    {
        IDLE,
        PAY,
        GOAL,
        END,
    }
    private SquareEnforcedGoalState _state;
    public SquareEnforcedGoalState State => _state;

    private CharacterBase _character;
    private MessageWindow _messageWindow;
    private StatusWindow _statusWindow;
    private PayUI _payUI;

    private List<CharacterBase> _characters = new List<CharacterBase>();

    private int _moveNum;

    [SerializeField]
    private int _baseCost;
    private int _cost;


    public override string GetSquareInfo(CharacterBase character)
    {
        int displayCost = ComputeCost(character);

        _squareInfo =
            "強制ゴールマス\n" +
            "コスト：" + displayCost.ToString() + "\n";

        return _squareInfo;
    }

    //=============================

    void Start()
    {
        _messageWindow = FindObjectOfType<MessageWindow>();
        _statusWindow = FindObjectOfType<StatusWindow>();
        _payUI = FindObjectOfType<PayUI>();

        _characters.AddRange(FindObjectsOfType<CharacterBase>());

        ComputeMoveNum();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case SquareEnforcedGoalState.IDLE:
                break;
            case SquareEnforcedGoalState.PAY:
                PayStateProcess();
                break;
            case SquareEnforcedGoalState.GOAL:
                GoalStateProcess();
                break;
            case SquareEnforcedGoalState.END:
                EndStateProcess();
                break;
        }
    }


    public override void Stop(CharacterBase character)
    {
        _character = character;

        ComputeCost(_character);

        if (!_character.CanPay(_cost))
        {
            _messageWindow.SetMessage("お金が足りません", character);
            _state = SquareEnforcedGoalState.END;
            return;
        }

        var message = _cost.ToString() + "円を支払ってゴールしますか?";
        _messageWindow.SetMessage(message, character);
        _statusWindow.SetEnable(true);
        _payUI.Open(character);

        _state = SquareEnforcedGoalState.PAY;

        if (character.IsAutomatic)
        {
            Invoke("SelectAutomatic", 1.5f);
        }
    }

    private int ComputeCost(CharacterBase character)
    {
        //<LapCount,重複数>
        SortedDictionary<int, int> lapCounts = new SortedDictionary<int, int>();
        for (int i = 0; i < _characters.Count; i++)
        {
            int dummy;
            int lapCount = _characters[i].LapCount;
            if (lapCounts.TryGetValue(lapCount, out dummy))
            {
                lapCounts[lapCount]++;
            }
            else
            {
                lapCounts.Add(lapCount, 1);
            }
        }

        int invRank = 1;
        foreach (var lap in lapCounts)
        {
            if (lap.Key == character.LapCount)
                break;
            else
                invRank += lap.Value;
        }

        _cost = _baseCost * invRank * (character.LapCount + 1);
        return _cost;
    }

    private void ComputeMoveNum()
    {
        SquareBase searchSquare = OutConnects[0];
        _moveNum = 1;

        while(searchSquare != this)
        {
            if (searchSquare.name == "Japan")
                break;

            searchSquare = searchSquare.OutConnects[0];
            _moveNum++;
        }
    }

    private void SelectAutomatic()
    {
        _payUI.AISelectYes();
    }

    private void PayStateProcess()
    {
        if (_payUI.IsSelectComplete && !_messageWindow.IsDisplayed)
        {
            if (_payUI.IsSelectYes)
            {
                _character.Log.AddUseEventNum(SquareEventType.ENFORCED_GOAL);

                _character.SubMoney(_cost);

                _state = SquareEnforcedGoalState.GOAL;
            }
            else
            {
                _state = SquareEnforcedGoalState.END;

            }
        }
    }

    private void GoalStateProcess()
    {
        if(!_messageWindow.IsDisplayed)
        {
            _character.ReStartMove(_moveNum);
            _state = SquareEnforcedGoalState.END;
        }
    }

    private void EndStateProcess()
    {
        if (!_messageWindow.IsDisplayed)
        {
            // 止まる処理終了
            _character.CompleteStopExec();
            _statusWindow.SetEnable(false);

            _state = SquareEnforcedGoalState.IDLE;
        }
    }

    public override int GetScore(CharacterBase character, CharacterType characterType)
    {
        // コストが足りない
        if (_cost > character.Money) return base.GetScore(character, characterType);

        return (int)SquareScore.EGOAL + base.GetScore(character, characterType);
    }
}
