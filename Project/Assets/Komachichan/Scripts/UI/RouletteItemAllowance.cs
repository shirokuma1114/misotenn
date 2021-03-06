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
        return _addValue.ToString() + '円';
    }

    public void Select(CharacterBase character)
    {
        // キャラクターにお小遣い付与
        character.AddMoney(_addValue);

        var message = _addValue + "円獲得！<>"+ character.Name + 'の' +"　持ち金は\n" + character.Money + "円！";

        _messageWindow.SetMessage(message, character.IsAutomatic);
        _statusWindow.SetMoney(character.Money);
    }
}
