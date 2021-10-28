using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharacterControllerBase : MonoBehaviour
{
    protected enum EventState
    {
        WAIT,
        MOVE,
        COLLISION,
        GOAL,
    }

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

    protected SouvenirWindow _souvenirWindow;

    protected CollisionEvent _collisionEvent;

    protected EventState _eventState;

    public CharacterBase Character
    {
        get { return _character; }
    }

    protected Queue<SquareBase> _root = new Queue<SquareBase>();

    protected SquareBase _startSquare;

    //移動カードを選び次のマスに止まるまで
    public virtual void Move()
    {
        _eventState = EventState.MOVE;
        _startSquare = _character.CurrentSquare;
    }

    protected virtual void SetRoot()
    {

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
        if (_eventState == EventState.WAIT) return;
        
        // マス目を決定する
        if (_character.MovingCount == 0 && _eventState != EventState.COLLISION)
        {
            _movingCount.SetEnable(false);

            // 既に止まっているプレイヤーがいる
            if (_character.CurrentSquare.AlreadyStopped())
            {
                Collision(_character, _character.CurrentSquare.StoppedCharacters.ToList());
                _eventState = EventState.COLLISION;
                return;
            }
            //Debug.Log(_startSquare + _character.Name);
            _eventState = EventState.WAIT;
            _character.Stop();
            return;
        }

        // 通過ゴール判定
        var goal = _character.CurrentSquare.GetComponent<SquareGoal>();
        if (goal && _startSquare != goal)
        {
            Debug.Log(_startSquare);
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
}
