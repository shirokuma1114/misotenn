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

    private string _souvenirName;
    private int _cost;

    private int _nowStock;

    [Header("お土産のタイプ")]
    [Space(20)]
    [SerializeField]
    private SouvenirType _type;

    [Header("在庫数")]
    [SerializeField]
    private int _startStock = 3;

    bool _isEffectUsed = false;


    public override string GetSquareInfo(CharacterBase character)
    {
        _squareInfo =
            "お土産マス" + "<" + _souvenirName + ">\n" +
            "コスト：" + _cost.ToString() + "円" + "\n" +
            "在庫数：" + _nowStock.ToString();

        return _squareInfo;
    }

    //=========================

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

        var souve = SouvenirCreater.Instance.ReferenceSouvenirParameter(_type);
        _cost = souve.Price;
        _souvenirName = souve.Name;

        _squareInfo =
            "お土産マス" + "<" + _souvenirName + ">\n" +
            "コスト：" + _cost.ToString() + "円" + "\n" +
            "在庫数：" + _nowStock.ToString();
    }

    public override void Stop(CharacterBase character)
    {
        _character = character;
        
        //お金チェック
        if (!_character.CanPay(_cost))
        {
            _messageWindow.SetMessage("お金が足りません", character);
            _state = SquareSouvenirState.END;
            return;
        }

        //在庫チェック
        if (_nowStock <= 0)
        {
            _messageWindow.SetMessage("在庫がありません", character);
            _state = SquareSouvenirState.END;
            return;
        }


        var message = _cost.ToString() + "円を支払って\nお土産　" + _souvenirName + "を　買いますか？";

        _messageWindow.SetMessage(message, character);
        _statusWindow.SetEnable(true);
        _payUI.Open(character,true);

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
        _statusWindow.SetMoney(_character.Money);

        Souvenir souvenir = SouvenirCreater.Instance.CreateSouvenir(_type);
        _character.AddSouvenir(souvenir);

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

        _messageWindow.SetMessage(buyMessage, _character);

        _souvenirWindow.SetSouvenirs(_character.Souvenirs);
        _souvenirWindow.SetEnable(true);

        //演出
        _effect.Use_OmiyageEnshutsu(souvenir.Sprite);
        _isEffectUsed = true;

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
