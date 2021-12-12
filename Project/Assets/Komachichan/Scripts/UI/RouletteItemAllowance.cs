using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteItemAllowance : RouletteItemBase
{
    private int _addValue;
    public int GetValue => _addValue;

    MessageWindow _messageWindow;

    StatusWindow _statusWindow;

    public RouletteItemAllowance(int addValue)
    {
        _addValue = addValue;
        _messageWindow = Object.FindObjectOfType<MessageWindow>();
        _statusWindow = Object.FindObjectOfType<StatusWindow>();
    }

    public string GetDisplayName()
    {
        return _addValue.ToString() + '‰~';
    }

    public void Select(CharacterBase character)
    {
        // ƒLƒƒƒ‰ƒNƒ^[‚É‚¨¬Œ­‚¢•t—^
        character.AddMoney(_addValue);

        var message = _addValue + "‰~Šl“¾I<>"+ character.Name + '‚Ì' +"@‚¿‹à‚Í\n" + character.Money + "‰~I";

        _messageWindow.SetMessage(message, character);
        _statusWindow.SetMoney(character.Money);
    }
}
