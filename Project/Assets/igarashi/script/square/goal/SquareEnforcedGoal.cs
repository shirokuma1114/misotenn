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

    [SerializeField]
    private int _cost;


    // Start is called before the first frame update
    void Start()
    {
        _messageWindow = FindObjectOfType<MessageWindow>();
        _statusWindow = FindObjectOfType<StatusWindow>();
        _payUI = FindObjectOfType<PayUI>();
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
            _messageWindow.SetMessage("お金が足りません", character.IsAutomatic);
            _state = SquareEnforcedGoalState.END;
            return;
        }

        var message = _cost.ToString() + "円を支払ってゴールしますか?";
        _messageWindow.SetMessage(message, character.IsAutomatic);
        _statusWindow.SetEnable(true);
        _payUI.Open(character);

        _state = SquareEnforcedGoalState.PAY;

        if (character.IsAutomatic)
        {
            Invoke("SelectAutomatic", 1.5f);
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
            // 13マス進めさせる
            _character.ReStartMove(13);
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
        if (_cost < character.Money) return base.GetScore(character, characterType);

        return (int)SquareScore.EGOAL + base.GetScore(character, characterType);
    }
}
