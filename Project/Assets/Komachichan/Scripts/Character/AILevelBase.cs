using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AILevelBase
{
    protected List<Queue<SquareBase>> _roots;

    protected List<int> _movingIndies;
    protected abstract int JudgeSquare(CharacterBase character, List<SquareBase> squares);

    public int CalcRoot(CharacterBase character, ref Queue<SquareBase> root)
    {
        _movingIndies = new List<int>();
        _roots = new List<Queue<SquareBase>>();

        //移動できるマスとルートを取得
        var square = character.CurrentSquare;
        List<SquareBase> squares = new List<SquareBase>();

        for (int i = 0; i < character.MovingCards.Count; i++)
        {
            FindRoot(square, character.MovingCards[i], root, squares, i);
        }

        var index = JudgeSquare(character, squares);
        if (index < 0) return -1;

        //Debug.Log("えらばれたのは" + squares[index]);

        for (int i = 0; i < root.Count; i++)
        {
            foreach (var x in _roots[i])
            {
                //Debug.Log(i + x.ToString());
            }
        }

        root = _roots[index];

        // 最初は自分のいるマス
        root.Dequeue();
        return _movingIndies[index];
    }

    protected void FindRoot(SquareBase square, int count, Queue<SquareBase> roots, List<SquareBase> squares, int index)
    {
        roots = new Queue<SquareBase>(roots);

        foreach (var x in roots)
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

    protected void AddRoot(SquareBase square, Queue<SquareBase> roots, List<SquareBase> squares, int index)
    {
        // 既にある
        if (squares.Contains(square)) return;
        squares.Add(square);
        roots.Enqueue(square);
        _roots.Add(roots);
        _movingIndies.Add(index);
    }

}
