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

    private CasinoGame _casinoGameUI;


    // Start is called before the first frame update
    void Start()
    {
        _messageWindow = FindObjectOfType<MessageWindow>();
        _statusWindow = FindObjectOfType<StatusWindow>();

        _selectUI = FindObjectOfType<SelectUI>();
        _selectElements = new List<string>();
        _betChoices = new List<int>();

        _casinoGameUI = FindObjectOfType<CasinoGame>();

        _squareInfo =
            "�J�W�m�}�X\n";
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


        //�����`�F�b�N
        if (_character.Money == 0)
        {
            _messageWindow.SetMessage("����������܂���", character);
            _state = SquareCasinoState.END;
            return;
        }

        _messageWindow.SetMessage("�q����z��I�����Ă�������", character);
        _statusWindow.SetEnable(true);


        int _maxBet = _character.Money;
        int _midBet = _character.Money / 2;
        int _minBet = _character.Money / 2 / 2;
        _bet = 0;
        _selectElements.Clear();
        _betChoices.Clear();

        _selectElements.Add(_minBet.ToString());
        _betChoices.Add(_minBet);
        _selectElements.Add(_midBet.ToString());
        _betChoices.Add(_midBet);
        _selectElements.Add(_maxBet.ToString());
        _betChoices.Add(_maxBet);
        _selectElements.Add("��߂�");

        _selectUI.Open(_selectElements,character);
        if(character.IsAutomatic)
            _selectUI.IndexSelect(Random.Range(0, 3));

        Camera.main.GetComponent<CameraInterpolation>().Enter_Event();


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

            _character.Log.AddUseEventNum(SquareEventType.CASINO);

            _bet = _betChoices[_selectUI.SelectIndex];
            _character.SubMoney(_bet);


            _casinoGameUI.Play(_character,_bet);
            _messageWindow.SetMessage("�����Ǝv�����̃J�[�h��I�����Ă�������", _character);

            _statusWindow.SetEnable(false);

            _state = SquareCasinoState.CHALLENGE;
            return;
        }
    }
    private void ChallengeStateProcess()
    {
        if (_messageWindow.IsDisplayed)
            return;
        if (!_casinoGameUI.IsComplate)
            return;

        if(_casinoGameUI.IsCorrectAnswer)
        {
            _messageWindow.SetMessage(_casinoGameUI.Reward.ToString() + "�~�l���I", _character);
            _character.AddMoney(_casinoGameUI.Reward);
        }
        else //�͂���
        {
            _messageWindow.SetMessage(_bet.ToString() + "�~�����܂���", _character);
        }


       _state = SquareCasinoState.END;
    }
    private void EndStateProcess()
    {
        if (!_messageWindow.IsDisplayed)
        {
            // �~�܂鏈���I��
            _character.CompleteStopExec();
            _statusWindow.SetEnable(false);

            Camera.main.GetComponent<CameraInterpolation>().Leave_Event();

            _state = SquareCasinoState.IDLE;
        }
    }

    public override int GetScore(CharacterBase character, CharacterType characterType)
    {
        if (character.Money == 0) return base.GetScore(character, characterType);

        return (int)SquareScore.CASINO + base.GetScore(character, characterType);
    }
}
