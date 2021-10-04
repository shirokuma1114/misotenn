using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class MessageWindow : MonoBehaviour
{
    // 文字の表示速度
    public const float TEXT_SPEED = 0.05f;

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
    private bool _isEndMessage;
    
    private float _elapsedTime = 0.0f;

    private float _textSpeed;

    private float _iconPosY;

    // Start is called before the first frame update
    void Start()
    {
        var s = "小町社長は\nプラス駅に　止まった！<>";
        _iconPosY = _iconTr.anchoredPosition.y;
        _iconImage.enabled = false;
        _frameImage.enabled = false;
        _text.enabled = false;
        //SetMessage(s);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isDisplayed) return;

        _elapsedTime += Time.deltaTime;

        if (!_isOneMessage){
            if(_elapsedTime >= TEXT_SPEED)
            {
                _text.text += _splitMessage[_messageNum][_currentTextNum];
                _currentTextNum++;
                _elapsedTime = 0.0f;

                if (_currentTextNum >= _splitMessage[_messageNum].Length)
                {
                    _isOneMessage = true;
                    _iconImage.enabled = true;
                }
            }
        }
        else
        {
            var pos = _iconTr.anchoredPosition;
            pos.y = _iconPosY + Mathf.Sin(_elapsedTime * 8.0f) * 5.0f;
            _iconTr.anchoredPosition = pos;

            if (Input.GetKeyDown(KeyCode.Return))
            {
                _currentTextNum = 0;
                _messageNum++;
                _text.text = "";
                _elapsedTime = 0.0f;
                _isOneMessage = false;

                if (_messageNum >= _splitMessage.Length)
                {
                    _isOneMessage = true;
                    _isDisplayed = false;
                    _iconImage.enabled = false;
                    _frameImage.enabled = false;
                    _text.enabled = false;
                    //transform.GetChild(0).gameObject.SetActive();
                }
            }
        }
    }

    // 区切り文字を<>とする
    public void SetMessage(string message, float textSpeed = TEXT_SPEED)
    {
        _isDisplayed = true;
        _splitMessage = Regex.Split(message, @"\s*" + _splitString + @"\s*", RegexOptions.IgnorePatternWhitespace);
        _currentTextNum = 0;
        _messageNum = 0;
        _text.text = "";
        _isOneMessage = false;
        _isEndMessage = false;
        _elapsedTime = 0.0f;
        _textSpeed = textSpeed;
        _frameImage.enabled = true;
        _text.enabled = true;
    }
}
