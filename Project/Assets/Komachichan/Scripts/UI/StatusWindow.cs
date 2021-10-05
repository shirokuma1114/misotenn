using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusWindow : MonoBehaviour
{
    [SerializeField]
    Image _frame;

    [SerializeField]
    Text _nameText;

    [SerializeField]
    Text _moneyText;
    // Start is called before the first frame update
    void Start()
    {
        SetEnable(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetEnable(bool enable)
    {
        _frame.enabled = enable;
        _nameText.enabled = enable;
        _moneyText.enabled = enable;
    }

    public void SetMoney(int value)
    {
        _moneyText.text = value + "‰~";
    }

}
