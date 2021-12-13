using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AIController : CharacterControllerBase
{
    AILevelBase _aiLevel;

    List<Queue<SquareBase>> _roots = new List<Queue<SquareBase>>();

    List<int> _movingIndies = new List<int>();

    bool _isSelectedCard = false;

    void Awake()
    {
        _moveCardManager = FindObjectOfType<MoveCardManager>();
        _movingCount = FindObjectOfType<MovingCountWindow>();
        _statusWindow = FindObjectOfType<StatusWindow>();
        _souvenirWindow = FindObjectOfType<SouvenirWindow>();
        _eventState = EventState.WAIT;
        _aiLevel = new AILevelHard();
        _animation = GetComponent<CakeAnimation>();
        _isAutomatic = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isSelectedCard)
        {
            UpdateSelect();
            return;
        }
        UpdateMove();
    }

    public void SetAILevel(AILevelBase aiLevel)
    {
        _aiLevel = aiLevel;
    }

    public override void InitTurn()
    {
        base.InitTurn();
        _character.Init();
        _root.Clear();
        _roots.Clear();
        _movingIndies.Clear();
        _statusWindow.SetEnable(true);
        _statusWindow.SetMoney(_character.Money);
        _statusWindow.SetName(_character.Name);
        _statusWindow.SetLapNum(_character.LapCount);
        _souvenirWindow.SetSouvenirs(_character.Souvenirs);
        _souvenirWindow.SetEnable(true);
        _selectWindow.SetCharacter(_character);
        _selectWindow.SetEnable(true);
    }

    public override void Move()
    {
        base.Move();
        _moveCardManager.SetCardList(_character.MovingCards, _character);
        _isSelectedCard = true;
        Invoke("SelectMovingCard", 2.0f);
    }

    void SelectMovingCard()
    {
        var index = _aiLevel.CalcRoot(_character, ref _root);
        _moveCardManager.IndexSelect(index);
        _isSelectedCard = true;
    }

    void DelayStartMove()
    {
        StartMove(_root.Dequeue());
    }

    public override void SetRoot()
    {
        // 思考＆ルート作成
        base.SetRoot();
        var index = _moveCardManager.GetSelectedCardIndex();
        _character.RemoveMovingCard(index);
        _goalMovingCount = _character.MovingCount;
        NotifyMovingCount(_character.MovingCount);
        _moveCardManager.DeleteCards();
        _isSelectedCard = false;
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
