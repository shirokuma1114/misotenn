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


    [Header("お土産")]
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

        _squareInfo =
            "お土産マス\n" +
            "コスト：" + _cost.ToString() + "\n" +
            "お土産名：" + _souvenirName + "\n" +
            "お土産タイプ：" + _type.ToString(); 
    }

    public override void Stop(CharacterBase character)
    {
        _character = character;

        
        //お金チェック
        if (!_character.CanPay(_cost))
        {
            _messageWindow.SetMessage("お金が足りません", character.IsAutomatic);
            _state = SquareSouvenirState.END;
            return;
        }


        var message = _cost.ToString() + "円を支払って\nお土産　" + _souvenirName + "を　買いますか？";

        _messageWindow.SetMessage(message, character.IsAutomatic);
        _statusWindow.SetEnable(true);
        _payUI.Open(character);

        _state = SquareSouvenirState.PAY_WAIT;

        if (character.IsAutomatic)
        {
            Invoke("SelectAutomatic", 1.5f);
        }
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

    void SelectAutomatic()
    {
        _payUI.AISelectYes();
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

        _messageWindow.SetMessage(_character.Name + "は\nお土産　" + _souvenirName + "を　手に入れた！", _character.IsAutomatic); //_character.name + "は" + _souvenir.name + "を手に入れた"

        _state = SquareSouvenirState.END;
    }

    private void EndProcess()
    {

        if(!_messageWindow.IsDisplayed)
        {
            // 止まる処理終了
            _character.CompleteStopExec();
            _statusWindow.SetEnable(false);

            _state = SquareSouvenirState.IDLE;
        }
       
    }

    public override int GetScore(CharacterBase character, CharacterType characterType)
    {
        // お金が足りない
        if (_cost > character.Money) return base.GetScore(character, characterType);

        // 持っていないお土産が売っている
        if(character.Souvenirs.Where(x => x.Type == _type).Count() == 0)return (int)SquareScore.DONT_HAVE_SOUVENIR + base.GetScore(character, characterType);

        return (int)SquareScore.SOUVENIR + base.GetScore(character, characterType);
    }
}
