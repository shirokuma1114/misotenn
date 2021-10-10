using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerBase : MonoBehaviour
{
    [SerializeField]
    protected CharacterBase _character;

    [SerializeField]
    private bool _isAutomatic;

    public bool IsAutomatic
    {
        get { return _isAutomatic; }
    }

    protected SquareBase[] _directionRoots;

    protected MoveCardManager _moveCardManager;

    protected MovingCountWindow _movingCount;

    protected StatusWindow _statusWindow;

    public CharacterBase Character
    {
        get { return _character; }
    }

    //移動カードを選び次のマスに止まるまで
    public virtual void Move()
    {

    }

    public virtual void SetRoot()
    {

    }

    protected void SetSquareRoots()
    {
        // ４方向の選択肢を生成

        //TODO 前進？後退？

        _directionRoots = new SquareBase[4];

        // 左0 上 1 右 2 下 3 の順に向きが適切なマスを入れる
        var squarePos = _character.CurrentSquare.GetPosition();


        foreach (var x in _character.GetInConnects())
        {
            if (x._directionType == ConnectDirection.LEFT)
            {
                _directionRoots[0] = x._square;
            }
            if (x._directionType == ConnectDirection.UP)
            {
                _directionRoots[1] = x._square;
            }
            if (x._directionType == ConnectDirection.RIGHT)
            {
                _directionRoots[2] = x._square;
            }
            if (x._directionType == ConnectDirection.DOWN)
            {
                _directionRoots[3] = x._square;
            }
        }

        foreach (var x in _character.GetOutConnects())
        {
            if (x._directionType == ConnectDirection.LEFT)
            {
                _directionRoots[0] = x._square;
            }
            if (x._directionType == ConnectDirection.UP)
            {
                _directionRoots[1] = x._square;
            }
            if (x._directionType == ConnectDirection.RIGHT)
            {
                _directionRoots[2] = x._square;
            }
            if (x._directionType == ConnectDirection.DOWN)
            {
                _directionRoots[3] = x._square;
            }
        }

        // 選択肢の表示
        /*
        Debug.Log(_directionRoots[0]);
        Debug.Log(_directionRoots[1]);
        Debug.Log(_directionRoots[2]);
        Debug.Log(_directionRoots[3]);
        */
    }

    protected void NotifyMovingCount(int count)
    {
        _movingCount.SetMovingCount(count);
    }

    protected void StartMove(SquareBase square)
    {
        _character.StartMove(square);
    }
}
