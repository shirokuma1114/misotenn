using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleGameControllerUI : MonoBehaviour
{
    private Text _playerName;
    private Text _rotateCounter;

    public void SetPlayerName(string playerName)
    {
        _playerName.text = playerName;
    }

    public void SetRotateCounter(int count)
    {
        _rotateCounter.text = count.ToString();
    }

    //================

    void Start()
    {
        _playerName = transform.Find("PlayerName").GetComponent<Text>();
        _rotateCounter = transform.Find("RotateCounter").GetComponent<Text>();
    }

    void Update()
    {
        
    }
}
