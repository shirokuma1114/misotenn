using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AIController : CharacterControllerBase
{
    bool _isMoved = false;

    List<Stack<SquareBase>> _roots = new List<Stack<SquareBase>>();

    Stack<SquareBase> _root = new Stack<SquareBase>();

    List<int> _movingIndies = new List<int>();

    void Awake()
    {
        _moveCardManager = FindObjectOfType<MoveCardManager>();
        _movingCount = FindObjectOfType<MovingCountWindow>();
        _statusWindow = FindObjectOfType<StatusWindow>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMove();
    }

    public override void Move()
    {
        _character.Init();

        _root.Clear();
        _roots.Clear();
        _movingIndies.Clear();

        _moveCardManager.SetCardList(_character.MovingCards);
        
        _statusWindow.SetEnable(true);
        _statusWindow.SetMoney(_character.Money);
        _statusWindow.SetName(_character.Name);

        Invoke("SelectMovingCard", 1.5f);
    }

    void SelectMovingCard()
    {
        var index = CalcRoot();
        _moveCardManager.IndexSelect(index);
        _statusWindow.SetEnable(false);
        _movingCount.SetEnable(true);
        _character.RemoveMovingCard(index);
        SetRoot();
        _moveCardManager.DeleteCards();
        _isMoved = true;
    }

    void UpdateMove()
    {
        if (!_isMoved) return;
        if (_character.State != CharacterState.WAIT) return;

        // マス目を決定する
        if (_character.MovingCount == 0)
        {
            _movingCount.SetEnable(false);
            _character.Stop();
            _isMoved = false;
            return;
        }

        StartMove(_root.Pop());
    }

    void DelayStartMove()
    {
        StartMove(_root.Pop());
    }

    public override void SetRoot()
    {
        NotifyMovingCount(_character.MovingCount);

    }

    int CalcRoot()
    {
        //移動できるマスとルートを取得
        var square = _character.CurrentSquare;

        Stack<SquareBase> roots = new Stack<SquareBase>();

        List<SquareBase> squares = new List<SquareBase>();

        for(int i = 0; i < _character.MovingCards.Count; i++)
        {
            FindRoot(square, _character.MovingCards[i], roots, squares, i);
        }

        // 最も良いマスの選択
        int maxScore = -1;
        int index = -1;
        for(int i = 0; i < squares.Count; i++)
        {
            var score = squares[i].GetScore(_character);
            if(maxScore < score)
            {
                maxScore = score;
                index = i;
            }
        }

        if (index < 0) return -1;

        //Debug.Log("えらばれたのは" + squares[index]);

        for(int i = 0; i < _roots.Count; i++)
        {
            foreach(var x in _roots[i])
            {
                //Debug.Log(i + x.ToString());
            }
        }

        _root = _roots[index];
        _root = new Stack<SquareBase>(_root);

        foreach(var x in _root)
        {
            //Debug.Log(x);
        }


        // 最初は自分のいるマス
        _root.Pop();

        return _movingIndies[index];
    }

 
    void FindRoot(SquareBase square, int count, Stack<SquareBase> roots, List<SquareBase> squares, int index)
    {
        roots = new Stack<SquareBase>(roots.Reverse());

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
        roots.Push(square);
        foreach (var x in square.OutConnects)
        {
            FindRoot(x._square, count, roots, squares, index);
        }
    }

    void AddRoot(SquareBase square, Stack<SquareBase> roots, List<SquareBase> squares, int index)
    {
        // 既にある
        if (squares.Contains(square)) return;
        squares.Add(square);
        roots.Push(square);
        _roots.Add(roots);
        _movingIndies.Add(index);
    }
}
