using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleGameControllerUI : MonoBehaviour
{
    private Text _playerName;
    private Text _rotateCounter;
    private Text _rendaKey;

    public void SetPlayerName(string playerName)
    {
        _playerName.text = playerName;
    }

    public void SetRotateCounter(int count)
    {
        _rotateCounter.text = count.ToString();
    }

    public void SetRendaKeyEnable(bool enable)
    {
        _rendaKey.enabled = enable;
    }

    //================

    void Awake()
    {
        _playerName = transform.Find("PlayerName").GetComponent<Text>();
        _rotateCounter = transform.Find("RotateCounter").GetComponent<Text>();
        _rendaKey = transform.Find("RendaKey").GetComponent<Text>();
    }

    void Update()
    {
        
    }
}
