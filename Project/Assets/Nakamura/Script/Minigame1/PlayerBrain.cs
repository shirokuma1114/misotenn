using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerBrain : MonoBehaviour
{
    [SerializeField] private TurnController _turnController;
    [SerializeField] private CardManager _cardMgr;
    [SerializeField] private Color _playerColor;
    private int _nowCursol;
    private int _nowStep;
    private int[] _turnCard = new int[3];//めくったカード

    public int _myId { get; private set;}//正解のカード画像
    public int correctAnswer { get; set; }//正解数
    public bool isControl { get; set; }
   
    void Start()
    {
        _myId = 0;
        correctAnswer = 0;

        //仮に自分からとしておく
        StartTurn();
    }
   
    void Update()
    {
        if (isControl)
        {
            //操作ができる
            if (Input.GetKeyDown(KeyCode.A))
            {
                if(_nowCursol > ((_nowStep - 1) * 4))_nowCursol -= 1;
                _cardMgr.SetCursolCurd(_nowCursol, _playerColor);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (_nowCursol < _nowStep * 4 - 1) _nowCursol += 1;
                _cardMgr.SetCursolCurd(_nowCursol, _playerColor);
            }

            //スペースキー押したらCardの関数呼ぶ
            if(Input.GetKeyDown(KeyCode.Space))
            {
                isControl = false;

                //カーソル位置と照らし合わせてめくったカードの番号を持って置く
                _turnCard[_nowStep - 1] = _nowCursol;

                //正誤判定
                var _isCorrect = _cardMgr.SetClickCurd(_nowCursol,_myId, _nowStep);
                
                if (_isCorrect)
                {
                    //正解数更新
                    if (correctAnswer < _nowStep)
                    {
                        correctAnswer = _nowStep;
                    }
                    _nowStep += 1;

    
                    //カーソルの位置を次の段の左端にする
                    _nowCursol = (_nowStep - 1) * 4;
                    _cardMgr.SetCursolCurd(_nowCursol, _playerColor);

                    DOVirtual.DelayedCall(0.5f, () => isControl = true);
                }
                else
                {
                    //ターン終了処理
                    DOVirtual.DelayedCall(2, () =>
                    {
                        _cardMgr.ResetCards(_turnCard);
                        _turnController.TurnChange();
                    });
                }
            }
        }
    }

    //プレイヤーのターンの最初の初期化処理
    public void StartTurn()
    {
        _nowStep = 1;

        //カーソル設定
        _nowCursol = 0;
        _cardMgr.SetCursolCurd(_nowCursol, _playerColor);

        //めくられたカードの初期化
        for(int i = 0;i < 3; i++)
        {
            _turnCard[i] = -1;
        }

        isControl = true;
    }
}