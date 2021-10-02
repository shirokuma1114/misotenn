using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCard : MonoBehaviour
{
    private int _index;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //=================================
    //public
    //=================================
    public void OnClick()
    {
        transform.parent.gameObject.GetComponent<MoveCardManager>().SelectedCardIndex(_index);
    }

    public void SetIndex(int index)
    {
        _index = index;
    }

    //=================================
}
