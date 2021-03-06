using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareCasino : SquareBase
{
    public enum SquareCasinoState
    {
        IDLE,
        PAY,
        CHALLENGE,
        END,
    }
    private SquareCasinoState _state;
    public SquareCasinoState State => _state;

    private CharacterBase _character;
    private MessageWindow _messageWindow;
    private StatusWindow _statusWindow;

    private SelectUI _selectUI;
    private List<string> _selectElements;
    private List<int> _betChoices;
    private int _bet;

    [SerializeField]
    private int _percentage;
    [SerializeField]
    private int _rate;

    // Start is called before the first frame update
    void Start()
    {
        _messageWindow = FindObjectOfType<MessageWindow>();
        _statusWindow = FindObjectOfType<StatusWindow>();

        _selectUI = FindObjectOfType<SelectUI>();
        _selectElements = new List<string>();
        _betChoices = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case SquareCasinoState.IDLE:
                break;
            case SquareCasinoState.PAY:
                PayStateProcess();
                break;
            case SquareCasinoState.CHALLENGE:
                ChallengeStateProcess();
                break;
            case SquareCasinoState.END:
                EndStateProcess();
                break;
        }
    }


    public override void Stop(CharacterBase character)
    {
        _character = character;


        //お金チェック
        if (_character.Money == 0)
        {
            _messageWindow.SetMessage("お金がありません", character.IsAutomatic);
            _state = SquareCasinoState.END;
            return;
        }

        _messageWindow.SetMessage("賭ける額を選択してください", character.IsAutomatic);
        _statusWindow.SetEnable(true);


        int _maxBet = _character.Money;
        int _midBet = _character.Money / 2;
        int _minBet = _character.Money / 2 / 2;
        _bet = 0;
        _selectElements.Clear();

        _selectElements.Add(_minBet.ToString());
        _betChoices.Add(_minBet);
        _selectElements.Add(_midBet.ToString());
        _betChoices.Add(_midBet);
        _selectElements.Add(_maxBet.ToString());
        _betChoices.Add(_maxBet);
        _selectElements.Add("やめる");

        _selectUI.Open(_selectElements);


        _state = SquareCasinoState.PAY;
    }

    private void PayStateProcess()
    {
        if(_selectUI.IsComplete && !_messageWindow.IsDisplayed)
        {
            if(_selectUI.SelectIndex == _selectElements.Count - 1)
            {
                _state = SquareCasinoState.END;

                return;
            }


            _bet = _betChoices[_selectUI.SelectIndex];
            _character.SubMoney(_bet);

            _state = SquareCasinoState.CHALLENGE;
        }
    }
    private void ChallengeStateProcess()
    {
        int rand = Random.Range(1, 100);
        
        if(1 <= rand && _percentage >= rand) //当たり
        {
            _messageWindow.SetMessage("当たり!!\n" + (_bet * _rate).ToString() + "円になりました", _character.IsAutomatic);
            _character.AddMoney(_bet * _rate);
        }
        else //はずれ
        {
            _messageWindow.SetMessage("はずれ\n" + _bet.ToString() + "失いました", _character.IsAutomatic);
        }

        _state = SquareCasinoState.END;
    }
    private void EndStateProcess()
    {
        if (!_messageWindow.IsDisplayed)
        {
            // 止まる処理終了
            _character.CompleteStopExec();
            _statusWindow.SetEnable(false);

            _state = SquareCasinoState.IDLE;
        }
    }
}
