using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteItemObject : MonoBehaviour
{
    private RouletteItemBase _rouletteItem;

    public RouletteItemBase RouletteItem
    {
        get { return _rouletteItem; }
        set { _rouletteItem = value; }
    }
}
