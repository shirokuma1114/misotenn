using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareAllowance : SquareBase
{
    CharacterBase _character;

    MessageWindow _messageWindow;

    bool _isUsed = false;

    public override void Stop(CharacterBase character)
    {
        _character = character;

        // �C���X�^���X����
        var message = "�����В���\n�����Â����}�X�Ɂ@�~�܂����I";

        _messageWindow = FindObjectOfType<MessageWindow>();

        _messageWindow.SetMessage(message);

        _isUsed = true;
    }

    private void Update()
    {
        if (!_isUsed) return;

        // ���ʑ҂�
        if (!_messageWindow.IsDisplayed)
        {
            _character.CompleteStopExec();
            _isUsed = false;
        }
    }
}
