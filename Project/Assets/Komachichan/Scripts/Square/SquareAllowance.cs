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

        // インスタンス生成
        var message = "小町社長は\nおこづかいマスに　止まった！";

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
            // おこづかいルーレット
        }

        // 結果待ち
        if (!_messageWindow.IsDisplayed)
        {
            // 止まる処理終了
            _character.CompleteStopExec();
            _isUsed = false;
        }
    }
}
