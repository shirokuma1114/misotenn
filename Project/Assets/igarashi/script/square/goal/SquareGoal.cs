using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareGoal : SquareBase
{
    public enum SquareGoalState
    {
        IDLE,
        GOAL,
        END,
    }
    private SquareGoalState _state;
    public SquareGoalState State => _state;


    private CharacterBase _character;
    private MessageWindow _messageWindow;
    private StatusWindow _statusWindow;
    
    [SerializeField]
    private int _baseMoney;
    public int BaseMoney => _baseMoney;

    // Start is called before the first frame update
    void Start()
    {
        _messageWindow = FindObjectOfType<MessageWindow>();
        _statusWindow = FindObjectOfType<StatusWindow>();

        _squareInfo =
            "�S�[���}�X\n";
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case SquareGoalState.IDLE:
                break;
            case SquareGoalState.END:
                EndStateProcess();
                break;
        }
    }


    public override void Stop(CharacterBase character)
    {
        _character = character;

        Goal(character);

        _state = SquareGoalState.END;
    }

    public void Goal(CharacterBase character)
    {
        _statusWindow.SetEnable(true);

        // ���񐔒ǉ�
        _character.LapCount++;
        _statusWindow.SetLapNum(_character.LapCount);

        int money = _baseMoney * character.LapCount;
        character.AddMoney(money);
        _statusWindow.SetMoney(_character.Money);

        var message = character.Name + "��" + character.LapCount.ToString() + "�T��\n" + money.ToString() + "�~�������";
        _messageWindow.SetMessage(message, character.IsAutomatic);
    }


    private void GoalStateProcess()
    {
        if(!_messageWindow.IsDisplayed)
        {
            _state = SquareGoalState.END;
        }       
    }

    private void EndStateProcess()
    {
        if (!_messageWindow.IsDisplayed)
        {
            _character.CompleteStopExec();
            _statusWindow.SetEnable(false);

            _state = SquareGoalState.IDLE;
        }
    }

    public override int GetScore(CharacterBase character, CharacterType characterType)
    {
        return (int)SquareScore.GOAL + base.GetScore(character, characterType);
    }
}
