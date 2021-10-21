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
    
    //<ƒLƒƒƒ‰ƒNƒ^[,Žü‰ñ”>
    private Dictionary<CharacterBase,int> _characterGoalNums = new Dictionary<CharacterBase, int>();

    [SerializeField]
    private int _baseMoney;

    [SerializeField]
    private int _roundBonus;

    // Start is called before the first frame update
    void Start()
    {
        _messageWindow = FindObjectOfType<MessageWindow>();
        _statusWindow = FindObjectOfType<StatusWindow>();


        var c = FindObjectsOfType<CharacterBase>();
        for(int i = 0; i < c.Length;i++)
            _characterGoalNums.Add(c[i], 0);
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case SquareGoalState.IDLE:
                break;
            case SquareGoalState.GOAL:
                GoalStateProcess();
                break;
            case SquareGoalState.END:
                EndStateProcess();
                break;
        }
    }


    public override void Stop(CharacterBase character)
    {
        _character = character;

        _messageWindow.SetMessage("“q‚¯‚éŠz‚ð‘I‘ð‚µ‚Ä‚­‚¾‚³‚¢", character.IsAutomatic);
        _statusWindow.SetEnable(true);

        _state = SquareGoalState.GOAL;
    }

    private void GoalStateProcess()
    {
        int money = _baseMoney + _roundBonus * _characterGoalNums[_character];
        _character.AddMoney(money);
        _characterGoalNums[_character]++;

        var message = _character.Name + "‚Í" + _characterGoalNums[_character].ToString() + "T–Ú\n" + money.ToString() + "‰~‚à‚ç‚Á‚½";
        _messageWindow.SetMessage(message, _character.IsAutomatic);

        _state = SquareGoalState.END;
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
}
