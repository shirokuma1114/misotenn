using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveCardFrame : MonoBehaviour
{
    private Image _sprite;

    [SerializeField]
    private Color SELECTED_COLOR;
    [SerializeField]
    private Color NOT_SELECTED_COLOR;


    public void SelectedColorUpdate(bool isSlected)
    {
        if(isSlected)
        {
            _sprite.color = SELECTED_COLOR;
        }
        else
        {
            _sprite.color = NOT_SELECTED_COLOR;
        }
    }

    //===================

    private void Awake()
    {
        _sprite = GetComponent<Image>();
    }

    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
