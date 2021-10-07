using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareEventGift : SquareEvent
{
   // [SerializeField]
    //private GameObject _choiceUIPrefab;
    //private ChoiceUI _choiceUI;


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
                PayWaitProcess();
                break;

            case EventState.EVENT:
                EventProcess();
                break;

            case EventState.END:
                EndProcess();
                break;
        }
    }


    //=================================
    //public
    //=================================
    public override void InvokeEvent(GameObject target)
    {
        //base.InvokeEvent(target);

        //_state = EventState.PAY_WAIT;

        //_choiceUI = Instantiate(_choiceUIPrefab).GetComponent<ChoiceUI>();
        //_choiceUI.SetDescription(_cost.ToString() + "Çéxï•Ç¡ÇƒÇ®ìyéYÇîÉÇ¢Ç‹Ç∑Ç©ÅH");
    }
    //=================================


    private void PayWaitProcess()
    {
        //if (_choiceUI.IsChoiseComplete())
        //{
        //    if (_choiceUI.IsSelectYes())
        //    {
        //        _state = EventState.EVENT;
        //    }
        //    else
        //    {
        //        _state = EventState.END;
        //    }

        //    Destroy(_choiceUI.gameObject);
        //}
    }

    private void EventProcess()
    {

    }

    private void EndProcess()
    {

    }
}
