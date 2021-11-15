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
    }

    // Update is called once per frame
    void Update()
    {
        if (_isSelectedCard) return;
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
        _selectWindow.SetIsAutomatic(_character.IsAutomatic);
        _selectWindow.SetEnable(true);
    }

    public override void Move()
    {
        _moveCardManager.SetCardList(_character.MovingCards,true);
        _isSelectedCard = true;
        Invoke("SelectMovingCard", 2.5f);
    }

    void SelectMovingCard()
    {
        var index = CalcRoot();
        _moveCardManager.IndexSelect(index);
        _statusWindow.SetEnable(false);
        _movingCount.SetEnable(true);
        _souvenirWindow.SetEnable(false);
        _character.RemoveMovingCard(index);
        _goalMovingCount = _character.MovingCount;
        SetRoot();
        _moveCardManager.DeleteCards();
        _isSelectedCard = false;
        base.Move();
    }

    void DelayStartMove()
    {
        StartMove(_root.Dequeue());
    }

    protected override void SetRoot()
    {
        NotifyMovingCount(_character.MovingCount);

    }

    int CalcRoot()
    {
        return _aiLevel.CalcRoot(_character, ref _root);
    }

    void FindRoot(SquareBase square, int count, Queue<SquareBase> roots, List<SquareBase> squares, int index)
    {
        roots = new Queue<SquareBase>(roots);

        foreach(var x in roots)
        {
           //Debug.Log(count + x.ToString());
        }
        
        if (count <= 0)
        {
            AddRoot(square, roots, squares, index);
            return;
        }
        count--;
        roots.Enqueue(square);
        foreach (var x in square.OutConnects)
        {
            FindRoot(x, count, roots, squares, index);
        }
    }

    void AddRoot(SquareBase square, Queue<SquareBase> roots, List<SquareBase> squares, int index)
    {
        // Šù‚É‚ ‚é
        if (squares.Contains(square)) return;
        squares.Add(square);
        roots.Enqueue(square);
        _roots.Add(roots);
        _movingIndies.Add(index);
    }
}
