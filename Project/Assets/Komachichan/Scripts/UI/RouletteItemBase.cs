using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface RouletteItemBase
{
    void Select(CharacterBase character);

    string GetDisplayName();

}
