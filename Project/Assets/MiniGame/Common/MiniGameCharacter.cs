using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MiniGameCharacter
{
    private CharacterBase _ridingCharacter;

    public virtual bool IsAutomatic => _ridingCharacter.IsAutomatic;
    public virtual string Name => _ridingCharacter.Name;
    public virtual int Money => _ridingCharacter.Money;

    public MiniGameCharacter(CharacterBase controller)
    {
        _ridingCharacter = controller;
    }

    public virtual void AddMoney(int money)
    {
        _ridingCharacter.AddMoney(money);
    }
}


[System.Serializable]
public class DebugMiniGameCharacter : MiniGameCharacter
{
    [SerializeField]
    private bool _automatic;
    [SerializeField]
    private string _name;
    [SerializeField]
    private int _money;

    public override bool IsAutomatic => _automatic;
    public override string Name => _name;
    public override int Money => _money;

    public DebugMiniGameCharacter() : base(null)
    {

    }
    public DebugMiniGameCharacter(DebugMiniGameCharacter debugCharacter) : base(null)
    {
        _automatic = debugCharacter._automatic;
        _name = debugCharacter._name;
        _money = debugCharacter._money;
    }

    public override void AddMoney(int money)
    {
        _money += money;
    }
}