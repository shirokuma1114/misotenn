using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Souvenir
{
    //’l’i
    [SerializeField]
    private int _price;

    public int Price { get { return _price; } }

    //–¼‘O
    [SerializeField]
    private string _name;

    public string Name { get { return _name; } }

    [SerializeField]
    private SouvenirType _type;

    public SouvenirType Type 
    {
        get { return _type; }
    }

    [SerializeField]
    private Sprite _sprite;
    public Sprite Sprite { get { return _sprite; } }

    public Souvenir(int price, string name, SouvenirType type)
    {
        _price = price;
        _name = name;
        _type = type;
    }

    public Souvenir(Souvenir origin)
    {
        _name   = origin.Name;
        _price  = origin.Price;
        _type   = origin.Type;
        _sprite = origin.Sprite;
    }
}
