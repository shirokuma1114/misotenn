using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Souvenir
{
    //’l’i
    private int _price;

    public int Price { get { return _price; } }

    //–¼‘O
    private string _name;

    public string Name { get { return _name; } }

    private SouvenirType _type;

    public SouvenirType Type 
    {
        get { return _type; }
    }

    public Souvenir(int price, string name)
    {
        _price = price;
        _name = name;
    }
}
