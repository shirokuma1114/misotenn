using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovingCountWindow : MonoBehaviour
{
    [SerializeField]
    Text _text;

    [SerializeField]
    Image _frame;

    public void Start()
    {
        SetEnable(false);
    }

    public void SetMovingCount(int count)
    {
        _text.text = "Ç†Ç∆" + count + "É}ÉX";
    }

    public void SetEnable(bool enable)
    {
        _text.enabled = enable;
        _frame.enabled = enable;
    }
}
