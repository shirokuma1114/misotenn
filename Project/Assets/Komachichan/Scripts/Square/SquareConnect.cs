using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SquareConnect
{
    [SerializeField]
    public SquareBase _square;

    [SerializeField]
    public ConnectDirection _directionType;
}
