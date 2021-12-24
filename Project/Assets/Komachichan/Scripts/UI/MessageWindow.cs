using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class MessageWindow : WindowBase
{
    // 文字の表示速度
    public const float DEFAULT_TEXT_SPEED = 30.0f;
    public const float TEXT_SPEED_MIN = DEFAULT_TEXT_SPEED;
    public const float TEXT_SPEED_MAX = 120.0f;

    public static readonly float CLICK_FLASH_TIME = 0.2f;
    
    [SerializeField]
    Text _text;

    [SerializeField]
    MessageFrame _frame;

    [SerializeField]
    Image _frameImage;

    [SerializeField]
    Image _iconImage;

    [SerializeField]
    RectTransform _iconTr;

    bool _isAutomatic;

    CharacterBase _character;



    private bool _isDisplayed = false;
    public bool IsDisplayed
    {
        get { return _isDisplayed; }
    }

    private string _splitString = "<>";
    private string[] _splitMessage;

    private int _currentTextNum;
    private int _messageNum;

    private bool _isOneMessage;
    
    private float _elapsedTime = 0.0f;

    private float _textSpeed;

    private float _iconPosY;

    private bool _playerInput;

    // Start is called before the first frame update
    void Start()
    {
        _iconPosY = _iconTr.anchoredPosition.y;
        _iconImage.enabled = false;
        _frameImage.enabled = false;
        _text.enabled = false;
        _textSpeed = FindObjectOfType<DontDestroyManager>().TextSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isDisplayed) return;

        _elapsedTime += _textSpeed * Time.deltaTime;

        if (!_isOneMessage){
            if(_elapsedTime >= 1f)
            {
                _text.text += _splitMessage[_messageNum][_currentTextNum];
                _currentTextNum++;
                _elapsedTime = 0.0f;

                if (_currentTextNum >= _splitMessage[_messageNum].Length)
                {
                    _isOneMessage = true;
                    _iconImage.enabled = true;
                    if (_isAutomatic)
                    {
                        Invoke("SetNextMessage", 1.0f);
                    }
                }
            }
        }
        else
        {
            var pos = _iconTr.anchoredPosition;
            pos.y = _iconPosY + Mathf.Sin(Time.time * 8.0f) * 5.0f;
            _iconTr.anchoredPosition = pos;

        }

        if(_playerInput && (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("A")))
        {
            SetNextMessage();
        }
        if (_playerInput) return;

        if (!_isAutomatic && (Input.GetKeyDown(KeyCode.Return) || _character.Input.GetButtonDown("A")))
        {
            SetNextMessage();
        }
    }

    private void SetNextMessage()
    {
        Control_SE.Get_Instance().Play_SE("UI_Select");
        _currentTextNum = 0;
        _messageNum++;
        _text.text = "";
        _elapsedTime = 0.0f;
        _isOneMessage = false;
        _iconImage.enabled = false;
        if (_messageNum >= _splitMessage.Length)
        {
            _isOneMessage = true;
            _isDisplayed = false;
            _frameImage.enabled = false;
            _text.enabled = false;
        }
    }

    // 区切り文字を<>とする
    public void SetMessage(string message, CharacterBase character, bool playerInput = false)
    {
        _isDisplayed = true;
        _splitMessage = Regex.Split(message, @"\s*" + _splitString + @"\s*", RegexOptions.IgnorePatternWhitespace);
        _currentTextNum = 0;
        _messageNum = 0;
        _text.text = "";
        _isOneMessage = false;
        _elapsedTime = 0.0f;
        _frameImage.enabled = true;
        _text.enabled = true;
        _isAutomatic = character.IsAutomatic;
        _character = character;
        _playerInput = playerInput;
        if (_playerInput)
        {
            _isAutomatic = false;
        }
    }

    public void SetTextSpeed(float textSpeed)
    {
        _textSpeed = textSpeed;
    }
}
