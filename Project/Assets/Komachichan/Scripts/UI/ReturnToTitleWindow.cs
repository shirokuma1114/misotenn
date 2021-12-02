using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReturnToTitleWindow : WindowBase
{
    [SerializeField]
    private Image _cursorImage;
    [SerializeField]
    private RectTransform _cursorRt;


    [SerializeField]
    private Image _frameImage;

    [SerializeField]
    private Text _text;

    [SerializeField]
    private Text _yesText;
    
    [SerializeField]
    private Text _noText;

    [SerializeField]
    private WindowBase _backToWindow;

    bool _enable;

    bool _selectedYes;

    [SerializeField]
    Animator _fadeAnimation;

    bool _isFade;

    public override void SetEnable(bool enable)
    {
        _enable = enable;
        _cursorImage.enabled = enable;
        _frameImage.enabled = enable;
        _text.enabled = enable;
        _yesText.enabled = enable;
        _noText.enabled = enable;

        if(enable)
        {
            _selectedYes = false;
            UpdateCursor();
            return;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        SetEnable(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_enable) return;

        if (_isFade && _fadeAnimation.GetCurrentAnimatorClipInfo(0)[0].clip.name == "FadeOut" && _fadeAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            SceneManager.LoadScene("Title");
            SetEnable(false);
        }
        if (_isFade) return;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            _selectedYes = !_selectedYes;
            UpdateCursor();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (_selectedYes)
            {
                _fadeAnimation.Play("FadeOut");
                _isFade = true;
                return;
            }
            Invoke("BackToWindow", 0.01f);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Invoke("BackToWindow", 0.01f);
        }
    }

    void BackToWindow()
    {
        _backToWindow.SetEnable(true);
        SetEnable(false);
    }

    void UpdateCursor()
    {
        var pos = _cursorRt.anchoredPosition;
        if (_selectedYes)
        {
            pos.x = _yesText.GetComponent<RectTransform>().anchoredPosition.x - 7;
            _cursorRt.anchoredPosition = pos;
            return;
        }
        pos.x = _noText.GetComponent<RectTransform>().anchoredPosition.x - 10;
        _cursorRt.anchoredPosition = pos;
    }
}
