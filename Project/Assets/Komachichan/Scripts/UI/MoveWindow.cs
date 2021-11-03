using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWindow : WindowBase
{
    [SerializeField]
    MyGameManager _myGameManager;
    public override void SetEnable(bool enable)
    {
        _myGameManager.Move();
    }
}
