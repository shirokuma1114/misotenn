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

        // インスタンス生成
        var message = "小町社長は\nおこづかいマスに　止まった！";

        _messageWindow = FindObjectOfType<MessageWindow>();

        _messageWindow.SetMessage(message);

        _isUsed = true;
    }

    private void Update()
    {
        if (!_isUsed) return;

        // 結果待ち
        if (!_messageWindow.IsDisplayed)
        {
            _character.CompleteStopExec();
            _isUsed = false;
        }
    }
}
