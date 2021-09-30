using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBase : MonoBehaviour
{
    [SerializeField]
    CharacterControllerBase _controller;

    // ������
    private int _money;

    public int Money
    { get { return _money; } }


    // �ړ��J�[�h���X�g
    private List<int> _movingCards = new List<int>();
    public List<int> MovingCards
    { get { return _movingCards; } }

    // ���y�Y�J�[�h���X�g
    private List<Souvenir> _souvenirs = new List<Souvenir>();

    public List<Souvenir> Souvenirs
    { get { return _souvenirs; } }

    // ���ݎ~�܂��Ă���}�X
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

    // ���[�g�ۑ��p
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

    // �X�^�[�g�̃}�X��ݒ�
    public void SetCurrentSquare(SquareBase square)
    {
        _currentSquare = square;
    }

    public void StartMove(SquareBase square)
    {
        _isMoved = true;

        
        // ���
        if (_rootStack.Contains(square))
        {
            _rootStack.Pop();
            _movingCount++;
        }
        // �O�i
        else
        {
            _rootStack.Push(_currentSquare);
            _movingCount--;
        }
        _currentSquare = square;

        // �X�e�[�W��]
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

        // �X�^�b�N�ɂ���}�X�̂�
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
