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

    private WarpHole _warpHole;
    private Tween _inholeTween;
    private Tween _outholeTween;
    private CameraInterpolation _camera;

    [SerializeField]
    private int _cost;
    [SerializeField]
    private GameObject _warpHolePrefab;
    [SerializeField]
    private Vector3 _warpHolePosition;


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

        _camera = Camera.main.GetComponent<CameraInterpolation>();


        _squareInfo =
            "���[�v�}�X\n" +
            "�R�X�g�F" + _cost.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(_characters[2].name + _characters[0].transform.position);
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


        //�����`�F�b�N
        if(!_character.CanPay(_cost))
        {
            _messageWindow.SetMessage("����������܂���", character.IsAutomatic);
            _state = SquareWarpState.END;
            return;
        }

        var message = _cost.ToString() + "�~���x�����đS���������_���Ƀ��[�v�����܂����H";
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
                _character.Log.AddUseEventNum(SquareEventType.WARP);


                _character.SubMoney(_cost);

                //���[�v�z�[���ɔ��ł�
                foreach (var chara in _characters)
                {
                    if (chara != _character)
                        chara.SetWaitEnable(false);

                    Vector3[] path =
                    {
                        chara.transform.position,
                        chara.transform.position + new Vector3(3.0f,5.0f,1.0f),
                        _warpHolePosition
                    };
                    _inholeTween = chara.transform.DOPath(path,3.0f,PathType.CatmullRom).SetEase(Ease.Linear);
                    _inholeTween.SetAutoKill(false);
                }

                //�G�t�F�N�g
                _warpHole = Instantiate(_warpHolePrefab, _warpHolePosition, new Quaternion(0, 0, 0, 0)).GetComponent<WarpHole>();

                //�J����
                _camera.Enter_Event();
                _camera.Set_NextCamera(2);

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
        if (_warpHole.State != WarpHole.WarpHoleState.WHITE_OPEN)
            return;

        _inholeTween.Kill();

        int flyCount = 0;
        Vector3[] flyAngle =
        {
            new Vector3(1.0f,0.0f,0.0f),
            new Vector3(0.0f,0.0f,1.0f),
            new Vector3(0.0f,0.0f,-1.0f),
            new Vector3(-1.0f,0.0f,0.0f),
        };
        foreach (var chara in _characters)
        {
            SquareBase randomSquare = _squares[Random.Range(0, _squares.Count)];           

            Vector3[] path =
            {
                chara.transform.position,
                chara.transform.position + flyAngle[flyCount] * 3.0f,
                randomSquare.transform.position
            };
            _outholeTween = chara.transform.DOPath(path, 3.0f, PathType.CatmullRom).SetEase(Ease.Linear);
            _outholeTween.SetAutoKill(false);
            
            chara.SetCurrentSquare(randomSquare);

            flyCount++;
        }

        _state = SquareWarpState.WARP_END;
    }

    private void WarpEndStateProcess()
    {
        if (_outholeTween.IsPlaying())
            return;

        _outholeTween.Kill();

        foreach (var chara in _characters)
        {
            chara.SetWaitEnable(true);

            chara.transform.up = chara.CurrentSquare.transform.up;
            chara.Alignment();
        }

        _warpHole.Close();

        _state = SquareWarpState.END;
    }

    private void EndStateProcess()
    {

        if (!_messageWindow.IsDisplayed)
        {
            _character.CompleteStopExec();
            _statusWindow.SetEnable(false);
            _camera.Leave_Event();

            _state = SquareWarpState.IDLE;
        }            
    }
    public override int GetScore(CharacterBase character, CharacterType characterType)
    {
        // ����������Ȃ�
        if (_cost > character.Money) return base.GetScore(character, characterType);

        // �������s��
        if (_gameManager.GetRank(character) > 2) return (int)SquareScore.HANDICAP_WARP + base.GetScore(character, characterType);

        return (int)SquareScore.WARP + base.GetScore(character, characterType);
    }
}