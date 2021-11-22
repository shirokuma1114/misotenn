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

    private OmiyageEnshutsu _effect;

    private int _nowStock;


    [Header("Ç®ìyéY")]
    [Space(20)]
    [SerializeField]
    private string _souvenirName;
    [SerializeField]
    private int _cost;

    [SerializeField]
    private SouvenirType _type;

    [SerializeField]
    private int _startStock = 3;


    private void Awake()
    {
        _nowStock = _startStock;
    }

    void Start()
    {
        _messageWindow = FindObjectOfType<MessageWindow>();
        _statusWindow = FindObjectOfType<StatusWindow>();
        _payUI = FindObjectOfType<PayUI>();

        _effect = FindObjectOfType<OmiyageEnshutsu>();

        _squareInfo =
            "Ç®ìyéYÉ}ÉX\n"   +
            "ÉRÉXÉgÅF"       + _cost.ToString()  + "\n" +
            "Ç®ìyéYñºÅF"     + _souvenirName     + "\n" +
            "Ç®ìyéYÉ^ÉCÉvÅF" + _type.ToString()  + "\n" +
            "ç›å…êîÅF"       + _nowStock.ToString();
    }

    public override void Stop(CharacterBase character)
    {
        _character = character;

        
        //Ç®ã‡É`ÉFÉbÉN
        if (!_character.CanPay(_cost))
        {
            _messageWindow.SetMessage("Ç®ã‡Ç™ë´ÇËÇ‹ÇπÇÒ", character.IsAutomatic);
            _state = SquareSouvenirState.END;
            return;
        }

        //ç›å…É`ÉFÉbÉN
        if(_nowStock <= 0)
        {
            _messageWindow.SetMessage("ç›å…Ç™Ç†ÇËÇ‹ÇπÇÒ", character.IsAutomatic);
            _state = SquareSouvenirState.END;
            return;
        }


        var message = _cost.ToString() + "â~Çéxï•Ç¡Çƒ\nÇ®ìyéYÅ@" + _souvenirName + "ÇÅ@îÉÇ¢Ç‹Ç∑Ç©ÅH";

        _messageWindow.SetMessage(message, character.IsAutomatic);
        _statusWindow.SetEnable(true);
        _payUI.Open(character);

        _state = SquareSouvenirState.PAY_WAIT;

        if (character.IsAutomatic)
        {
            Invoke("SelectAutomatic", 2.0f);
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
        _statusWindow.SetMoney(_character.Money);
        _character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(_type));

        var buyMessage =
            _character.Name + "ÇÕ\n" + 
            "Ç®ìyéY  " + _souvenirName + "ÇÅ@éËÇ…ì¸ÇÍÇΩÅI\n" +
            "ç›å…ÇÕ  " + _nowStock.ToString() + "å¬";

        _messageWindow.SetMessage(buyMessage, _character.IsAutomatic);

        //ââèo
        _effect.Use_OmiyageEnshutsu(gameObject.name);

        //ç›å…çXêV
        _nowStock--;
        _squareInfo =
            "Ç®ìyéYÉ}ÉX\n" +
            "ÉRÉXÉgÅF" + _cost.ToString() + "\n" +
            "Ç®ìyéYñºÅF" + _souvenirName + "\n" +
            "Ç®ìyéYÉ^ÉCÉvÅF" + _type.ToString() + "\n" +
            "ç›å…êîÅF" + _nowStock.ToString();

        _state = SquareSouvenirState.END;
    }

    private void EndProcess()
    {

        if(!_messageWindow.IsDisplayed && _effect.IsAnimComplete)
        {
            // é~Ç‹ÇÈèàóùèIóπ
            _character.CompleteStopExec();
            _statusWindow.SetEnable(false);

            _state = SquareSouvenirState.IDLE;
        }
       
    }

    public override int GetScore(CharacterBase character, CharacterType characterType)
    {
        // Ç®ã‡Ç™ë´ÇËÇ»Ç¢
        if (_cost > character.Money) return base.GetScore(character, characterType);

        // éùÇ¡ÇƒÇ¢Ç»Ç¢Ç®ìyéYÇ™îÑÇ¡ÇƒÇ¢ÇÈ
        if(character.Souvenirs.Where(x => x.Type == _type).Count() == 0)return (int)SquareScore.DONT_HAVE_SOUVENIR + base.GetScore(character, characterType);

        return (int)SquareScore.SOUVENIR + base.GetScore(character, characterType);
    }
}
