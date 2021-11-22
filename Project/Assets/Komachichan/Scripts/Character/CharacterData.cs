using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData
{
    public CharacterBase _character;
    public string _characterName;
    public int _rank;
    public int _lapCount;
    public int _souvenirNum;
    public int _money;
    public int[] _useEventNumByType;
    public List<int> _moneyByTurn;
}