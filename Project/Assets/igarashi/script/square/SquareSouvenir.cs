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

    private CharacterBase _character;
    private MessageWindow _messageWindow;
    private StatusWindow _statusWindow;
    private PayUI _payUI;
    private SouvenirWindow _souvenirWindow;

    private OmiyageEnshutsu _effect;

    private int _nowStock;


    [Header("お土産")]
    [Space(20)]
    [SerializeField]
    private string _souvenirName;
    [SerializeField]
    private int _cost;

    [SerializeField]
    private SouvenirType _type;

    [SerializeField]
    private int _startStock = 3;

    bool _isEffectUsed = false;    


    private void Awake()
    {
        _nowStock = _startStock;
    }

    // Start is called before the first frame update
    void Start()
    {
        _messageWindow = FindObjectOfType<MessageWindow>();
        _statusWindow = FindObjectOfType<StatusWindow>();
        _payUI = FindObjectOfType<PayUI>();
        _souvenirWindow = FindObjectOfType<SouvenirWindow>();

        _effect = FindObjectOfType<OmiyageEnshutsu>();

        _squareInfo =
            "お土産マス\n" +
            "コスト：" + _cost.ToString() + "\n" +
            "お土産名：" + _souvenirName + "\n" +
            "お土産タイプ：" + _type.ToString() + "\n" +
            "在庫数：" + _nowStock.ToString();
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

        //在庫チェック
        if (_nowStock <= 0)
        {
            _messageWindow.SetMessage("在庫がありません", character.IsAutomatic);
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

        //在庫更新
        _nowStock--;

        var buyMessage = _character.Name + "は\n" + "お土産  " + _souvenirName + "を　手に入れた！\n";
        if (_nowStock > 0)
        {
            buyMessage += "残りの在庫は  " + _nowStock.ToString() + "個";
        }
        else
        {
            buyMessage += "在庫が　なくなった！";
        }

        _messageWindow.SetMessage(buyMessage, _character.IsAutomatic);

        _souvenirWindow.SetSouvenirs(_character.Souvenirs);
        _souvenirWindow.SetEnable(true);

        //演出
        _effect.Use_OmiyageEnshutsu(gameObject.name);
        _isEffectUsed = true;

        _squareInfo =
            "お土産マス\n" +
            "コスト：" + _cost.ToString() + "\n" +
            "お土産名：" + _souvenirName + "\n" +
            "お土産タイプ：" + _type.ToString() + "\n" +
            "在庫数：" + _nowStock.ToString();

        _state = SquareSouvenirState.END;
    }

    private void EndProcess()
    {
        if (_isEffectUsed)
        {
            if (!_messageWindow.IsDisplayed && _effect.IsAnimComplete)
            {
                // 止まる処理終了
                _character.CompleteStopExec();
                _statusWindow.SetEnable(false);
                _souvenirWindow.SetEnable(false);

                _state = SquareSouvenirState.IDLE;
                _isEffectUsed = false;
            }
        }
        else
        {
            if (!_messageWindow.IsDisplayed)
            {
                // 止まる処理終了
                _character.CompleteStopExec();
                _statusWindow.SetEnable(false);

                _state = SquareSouvenirState.IDLE;
            }
        }
    }

    public override int GetScore(CharacterBase character, CharacterType characterType)
    {
        // お金が足りない
        if (_cost > character.Money) return base.GetScore(character, characterType);

        // 在庫がない
        if (_nowStock <= 0) return base.GetScore(character, characterType);

        // 持っていないお土産が売っている
        if (character.Souvenirs.Where(x => x.Type == _type).Count() == 0)return (int)SquareScore.DONT_HAVE_SOUVENIR + base.GetScore(character, characterType);

        return (int)SquareScore.SOUVENIR + base.GetScore(character, characterType);
    }
}
