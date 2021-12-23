using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SouvenirScrambleControllerUI : MonoBehaviour
{
    private Text _playerName;
    private Text _itemCounter;
    private Image _icon;


    public void Init(string playerName,Sprite sprite)
    {
        _playerName.text = playerName;
        _itemCounter.text = "0";
        _icon.sprite = sprite;
    }

    public void SetItemCounter(int count)
    {
        _itemCounter.text = count.ToString();
    }

    //---------------------

    void Start()
    {
        _playerName = transform.Find("PlayerName").GetComponent<Text>();
        _itemCounter = transform.Find("ItemCounter").GetComponent<Text>();
        _icon = transform.Find("Icon").GetComponent<Image>();
    }

    void Update()
    {
        
    }
}
