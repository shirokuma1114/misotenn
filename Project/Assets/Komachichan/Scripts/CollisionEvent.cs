using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEvent
{
    enum Phase
    {
        NONE,
        INIT,
        SELECT,
        END,
    }

    Phase _phase;

    MessageWindow _messageWindow;

    bool _isFinished = false;

    int _targetIndex;

    CharacterBase _owner;

    List<CharacterBase> _targets;

    int _souvenirIndex;

    public CollisionEvent(CharacterBase owner, List<CharacterBase> targets)
    {
        _messageWindow = Object.FindObjectOfType<MessageWindow>();
        _messageWindow.SetMessage("衝突！", owner.IsAutomatic);
        _phase = Phase.INIT;
        _targetIndex = 0;
        _owner = owner;
        _targets = targets;
        _souvenirIndex = 0;
    }

    public void Update()
    {
        if (_phase == Phase.NONE) return;
        if (_phase == Phase.INIT)
        {
            if (!_messageWindow.IsDisplayed)
            {
                // お土産を持ってない
                if (_targets[_targetIndex].Souvenirs.Count == 0)
                {
                    _messageWindow.SetMessage(_targets[_targetIndex].Name + "は　お土産を　持っていなかった！", _owner.IsAutomatic);
                    _phase = Phase.END;
                }
                else
                {
                    _phase = Phase.SELECT;
                }
            }
        }

        if(_phase == Phase.SELECT)
        {
            if (true /* 選択 or ランダムでお土産が選択された */)
            {
                _souvenirIndex = Random.Range(0, _targets.Count);

                var souvenir = _targets[_targetIndex].Souvenirs[_souvenirIndex];
                _messageWindow.SetMessage(_targets[_targetIndex].Name + "　のお土産カード　" + souvenir.Name + "\nを　いただいた！", _owner.IsAutomatic);

                _owner.AddSouvenir(souvenir);
                _targets[_targetIndex].RemoveSouvenir(_souvenirIndex);
                _phase = Phase.END;
            }
        }
        

        if(_phase == Phase.END)
        {
            if (!_messageWindow.IsDisplayed)
            {
                _phase = Phase.SELECT;
                _targetIndex++;
                // 終了
                if(_targetIndex >= _targets.Count)
                {
                    _phase = Phase.NONE;
                    _isFinished = true;
                }
            }
        }
    }

    public bool IsFinished()
    {
        return _isFinished;
    }
}
