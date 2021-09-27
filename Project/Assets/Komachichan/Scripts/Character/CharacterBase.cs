using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    // ������
    private int _money;

    public int Money
    { get { return _money; } }


    // �ړ��J�[�h���X�g
    private List<int> _movingCards;
    public List<int> MovingCards
    { get { return _movingCards; } }

    // ���y�Y�J�[�h���X�g
    private List<Souvenir> _souvenirs;

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
    private Stack<SquareBase> _rootStack;

    private int _movingCount;

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
    }

    private void UpdateMove()
    {
        if (!_isMoved) return;

    }

    public List<SquareBase> GetInConnects()
    {
        return _currentSquare.InConnects;
    }

    public List<SquareBase> GetOutConnects()
    {
        var outs = new List<SquareBase>();

        // �X�^�b�N�ɂ���}�X�̂�
        foreach(var s in _currentSquare.OutConnects)
        {
            if (_rootStack.Contains(s))
            {
                outs.Add(s);
            }
        }
        
        return outs;
    }
}
