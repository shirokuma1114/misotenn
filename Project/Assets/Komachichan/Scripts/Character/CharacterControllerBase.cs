using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharacterControllerBase : MonoBehaviour
{
    protected enum EventState
    {
        DEFORM_FLY,
        WAIT,
        SELECT,
        MOVE,
        COLLISION,
        GOAL,
    }

    [SerializeField]
    protected CharacterBase _character;

    [SerializeField]
    private bool _isAutomatic;

    [SerializeField]
    protected SelectWindow _selectWindow;

    public bool IsAutomatic
    {
        get { return _isAutomatic; }
    }

    protected SquareBase[] _directionRoots;

    protected MoveCardManager _moveCardManager;

    protected MovingCountWindow _movingCount;

    protected StatusWindow _statusWindow;

    protected SouvenirWindow _souvenirWindow;

    protected CollisionEvent _collisionEvent;

    protected EventState _eventState;

    public CharacterBase Character
    {
        get { return _character; }
    }

    protected Queue<SquareBase> _root = new Queue<SquareBase>();

    protected int _goalMovingCount = 1;

    [SerializeField]
    CakeAnimation _animation;

    public virtual void InitTurn()
    {
        _eventState = EventState.SELECT;
    }

    //移動カードを選び次のマスに止まるまで
    public virtual void Move()
    {
        _eventState = EventState.DEFORM_FLY;
    }

    public virtual void SetRoot()
    {
        _animation.StartMove();
    }

    protected void NotifyMovingCount(int count)
    {
        _movingCount.SetMovingCount(count);
    }

    protected void StartMove(SquareBase square)
    {
        _character.StartMove(square);
    }

    protected void UpdateMove()
    {
        if (_character.State != CharacterState.WAIT) return;
        if(_eventState == EventState.DEFORM_FLY)
        {
            if (_animation.CanMove()) _eventState = EventState.MOVE;
            return;
        }

        if (_eventState == EventState.SELECT || _eventState == EventState.WAIT) return;

        // マス目を決定する
        if (_character.MovingCount == 0 && _eventState != EventState.COLLISION)
        {
            _movingCount.SetEnable(false);

            // 既に止まっているプレイヤーがいる
            if (_character.CurrentSquare.AlreadyStopped())
            {
                Collision(_character, _character.CurrentSquare.StoppedCharacters.ToList());
                _eventState = EventState.COLLISION;
                _animation.EndMove();
                return;
            }
            //Debug.Log(_startSquare + _character.Name);
            _eventState = EventState.WAIT;
            _character.Stop();
            _animation.EndMove();
            return;
        }

        // 通過ゴール判定
        var goal = _character.CurrentSquare.GetComponent<SquareGoal>();
        if (goal && _goalMovingCount != _character.MovingCount)
        {
            _goalMovingCount = _character.MovingCount;
            Debug.Log("ゴールに止まった！");
            _eventState = EventState.GOAL;
            _character.Stop();
            return;
        }

        if (_eventState == EventState.COLLISION)
        {
            UpdateColliision();
            if (IsFinishedCollision())
            {
                _eventState = EventState.WAIT;
                _character.Stop();
                _animation.EndMove();
                return;
            }
        }

        if (_root.Count == 0) return;
        NotifyMovingCount(_character.MovingCount);
        //Debug.Log(_root.Peek());
        StartMove(_root.Dequeue());
    }

    protected void Collision(CharacterBase owner, List<CharacterBase> targets)
    {
        _collisionEvent = new CollisionEvent(owner, targets);
    }

    protected void UpdateColliision()
    {
        _collisionEvent.Update();
    }

    protected bool IsFinishedCollision()
    {
        if (_collisionEvent == null) return false;
        return _collisionEvent.IsFinished();
    }

    public virtual bool IsTurnFinished()
    {
        return _character.State == CharacterState.END && _eventState == EventState.WAIT;
    }

    protected void DefaultGenerateRoot()
    {
        // ルート生成
        var next = _character.CurrentSquare;
        for (int i = 0; i < _character.MovingCount; i++)
        {
            next = next.OutConnects.Last();
            _root.Enqueue(next);
        }
    }

    public virtual void ReStartMove(int moveCount)
    {
        _movingCount.SetEnable(true);
        _character.AddMovingCard(moveCount);
        _character.RemoveMovingCard(_character.MovingCards.Count - 1);
        DefaultGenerateRoot();
        _animation.StartMove();
        _eventState = EventState.DEFORM_FLY;
    }
}
