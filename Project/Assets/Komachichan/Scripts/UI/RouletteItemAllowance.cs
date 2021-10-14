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
        return _addValue.ToString() + '�~';
    }

    public void Select(CharacterBase character)
    {
        // �L�����N�^�[�ɂ��������t�^
        character.AddMoney(_addValue);

        var message = _addValue + "�~�l���I<>"+ character.Name + '��' +"�@��������\n" + character.Money + "�~�I";

        _messageWindow.SetMessage(message, character.IsAutomatic);
        _statusWindow.SetMoney(character.Money);
    }
}
