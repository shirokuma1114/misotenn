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
            "ã≠êßÉSÅ[ÉãÉ}ÉX\n" +
            "ÉRÉXÉgÅF" + displayCost.ToString() + "â~";

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
        _characters.AddRange(FindObjectsOfType<CharacterBase>());
        _character = character;

        ComputeCost(_character);

        if (!_character.CanPay(_cost))
        {
            _messageWindow.SetMessage("Ç®ã‡Ç™ë´ÇËÇ‹ÇπÇÒ", character);
            _state = SquareEnforcedGoalState.END;
            return;
        }

        var message = _cost.ToString() + "â~Çéxï•Ç¡ÇƒÉSÅ[ÉãÇµÇ‹Ç∑Ç©?";
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
        _cost = _baseCost * (character.LapCount + 1);
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
            // é~Ç‹ÇÈèàóùèIóπ
            _character.CompleteStopExec();
            _statusWindow.SetEnable(false);

            _state = SquareEnforcedGoalState.IDLE;
        }
    }

    public override int GetScore(CharacterBase character, CharacterType characterType)
    {
        // ÉRÉXÉgÇ™ë´ÇËÇ»Ç¢
        if (ComputeCost(character) > character.Money) return base.GetScore(character, characterType);

        return (int)SquareScore.EGOAL + base.GetScore(character, characterType);
    }
}
