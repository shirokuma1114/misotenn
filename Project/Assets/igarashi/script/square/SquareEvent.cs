using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//インターフェース
public class SquareEvent : MonoBehaviour
{
    public enum EventState
    {
        IDLE,
        PAY_WAIT,
        EVENT,
        END,
    }
    protected EventState _state = EventState.IDLE;

    protected GameObject _targetPlayer;

    [Header("共通")]
    [SerializeField]
    protected int _cost;


    //=================================
    //public
    //=================================
    public virtual void InvokeEvent(GameObject target)
    {
        _targetPlayer = target;
    }
    //=================================
}
