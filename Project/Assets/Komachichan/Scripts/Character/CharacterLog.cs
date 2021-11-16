using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLog : MonoBehaviour
{
    private int[] _useEventNumByType;

    private List<int> _addMoneyByTurn;


    public CharacterLog()
    {
        _useEventNumByType = new int[(int)SquareEventType.EVENT_TYPE_MAX];
    }

    public void AddUseEventNum(SquareEventType eventType)
    {
        _useEventNumByType[(int)eventType]++;
    }

    public int[] GetUseEventNum()
    {
        return _useEventNumByType;
    }

    public void SetAddMoenyByTurn()
    {

    }
}
