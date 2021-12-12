using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfomationWindow : WindowBase
{
    [SerializeField]
    List<Image> _frameImages;

    [SerializeField]
    List<Text> _characterNameTexts;

    [SerializeField]
    List<Text> _characterMoneyTexts;

    [SerializeField]
    List<Text> _characterMoneyAmountTexts;
    
    
    [SerializeField]
    List<MiniSouvenirWindow> _souvenirWindows;

    [SerializeField]
    WindowBase _backToWindow;

    [SerializeField]
    SouvenirWindow _souvenirWindow;

    [SerializeField]
    StatusWindow _statusWindow;

    [SerializeField]
    MyGameManager _gameManager;

    bool _enable;

    private CharacterBase _character;
    // Start is called before the first frame update
    void Start()
    {
        ShowWindow(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (!_enable) return;

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return)
            || _character.Input.GetButtonDown("A") || _character.Input.GetButtonDown("B"))
        {
            Invoke("BackToWindow", 0.01f);

            if(Control_SE.Get_Instance())Control_SE.Get_Instance().Play_SE("UI_Close");
        }
    }

    void BackToWindow()
    {
        SetEnable(false);
        _backToWindow.SetEnable(true);
    }

    void ShowWindow(bool enable)
    {
        _enable = enable;

        foreach (var x in _frameImages)
        {
            x.enabled = enable;
        }

        foreach(var x in _characterNameTexts)
        {
            x.enabled = enable;
        }
        foreach(var x in _characterMoneyTexts)
        {
            x.enabled = enable;
        }
        foreach(var x in _characterMoneyAmountTexts)
        {
            x.enabled = enable;
        }
        
    }

    public override void SetEnable(bool enable)
    {
        ShowWindow(enable);
        
        _souvenirWindow.SetEnable(!enable);
        _statusWindow.SetEnable(!enable);

        if (enable)
        {
            UpdateInfomation();
        }

        foreach (var x in _souvenirWindows)
        {
            x.SetEnable(enable);
        }

    }

    public override void SetCharacter(CharacterBase character)
    {
        _character = character;
    }

    void UpdateInfomation()
    {
        var ranksortedCharacters = _gameManager.GetRankSortedCharacters();

        for(int i = 0; i < 4; i++)
        {
            _characterNameTexts[i].text = ranksortedCharacters[i].Name;
            _characterMoneyAmountTexts[i].text = ranksortedCharacters[i].Money.ToString() + '‰~';
            _souvenirWindows[i].SetSouvenirs(ranksortedCharacters[i].Souvenirs);
        }
    }
    
}
