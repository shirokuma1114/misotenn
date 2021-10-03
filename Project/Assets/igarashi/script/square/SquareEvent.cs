using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�C���^�[�t�F�[�X
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

    [Header("����")]
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
