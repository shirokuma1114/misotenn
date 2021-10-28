using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PlayerController : CharacterControllerBase
{
    bool _isSelectedCard = false;

    // Start is called before the first frame update
    void Awake()
    {
        _moveCardManager = FindObjectOfType<MoveCardManager>();
        _movingCount = FindObjectOfType<MovingCountWindow>();
        _statusWindow = FindObjectOfType<StatusWindow>();
        _souvenirWindow = FindObjectOfType<SouvenirWindow>();
        _eventState = EventState.WAIT;
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
        base.Move();
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
            SetRoot();
            _moveCardManager.DeleteCards();
            
        }
    }
}
