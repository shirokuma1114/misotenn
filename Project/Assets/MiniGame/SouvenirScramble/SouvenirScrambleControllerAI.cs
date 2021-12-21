using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SouvenirScrambleControllerAI : SouvenirScrambleControllerBase
{
    private SouvenirScrambleItem _target;

    private new void Update()
    {
        base.Update();

        UpdateTarget();
    }

    public override void Operation()
    {
        if (!_target)
            return;

        Vector3 moveDir = (_target.TargetPos - transform.position).normalized;
        _rb.velocity += moveDir.normalized * _moveSpeed * 0.5f;
    }

    private void UpdateTarget()
    {
        if (_target)
            return;

        var targets = FindObjectsOfType<SouvenirScrambleItem>();
        if(targets.Length > 0)
            _target = targets[Random.Range(0, targets.Length)];
    }
}
