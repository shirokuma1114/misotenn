using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBase : MonoBehaviour
{
    [SerializeField]
    CharacterControllerBase _controller;

    // 所持金
    private int _money;

    public int Money
    { get { return _money; } }


    // 移動カードリスト
    private List<int> _movingCards = new List<int>();
    public List<int> MovingCards
    { get { return _movingCards; } }

    // お土産カードリスト
    private List<Souvenir> _souvenirs = new List<Souvenir>();

    public List<Souvenir> Souvenirs
    { get { return _souvenirs; } }

    // 現在止まっているマス
    protected SquareBase _currentSquare;

    public SquareBase CurrentSquare
    {
        get { return _currentSquare; }
    }


    private bool _isMoved = false;

    public bool IsMoved
    {
        get { return _isMoved; }
    }

    // ルート保存用
    private Stack<SquareBase> _rootStack = new Stack<SquareBase>();

    private int _movingCount;

    public int MovingCount
    {
        get { return _movingCount; }
    }

    void Start()
    {
        
    }
    void Update()
    {
        UpdateMove();
    }

    public void AddMovingCard(int movingValue)
    {
        _movingCards.Add(movingValue);
    }

    public void RemoveMovingCard(int cardIndex)
    {
        _movingCount = _movingCards[cardIndex];
        _movingCards.RemoveAt(cardIndex);
    }
    
    public void AddSouvenir(Souvenir souvenir)
    {
        _souvenirs.Add(souvenir);
    }

    public void RemoveSouvenir(int index)
    {
        _souvenirs.RemoveAt(index);
    }

    // スタートのマスを設定
    public void SetCurrentSquare(SquareBase square)
    {
        _currentSquare = square;
    }

    public void StartMove(SquareBase square)
    {
        _isMoved = true;

        
        // 後退
        if (_rootStack.Contains(square))
        {
            _rootStack.Pop();
            _movingCount++;
        }
        // 前進
        else
        {
            _rootStack.Push(_currentSquare);
            _movingCount--;
        }
        _currentSquare = square;

        // ステージ回転
        FindObjectOfType<EarthMove>().MoveToPosition(_currentSquare.GetPosition());
    }

    private void UpdateMove()
    {
        if (!_isMoved) return;
        if (FindObjectOfType<EarthMove>().State == EarthMove.EarthMoveState.END)
        {
            _controller.SetRoot();
            _isMoved = false;
        }
    }

    public List<SquareConnect> GetInConnects()
    {
        var outs = new List<SquareConnect>();

        // スタックにあるマスのみ
        foreach (var s in _currentSquare.InConnects)
        {
            if (_rootStack.Contains(s._square))
            {
                outs.Add(s);
            }
        }
        
        return outs;
    }

    public List<SquareConnect> GetOutConnects()
    {
        return _movingCount > 0 ? _currentSquare.OutConnects : new List<SquareConnect>();
    }


}
