using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PlayerController : CharacterControllerBase
{
    bool _isMoved = false;
    bool _isSelectedCard = false;

    Queue<SquareBase> _root = new Queue<SquareBase>();

    bool _collisionMode = false;


    // Start is called before the first frame update
    void Awake()
    {
        _moveCardManager = FindObjectOfType<MoveCardManager>();
        _movingCount = FindObjectOfType<MovingCountWindow>();
        _statusWindow = FindObjectOfType<StatusWindow>();
        _souvenirWindow = FindObjectOfType<SouvenirWindow>();
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
        _souvenirWindow.SetEnable(true);
        _souvenirWindow.SetSouvenirs(_character.Souvenirs);
        _isSelectedCard = true;
    }

    protected override void SetRoot()
    {
        // ルート生成
        var next = _character.CurrentSquare;
        for(int i = 0; i < _character.MovingCount; i++)
        {
            next = next.OutConnects.Last();
            _root.Enqueue(next);
        }
    }

    private void UpdateSelect()
    {
        if (_moveCardManager.GetSelectedCardIndex() != -1)
        {
            _statusWindow.SetEnable(false);
            _movingCount.SetEnable(true);
            _souvenirWindow.SetEnable(false);
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
        if (_character.MovingCount == 0 && !_collisionMode)
        {
            _movingCount.SetEnable(false);

            // 既に止まっているプレイヤーがいる
            if (_character.CurrentSquare.AlreadyStopped())
            {
                Collision(_character, _character.CurrentSquare.StoppedCharacters.ToList());
                _collisionMode = true;
                return;
            }
            _character.Stop();
            _isMoved = false;
            return;
        }

        if (_collisionMode)
        {
            UpdateColliision();
            if (IsFinishedCollision())
            {
                _collisionMode = false;
                _character.Stop();
                _isMoved = false;
                return;
            }
        }

        if (_root.Count == 0) return;
        NotifyMovingCount(_character.MovingCount);
        StartMove(_root.Dequeue());
    }
}
