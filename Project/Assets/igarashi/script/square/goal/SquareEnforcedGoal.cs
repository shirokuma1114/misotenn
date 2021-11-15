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

    private SquareGoal _squareGoal;

    [SerializeField]
    private int _cost;


    // Start is called before the first frame update
    void Start()
    {
        _messageWindow = FindObjectOfType<MessageWindow>();
        _statusWindow = FindObjectOfType<StatusWindow>();
        _payUI = FindObjectOfType<PayUI>();
        _squareGoal = FindObjectOfType<SquareGoal>();
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

        if (!_character.CanPay(_cost))
        {
            _messageWindow.SetMessage("Ç®ã‡Ç™ë´ÇËÇ‹ÇπÇÒ", character.IsAutomatic);
            _state = SquareEnforcedGoalState.END;
            return;
        }

        var message = _cost.ToString() + "â~Çéxï•Ç¡ÇƒÉSÅ[ÉãÇµÇ‹Ç∑Ç©?";
        _messageWindow.SetMessage(message, character.IsAutomatic);
        _statusWindow.SetEnable(true);
        _payUI.Open(character);

        _state = SquareEnforcedGoalState.PAY;
    }


    private void PayStateProcess()
    {
        if (_payUI.IsSelectComplete && !_messageWindow.IsDisplayed)
        {
            if (_payUI.IsSelectYes)
            {
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
            _squareGoal.Goal(_character);
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
        if (_cost < character.Money) return base.GetScore(character, characterType);

        return (int)SquareScore.EGOAL + base.GetScore(character, characterType);
    }
}
