using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SouvenirScrambleController : SouvenirScrambleControllerBase
{
    public override void Operation()
    {
        Vector3 moveDir = Vector3.zero;

        //パッド操作
        moveDir.y = _character.Input.GetAxis("Vertical") * -1;
        moveDir.x = _character.Input.GetAxis("Horizontal");

        //一応キー操作(ひとりまで)
        if (Input.GetKey(KeyCode.W))
            moveDir.y = 1;
        if (Input.GetKey(KeyCode.S))
            moveDir.y = -1;
        if (Input.GetKey(KeyCode.D))
            moveDir.x = 1;
        if (Input.GetKey(KeyCode.A))
            moveDir.x = -1;

        _rb.velocity += moveDir.normalized * _moveSpeed * Time.deltaTime;
    }
}
