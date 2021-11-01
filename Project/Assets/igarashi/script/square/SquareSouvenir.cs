using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SquareSouvenir : SquareBase
{
    public enum SquareSouvenirState
    {
        IDLE,
        PAY_WAIT,
        EVENT,
        END,
    }
    protected SquareSouvenirState _state = SquareSouvenirState.IDLE;

    CharacterBase _character;
    MessageWindow _messageWindow;
    StatusWindow _statusWindow;
    PayUI _payUI;


    [Header("‚¨“yŽY")]
    [Space(20)]
    [SerializeField]
    private string _souvenirName;
    [SerializeField]
    private int _cost;

    [SerializeField]
    private SouvenirType _type;

    // Start is called before the first frame update
    void Start()
    {
        _messageWindow = FindObjectOfType<MessageWindow>();
        _statusWindow = FindObjectOfType<StatusWindow>();
        _payUI = FindObjectOfType<PayUI>();
    }

    public override void Stop(CharacterBase character)
    {
        _character = character;

        
        //‚¨‹àƒ`ƒFƒbƒN
        if (!_character.CanPay(_cost))
        {
            _messageWindow.SetMessage("‚¨‹à‚ª‘«‚è‚Ü‚¹‚ñ", character.IsAutomatic);
            _state = SquareSouvenirState.END;
            return;
        }


        var message = _cost.ToString() + "‰~‚ðŽx•¥‚Á‚Ä‚¨“yŽY‚ð”ƒ‚¢‚Ü‚·‚©H";

        _messageWindow.SetMessage(message, character.IsAutomatic);
        _statusWindow.SetEnable(true);
        _payUI.Open(character);

        _state = SquareSouvenirState.PAY_WAIT;
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case SquareSouvenirState.PAY_WAIT:
                PayWaitProcess();
                break;

            case SquareSouvenirState.EVENT:
                EventProcess();
                break;

            case SquareSouvenirState.END:
                EndProcess();
                break;
        }
    }




    private void PayWaitProcess()
    {
        if (_payUI.IsSelectComplete && !_messageWindow.IsDisplayed)
        {
            if (_payUI.IsSelectYes)
            {
                _state = SquareSouvenirState.EVENT;
            }
            else
            {
                _state = SquareSouvenirState.END;
            }
        }            
    }

    private void EventProcess()
    {
        _character.SubMoney(_cost);
        _character.AddSouvenir(new Souvenir(_cost, _souvenirName, _type));

        _messageWindow.SetMessage(_character.Name + "‚Í\n‚¨“yŽY‚ðŽè‚É“ü‚ê‚½", _character.IsAutomatic); //_character.name + "‚Í" + _souvenir.name + "‚ðŽè‚É“ü‚ê‚½"

        _state = SquareSouvenirState.END;
    }

    private void EndProcess()
    {

        if(!_messageWindow.IsDisplayed)
        {
            // Ž~‚Ü‚éˆ—I—¹
            _character.CompleteStopExec();
            _statusWindow.SetEnable(false);

            _state = SquareSouvenirState.IDLE;
        }
       
    }

    public override int GetScore(CharacterBase character)
    {
        // ‚¨‹à‚ª‘«‚è‚È‚¢
        if (_cost > character.Money) return base.GetScore(character);

        // Ž‚Á‚Ä‚¢‚È‚¢‚¨“yŽY‚ª”„‚Á‚Ä‚¢‚é
        if(character.Souvenirs.Where(x => x.Type == _type).Count() == 0)return (int)SquareScore.DONT_HAVE_SOUVENIR + base.GetScore(character);

        return (int)SquareScore.SOUVENIR + base.GetScore(character);
    }
}
