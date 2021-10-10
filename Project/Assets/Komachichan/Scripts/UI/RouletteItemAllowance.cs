using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteItemAllowance : RouletteItemBase
{
    private int _addValue;

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
        return _addValue.ToString() + 'â~';
    }

    public void Select(CharacterBase character)
    {
        // ÉLÉÉÉâÉNÉ^Å[Ç…Ç®è¨å≠Ç¢ïtó^
        character.AddMoney(_addValue);

        var message = _addValue + "â~älìæÅI<>"+ character.Name + 'ÇÃ' +"Å@éùÇøã‡ÇÕ\n" + character.Money + "â~ÅI";

        _messageWindow.SetMessage(message, character.IsAutomatic);
        _statusWindow.SetMoney(character.Money);
    }
}
