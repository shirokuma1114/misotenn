using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBrain : MonoBehaviour
{
    public int _myId { get; private set;}
    public int correctAnswer { get; set; }
    public bool isMyTurn { get; set; }

    void Start()
    {
        _myId = 0;
        correctAnswer = 0;
    }

   
    void Update()
    {
        if(isMyTurn == true)
        {
            //操作ができる
            //if(Input.GetKeyDown(KeyCode.A))
            //スペースキー押したらCardの関数呼ぶ
        }
    }
}
