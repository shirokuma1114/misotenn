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

    protected CollisionEvent _collisionEvent;

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

    protected void NotifyMovingCount(int count)
    {
        _movingCount.SetMovingCount(count);
    }

    protected void StartMove(SquareBase square)
    {
        _character.StartMove(square);
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
}
