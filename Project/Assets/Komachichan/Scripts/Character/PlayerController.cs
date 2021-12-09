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
        _animation = GetComponent<CakeAnimation>();
        _isAutomatic = false;
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

    public override void InitTurn()
    {
        base.InitTurn();
        _character.Init();
        _statusWindow.SetEnable(true);
        _statusWindow.SetMoney(_character.Money);
        _statusWindow.SetName(_character.Name);
        _statusWindow.SetLapNum(_character.LapCount);
        _souvenirWindow.SetSouvenirs(_character.Souvenirs);
        _souvenirWindow.SetEnable(true);
        _selectWindow.SetCharacter(_character);
        _selectWindow.SetEnable(true);
    }

    // 移動カードを選ぶ
    public override void Move()
    {
        base.Move();
        _moveCardManager.SetCardList(_character.MovingCards);
        _isSelectedCard = true;
    }

    public override void SetRoot()
    {
        base.SetRoot();
        var index = _moveCardManager.GetSelectedCardIndex();
        _character.RemoveMovingCard(index);
        _goalMovingCount = _character.MovingCount;
        NotifyMovingCount(_character.MovingCount);
        _moveCardManager.DeleteCards();
        _isSelectedCard = false;
        // ルート生成
        DefaultGenerateRoot();
    }

    private void UpdateSelect()
    {
        if (_moveCardManager.GetSelectedCardIndex() != -1)
        {
            SetRoot();
            _statusWindow.SetEnable(false);
            _movingCount.SetEnable(true);
            _souvenirWindow.SetEnable(false);
        }
    }
}
