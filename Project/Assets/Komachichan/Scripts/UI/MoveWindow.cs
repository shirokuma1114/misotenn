using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWindow : WindowBase
{
    [SerializeField]
    MyGameManager _myGameManager;

    [SerializeField]
    CameraInterpolation _camera;
    public override void SetEnable(bool enable)
    {
        _camera.Set_Second(1f);
        _camera.Set_NextCamera(1);
        _myGameManager.Move();
    }
}
