using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KomachiInput
{
    private int _controllerId;

    public KomachiInput(int controllerId)
    {
        _controllerId = controllerId;
    }

    public bool GetButton(string buttonName)
    {
        return Input.GetButton(buttonName + _controllerId);
    }
    public bool GetButtonDown(string buttonName)
    {
        return Input.GetButtonDown(buttonName + _controllerId);
    }
    public bool GetButtonUp(string buttonName)
    {
        return Input.GetButtonUp(buttonName + _controllerId);
    }
    public float GetAxis(string axisName)
    {
        return Input.GetAxis(axisName + _controllerId);
    }
    public float GetAxisRaw(string axisName)
    {
        return Input.GetAxisRaw(axisName + _controllerId);
    }
}
