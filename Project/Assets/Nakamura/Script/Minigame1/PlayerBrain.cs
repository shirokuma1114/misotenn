using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerBrain : Brain
{
    [SerializeField] private TurnController _turnController;
    [SerializeField] private CardManager _cardMgr;
    [SerializeField] private Color _myColor;
    private int _nowCursol;
    private int _nowStep;
    private int[] _turnCard = new int[3];//めくったカード
    private int[] _correctMemory = new int[3];//正解した場所を記憶する
    public int _myId { get; private set;}//正解のカード画像
    public int correctAnswer { get; set; }//正解数
    public bool isControl { get; set; }
   

    void Start()
    {
        _myId = 0;
        correctAnswer = 0;
        for (int i = 0; i < 3; i++)
        {
            _correctMemory[i] = -1;
        }
    }
   
    void Update()
    {
        if (isControl)
        {
            Debug.Log("isControll = " + isControl);

            //操作ができる
            if (Input.GetKeyDown(KeyCode.A))
            {
                if(_nowCursol > ((_nowStep - 1) * 4))_nowCursol -= 1;
                _cardMgr.SetCursolCurd(_nowCursol, _myColor);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (_nowCursol < _nowStep * 4 - 1) _nowCursol += 1;
                _cardMgr.SetCursolCurd(_nowCursol, _myColor);
            }

            //スペースキー押したらCardの関数呼ぶ
            if(Input.GetKeyDown(KeyCode.Space))
            {
                if (_cardMgr.GetCard(_nowCursol).isCanTurn == false) return;

                isControl = false;

                //カーソル位置と照らし合わせてめくったカードの番号を持って置く
                _turnCard[_nowStep - 1] = _nowCursol;

                //正誤判定
                var _isCorrect = _cardMgr.SetClickCurd(_nowCursol,_myId, _nowStep);
                
                if (_isCorrect)
                {
                    //正解数更新
                    _correctMemory[_nowStep - 1] = _nowCursol;//脳のメモリに入れる
                    if (correctAnswer < _nowStep)
                    {
                        correctAnswer = _nowStep;
                    }
                    _nowStep += 1;

                    //勝った
                    if (correctAnswer == 3)
                    {
                        _turnController.SetWinner(_myId);
                        _cardMgr.SetCardCantTurn(_correctMemory);//カード裏返せなくする
                        _turnController.TurnChange();

                        Debug.Log("プレイヤー勝った");
                        return;
                    }
                    else
                    {
                        //カーソルの位置を次の段の左端にする
                        _nowCursol = (_nowStep - 1) * 4;
                        _cardMgr.SetCursolCurd(_nowCursol, _myColor);

                        isControl = true;
                    }
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
        _cardMgr.SetCursolCurd(_nowCursol, _myColor);

        //めくられたカードの初期化
        for(int i = 0;i < 3; i++)
        {
            _turnCard[i] = -1;
        }

        isControl = true;
    }
}