using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SouvenirWindow : MonoBehaviour
{
    [SerializeField]
    Image _frame;

    [SerializeField]
    Text _text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetEnable(bool enable)
    {
        _frame.enabled = enable;
        _text.enabled = enable;
    }

}
