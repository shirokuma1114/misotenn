using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SquareWarp : SquareBase
{
    public enum SquareWarpState
    {
        IDLE,
        PAY,
        WARP,
        WARP_END,
        END,
    }
    private SquareWarpState _state;
    public SquareWarpState State => _state;

    private CharacterBase _character;
    private MessageWindow _messageWindow;
    private StatusWindow _statusWindow;
    private PayUI _payUI;
    private SimpleFade _fade;

    private List<SquareBase> _squares;

    private List<CharacterBase> _characters;

    private Tween _inholeTween;
    private Tween _outholeTween;

    [SerializeField]
    private int _cost;
    [SerializeField]
    private Vector3 _warpholePosition;


    MyGameManager _gameManager;
    // Start is called before the first frame update
    void Awake()
    {
        _messageWindow = FindObjectOfType<MessageWindow>();
        _statusWindow = FindObjectOfType<StatusWindow>();
        _payUI = FindObjectOfType<PayUI>();
        _fade = FindObjectOfType<SimpleFade>();

        _characters = new List<CharacterBase>();
        _characters.AddRange(FindObjectsOfType<CharacterBase>());

        _squares = new List<SquareBase>();
        _squares.AddRange(FindObjectsOfType<SquareBase>());

        _gameManager = FindObjectOfType<MyGameManager>();


        _squareInfo =
            "ワープマス\n" +
            "コスト：" + _cost.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case SquareWarpState.IDLE:
                break;
            case SquareWarpState.PAY:
                PayStateProcess();
                break;
            case SquareWarpState.WARP:
                WarpStateProcess();
                break;
            case SquareWarpState.WARP_END:
                WarpEndStateProcess();
                break;

            case SquareWarpState.END:
                EndStateProcess();
                break;
        }
    }


    public override void Stop(CharacterBase character)
    {
        _character = character;


        //お金チェック
        if(!_character.CanPay(_cost))
        {
            _messageWindow.SetMessage("お金が足りません", character.IsAutomatic);
            _state = SquareWarpState.END;
            return;
        }

        var message = _cost.ToString() + "円を支払って全員をランダムにワープさせますか？";
        _messageWindow.SetMessage(message,character.IsAutomatic);
        _statusWindow.SetEnable(true);
        _payUI.Open(character);

        _state = SquareWarpState.PAY;
    }



    private void PayStateProcess()
    {
        if (_payUI.IsSelectComplete && !_messageWindow.IsDisplayed)
        {
            if (_payUI.IsSelectYes)
            {
                _character.SubMoney(_cost);
                foreach (var chara in _characters)
                {
                    chara.SetWaitEnable(false);
                    _inholeTween = chara.transform.DOMove(_warpholePosition, 5.0f);
                }

                _state = SquareWarpState.WARP;
            }
            else
            {
                _state = SquareWarpState.END;
            }
        }
    }

    private void WarpStateProcess()
    {
        if (_inholeTween.IsPlaying())
            return;

        _inholeTween.Kill();

        foreach (var chara in _characters)
        {
            SquareBase randomSquare = _squares[Random.Range(0, _squares.Count)];
            _outholeTween = chara.transform.DOMove(randomSquare.transform.position, 5.0f);
            chara.SetCurrentSquare(randomSquare);
        }

        _state = SquareWarpState.WARP_END;
    }

    private void WarpEndStateProcess()
    {
        if (_outholeTween.IsPlaying())
            return;

        foreach (var chara in _characters)
        {
            chara.SetWaitEnable(true);

            chara.transform.up = chara.CurrentSquare.transform.up;
            chara.Alignment();
        }

        _state = SquareWarpState.END;
    }

    private void EndStateProcess()
    {

        if (!_messageWindow.IsDisplayed)
        {
            _character.CompleteStopExec();
            _statusWindow.SetEnable(false);

            _state = SquareWarpState.IDLE;
        }            
    }
    public override int GetScore(CharacterBase character, CharacterType characterType)
    {
        // お金が足りない
        if (_cost > character.Money) return base.GetScore(character, characterType);

        // 自分が不利
        if (_gameManager.GetRank(character) > 2) return (int)SquareScore.HANDICAP_WARP + base.GetScore(character, characterType);

        return (int)SquareScore.WARP + base.GetScore(character, characterType);
    }
}
