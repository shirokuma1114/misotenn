using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui : MonoBehaviour
{
    private Text _playerName;
    private Text _Counter;

    public void SetPlayerName(string playerName)
    {
        _playerName.text = playerName;
    }

    public void SetRotateCounter(int count)
    {
        _Counter.text = count.ToString();
    }

    //================

    void Awake()
    {
        _playerName = transform.Find("PlayerName").GetComponent<Text>();
        _Counter = transform.Find("Counter").GetComponent<Text>();
    }

    void Update()
    {

    }
}
