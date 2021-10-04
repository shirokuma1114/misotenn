using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareEventPlus : SquareEvent
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case EventState.PAY_WAIT:
                break;

            case EventState.EVENT:
                break;

            case EventState.END:
                break;
        }
    }


    //=================================
    //public
    //=================================
    public override void InvokeEvent(GameObject target)
    {
        base.InvokeEvent(target);


        _state = EventState.EVENT;
    }
}
