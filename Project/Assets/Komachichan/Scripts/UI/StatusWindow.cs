using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusWindow : WindowBase
{
    [SerializeField]
    Image _frame;

    [SerializeField]
    Text _nameText;

    [SerializeField]
    Text _moneyText;

    [SerializeField]
    Text _turnText;

    [SerializeField]
    Text _lapText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void SetEnable(bool enable)
    {
        _frame.enabled = enable;
        _nameText.enabled = enable;
        _moneyText.enabled = enable;
        _turnText.enabled = enable;
        _lapText.enabled = enable;
    }

    public void SetMoney(int value)
    {
        _moneyText.text = value + "â~";
    }

    public void SetName(string name)
    {
        _nameText.text = name;
    }

    public void SetTurn(int turnCount)
    {
        _turnText.text = turnCount + "É^Å[Éìñ⁄";
    }

    public void SetLapNum(int lapNum)
    {
        lapNum++;
        _lapText.text = "åªç› " + lapNum + "é¸ñ⁄";
    }

}
