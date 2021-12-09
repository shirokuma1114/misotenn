using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SelectWindow : WindowBase
{
    [SerializeField]
    Image _cursorImage;

    [SerializeField]
    RectTransform _cursorTr;

    [SerializeField]
    Image _frame;
    
    List<Text> _selectTexts;

    [SerializeField]
    List<WindowBase> _selectWindows;

    [SerializeField]
    SelectWindow _backToWindow;

    int _selectIndex;

    bool _enable;

    float _textY;

    CharacterBase _character;

    bool _automaticMode;

    bool _calledSelectAuto;

    private float beforeTrigger;
    // Start is called before the first frame update
    void Start()
    {
        _selectTexts = transform.GetComponentsInChildren<Text>().ToList();
        _textY = _selectTexts.First().gameObject.GetComponent<RectTransform>().anchoredPosition.y;
        SetEnable(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!_enable) return;

        if (!_calledSelectAuto && _automaticMode)
        {
            _calledSelectAuto = true;
            Invoke("SelectAuto", 1.0f);
        }
        if (_automaticMode) return;

        bool move = false;

        float viewButton = _character.Input.GetAxis("Vertical");
        //Debug.Log(viewButton);

        //RTが押されたら視点を変える：前フレームの入力値が0の場合のみ実施
        if (beforeTrigger == 0.0f)
        {
            if(viewButton < 0)
            {
                _selectIndex = Mathf.Max(0, --_selectIndex);
                move = true;

                Control_SE.Get_Instance().Play_SE("UI_Select");
            }
            if(viewButton > 0)
            {
                _selectIndex = Mathf.Min(_selectTexts.Count - 1, ++_selectIndex);
                move = true;

                Control_SE.Get_Instance().Play_SE("UI_Select");
            }
        }
        beforeTrigger = viewButton;

        if (Input.GetKeyDown(KeyCode.W))
        {
            _selectIndex = Mathf.Max(0, --_selectIndex);
            move = true;

            if(Control_SE.Get_Instance())Control_SE.Get_Instance().Play_SE("UI_Select");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            _selectIndex = Mathf.Min(_selectTexts.Count - 1, ++_selectIndex);
            move = true;

            if(Control_SE.Get_Instance())Control_SE.Get_Instance().Play_SE("UI_Select");
        }

        if (Input.GetKeyDown(KeyCode.Return) || _character.Input.GetButtonDown("A"))
        {
            Invoke("ShowWindow", 0.001f);
            //ShowWindow();

            if(Control_SE.Get_Instance())Control_SE.Get_Instance().Play_SE("UI_Correct");
        }

        if (_backToWindow && (Input.GetKeyDown(KeyCode.Escape) || _character.Input.GetButtonDown("B")))
        {
            Invoke("BackWindow", 0.001f);

            if(Control_SE.Get_Instance())Control_SE.Get_Instance().Play_SE("UI_Close");
        }

        if (move)
        {
            _cursorTr.anchoredPosition = new Vector3(_cursorTr.anchoredPosition.x, _textY - _selectIndex * 30.0f, 1);
        }
    }

    public override void SetEnable(bool enable)
    {
        _frame.enabled = enable;
        foreach(var x in _selectTexts)
        {
            x.enabled = enable;
        }
        _cursorImage.enabled = enable;

        _enable = enable;
    }

    public override void SetCharacter(CharacterBase character)
    {
        _selectIndex = 0;
        _calledSelectAuto = false;
        _character = character;
        _automaticMode = character.IsAutomatic;
    }

    private void SelectAuto()
    {
        _selectWindows[_selectIndex].SetEnable(true);
        SetEnable(false);
    }

    private void ShowWindow()
    {
        _selectWindows[_selectIndex].SetCharacter(_character);
        _selectWindows[_selectIndex].SetEnable(true);
        SetEnable(false);
    }

    private void BackWindow()
    {
        _backToWindow.SetEnable(true);
        SetEnable(false);
    }
}
