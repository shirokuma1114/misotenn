using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : CharacterControllerBase
{
    bool _isMoved = false;
    bool _isSelectedCard = false;

    // Start is called before the first frame update
    void Awake()
    {
        _moveCardManager = FindObjectOfType<MoveCardManager>();
        _movingCount = FindObjectOfType<MovingCountWindow>();
        _statusWindow = FindObjectOfType<StatusWindow>();
        _arrowUI = FindObjectOfType<ArrowUI>();
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
        _character.Init();
        _moveCardManager.SetCardList(_character.MovingCards);

        _statusWindow.SetEnable(true);
        _statusWindow.SetMoney(_character.Money);
        _statusWindow.SetName(_character.Name);
        _isSelectedCard = true;
    }

    public override void SetRoot()
    {
        NotifyMovingCount(_character.MovingCount);

        // ルート生成
        SetSquareRoots();
        CreateArrow();
    }



    private void UpdateSelect()
    {
        if (_moveCardManager.GetSelectedCardIndex() != -1)
        {
            _statusWindow.SetEnable(false);
            _movingCount.SetEnable(true);
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

        if (_character.State != CharacterState.WAIT) return;

        // マス目を決定する
        if (_character.MovingCount == 0)
        {
            _movingCount.SetEnable(false);
            _character.Stop();
            DeleteArrow();
            _isMoved = false;
            return;
        }

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
    }

}
