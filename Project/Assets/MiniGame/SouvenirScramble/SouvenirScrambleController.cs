using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SouvenirScrambleController : SouvenirScrambleControllerBase
{
    public override void Operation()
    {
        Vector3 moveDir = Vector3.zero;
        moveDir.y = _character.Input.GetAxis("Vertical") * -1;
        moveDir.x = _character.Input.GetAxis("Horizontal");
        _rb.velocity += moveDir.normalized * _moveSpeed;
    }
}
