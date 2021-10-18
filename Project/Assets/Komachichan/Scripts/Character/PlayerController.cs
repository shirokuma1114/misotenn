using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PlayerController : CharacterControllerBase
{
    bool _isMoved = false;
    bool _isSelectedCard = false;

    Stack<SquareBase> _root = new Stack<SquareBase>();

    // Start is called before the first frame update
    void Awake()
    {
        _moveCardManager = FindObjectOfType<MoveCardManager>();
        _movingCount = FindObjectOfType<MovingCountWindow>();
        _statusWindow = FindObjectOfType<StatusWindow>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isSelectedCard) {
            UpdateSelect();
            return;
        }
        UpdateMove();
    }

    // 移動カードを選ぶ
    public override void Move()
    {
        _character.Init();
        _moveCardManager.SetCardList(_character.MovingCards);

        _statusWindow.SetEnable(true);
        _statusWindow.SetMoney(_character.Money);
        _statusWindow.SetName(_character.Name);
        _isSelectedCard = true;
    }

    public override void SetRoot()
    {
        NotifyMovingCount(_character.MovingCount);

        // ルート生成
        for(int i = 0; i < _character.MovingCount; i++)
        {
            var next = _character.CurrentSquare.OutConnects.First();
            _root.Push(next);
        }
    }

    private void UpdateSelect()
    {
        if (_moveCardManager.GetSelectedCardIndex() != -1)
        {
            _statusWindow.SetEnable(false);
            _movingCount.SetEnable(true);
            var index = _moveCardManager.GetSelectedCardIndex();
            _character.RemoveMovingCard(index);
            _isSelectedCard = false;
            _isMoved = true;
            SetRoot();
            _moveCardManager.DeleteCards();
            
        }
    }

    private void UpdateMove()
    {
        if (!_isMoved) return;
        if (_character.State != CharacterState.WAIT) return;

        // マス目を決定する
        if (_character.MovingCount == 0)
        {
            _movingCount.SetEnable(false);

            // 既に止まっているプレイヤーがいる
            if (_character.CurrentSquare.AlreadyStopped())
            {

            }
            _character.Stop();
            _isMoved = false;
            return;
        }

        StartMove(_root.Pop());
    }
}
