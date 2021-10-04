using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareAllowance : SquareBase
{
    CharacterBase _character;

    MessageWindow _messageWindow;

    bool _isUsed = false;

    bool _messageExists = false;

    public override void Stop(CharacterBase character)
    {
        _character = character;

        // �C���X�^���X����
        var message = "�����В���\n�����Â����}�X�Ɂ@�~�܂����I";

        _messageWindow = FindObjectOfType<MessageWindow>();

        _messageWindow.SetMessage(message);

        _isUsed = true;
        _messageExists = true;
    }

    private void Update()
    {
        if (!_isUsed) return;

        if(_messageExists && !_messageWindow.IsDisplayed)
        {
            _messageExists = false;
            // �����Â������[���b�g
        }

        // ���ʑ҂�
        if (!_messageWindow.IsDisplayed)
        {
            // �~�܂鏈���I��
            _character.CompleteStopExec();
            _isUsed = false;
        }
    }
}
