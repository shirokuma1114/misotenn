using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SouvenirScrambleControllerUI : MonoBehaviour
{
    private Text _playerName;
    private Text _itemCounter;


    public void Init(string playerName)
    {
        _playerName.text = playerName;
        _itemCounter.text = "0";
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
    }

    void Update()
    {
        
    }
}
