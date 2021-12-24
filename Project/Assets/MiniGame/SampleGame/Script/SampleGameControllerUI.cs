using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleGameControllerUI : MonoBehaviour
{
    private Text _playerName;
    private Text _rotateCounter;
    private Text _rendaKey;
    private Image _playerIcon;

    public void SetPlayerName(string playerName)
    {
        _playerName.text = playerName;
    }

    public void SetPlayerIcon(Sprite sprite)
    {
        _playerIcon.sprite = sprite;
    }

    public void SetRotateCounter(int count)
    {
        _rotateCounter.text = count.ToString();
    }

    public void SetRendaKeyEnable(bool enable,KeyCode keyCode)
    {
        _rendaKey.enabled = enable;
        _rendaKey.text = _rendaKey.text + keyCode.ToString().Replace("Alpha","");
    }

    //================

    void Awake()
    {
        _playerName = transform.Find("PlayerName").GetComponent<Text>();
        _rotateCounter = transform.Find("RotateCounter").GetComponent<Text>();
        _rendaKey = transform.Find("RendaKey").GetComponent<Text>();
        _playerIcon = transform.Find("PlayerImage").GetComponent<Image>();
    }

    void Update()
    {
        
    }
}
