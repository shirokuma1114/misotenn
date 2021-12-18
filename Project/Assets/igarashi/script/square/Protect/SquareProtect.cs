using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareProtect : SquareBase
{
    public enum SquareProtectState
    {
        IDLE,
        PAY,
        PROTECT,
        END,
    }
    private SquareProtectState _state;
    public SquareProtectState State => _state;


    CharacterBase _character;
    MessageWindow _messageWindow;
    StatusWindow _statusWindow;
    PayUI _payUI;

    private ProtectEffect _protectEffect;

    [SerializeField]
    private int _cost;

    [SerializeField]
    private int _protectTurn;

    [SerializeField]
    private GameObject _protectEffectPrefab;


    // Start is called before the first frame update
    void Start()
    {
        _messageWindow = FindObjectOfType<MessageWindow>();
        _statusWindow = FindObjectOfType<StatusWindow>();
        _payUI = FindObjectOfType<PayUI>();

        _squareInfo =
            "プロテクトマス\n" +
            "コスト：" + _cost.ToString() + "円" + "\n" +
            "守られるターン数：" + _protectTurn.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case SquareProtectState.IDLE:
                break;
            case SquareProtectState.PAY:
                PayStateProcess();
                break;
            case SquareProtectState.PROTECT:
                ProtectStateProcess();
                break;
            case SquareProtectState.END:
                EndStateProcess();
                break;
        }
    }

    private void SelectAutomatic()
    {
        if (_character.Souvenirs.Count == 0)
        {
            _payUI.AISelectNo();
            return;
        }
        _payUI.AISelectYes();
    }

    public override void Stop(CharacterBase character)
    {
        _character = character;

        if (!_character.CanPay(_cost))
        {
            _messageWindow.SetMessage("お金が足りません", character);
            _state = SquareProtectState.END;
            return;
        }

        var message = _cost.ToString() + "円を支払って" + _protectTurn.ToString() + "ターンの間身を守りますか？";
        _messageWindow.SetMessage(message, character);
        _statusWindow.SetEnable(true);
        _payUI.Open(character);

        Camera.main.GetComponent<CameraInterpolation>().Enter_Event();

        _state = SquareProtectState.PAY;

        if (character.IsAutomatic)
        {
            Invoke("SelectAutomatic", 1.5f);
        }
    }


    private void PayStateProcess()
    {
        if (_payUI.IsSelectComplete && !_messageWindow.IsDisplayed)
        {
            if (_payUI.IsSelectYes)
            {
                _character.Log.AddUseEventNum(SquareEventType.PROTECT);
                _character.SubMoney(_cost);

                _protectEffect = Instantiate(_protectEffectPrefab, _character.transform.position, _character.transform.rotation).GetComponent<ProtectEffect>();
                _protectEffect.transform.SetParent(_character.transform);

                _state = SquareProtectState.PROTECT;
            }
            else
            {
                _state = SquareProtectState.END;
            }
        }
    }

    private void ProtectStateProcess()
    {
        if (!_protectEffect.IsEnd)
            return;

        _character.GetComponent<Protector>().ProtectStart(_protectTurn,_protectEffect);
        _protectEffect = null;

        _state = SquareProtectState.END;
    }

    private void EndStateProcess()
    {
        if (!_messageWindow.IsDisplayed)
        {
            _character.CompleteStopExec();
            _statusWindow.SetEnable(false);

            Camera.main.GetComponent<CameraInterpolation>().Leave_Event();

            _state = SquareProtectState.IDLE;
        }
    }
}