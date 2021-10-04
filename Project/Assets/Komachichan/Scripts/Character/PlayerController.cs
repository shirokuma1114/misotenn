using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : CharacterControllerBase
{


    bool _isMoved = false;
    bool _isSelectedCard = false;

    SquareBase[] _directionRoots;

    MoveCardManager _moveCardManager;
    
    // Start is called before the first frame update
    void Awake()
    {
        _moveCardManager = FindObjectOfType<MoveCardManager>();
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
        _moveCardManager.SetCardList(_character.MovingCards);

        _isSelectedCard = true;
    }

    public override void SetRoot()
    {
        NotifyMovingCount(_character.MovingCount);

        // ルート生成
        SetSquareRoots();
        CreateArrow();
    }

    private void CreateArrow()
    {
        FindObjectOfType<UIArrow>().Create(_directionRoots);
    }

    private void DeleteArrow()
    {
        FindObjectOfType<UIArrow>().Delete();
    }

    public void SetSquareRoots()
    {
        // ４方向の選択肢を生成

        //TODO 前進？後退？

        _directionRoots = new SquareBase[4];

        // 左0 上 1 右 2 下 3 の順に向きが適切なマスを入れる
        var squarePos = _character.CurrentSquare.GetPosition();
        

        foreach(var x in _character.GetInConnects())
        {
            if(x._directionType == ConnectDirection.LEFT)
            {
                _directionRoots[0] = x._square;
            }
            if(x._directionType == ConnectDirection.UP)
            {
                _directionRoots[1] = x._square;
            }
            if(x._directionType == ConnectDirection.RIGHT)
            {
                _directionRoots[2] = x._square;
            }
            if(x._directionType == ConnectDirection.DOWN)
            {
                _directionRoots[3] = x._square;
            }
        }
        
        foreach(var x in _character.GetOutConnects())
        {
            if(x._directionType == ConnectDirection.LEFT)
            {
                _directionRoots[0] = x._square;
            }
            if(x._directionType == ConnectDirection.UP)
            {
                _directionRoots[1] = x._square;
            }
            if(x._directionType == ConnectDirection.RIGHT)
            {
                _directionRoots[2] = x._square;
            }
            if(x._directionType == ConnectDirection.DOWN)
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

    private void UpdateSelect()
    {
        if (_moveCardManager.GetSelectedCardIndex() != -1)
        {
            var index = _moveCardManager.GetSelectedCardIndex();
            _character.RemoveMovingCard(index);
            _isSelectedCard = false;
            _isMoved = true;
            SetRoot();
            _moveCardManager.DeleteCards();
            
        }
    }

    private void UpdateMove()
    {
        if (!_isMoved) return;

        if (_character.IsMoved) return;

        var inputDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        
        if (inputDir == Vector2.zero) return;

        if(Mathf.Abs(inputDir.x) > Mathf.Abs(inputDir.y))
        {
            if (inputDir.x < 0)
            {
                if (_directionRoots[0])
                    StartMove(_directionRoots[0]);
            }
            else
            {
                if (_directionRoots[2])
                    StartMove(_directionRoots[2]);
            }
        }
        else
        {
            if (inputDir.y < 0)
            {
                if (_directionRoots[1])
                    StartMove(_directionRoots[1]);
            }
            else
            {
                if (_directionRoots[3])
                    StartMove(_directionRoots[3]);
            }
        }

        // マス目を決定する
        if(_character.MovingCount == 0)
        {

        }

    }

    void NotifyMovingCount(int count)
    {
        GameObject.Find("Text").GetComponent<Text>().text = count.ToString();
    }

    void StartMove(SquareBase square)
    {
        _character.StartMove(square);
        
    }
}
