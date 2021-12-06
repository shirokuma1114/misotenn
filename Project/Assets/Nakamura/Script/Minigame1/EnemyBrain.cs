using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyBrain : MonoBehaviour
{
    //プレイヤー共通
    [SerializeField] private TurnController _turnController;
    [SerializeField] private CardManager _cardMgr;
    [SerializeField] private Color _myColor;
    [SerializeField] private int _myId;//正解のカード画像
    private int _nowCursol;
    private int _nowStep;
    private int[] _turnCard = new int[3];//めくったカード
    public int correctAnswer { get; set; }//正解数
    public bool isControl { get; set; }

    //敵なんちゃってAI関連
    [SerializeField] private float _correctMemoryRate;//一度正解したものを記憶しておける確率（0.0 ~ 1.0）
    [SerializeField] private float _lookMemoryRate;//他の人がめくったのを記憶しておける確率（0.0 ~ 1.0） 
    private int[] _correctMemory = new int[3];//正解した場所を記憶する
    private int[] _incorrectMemory = new int[3];//間違えた場所を記憶する
    public int[] lookMemory = new int[3]; //他の人がめくったのが目に入った（場所が入る）

    void Start()
    {
        _nowStep = 1;
        correctAnswer = 0;
        
        //脳内からっぽ
        for(int i = 0;i < 3;i++)
        {
            _correctMemory[i] = -1;
            _incorrectMemory[i] = -1;
            lookMemory[i] = -1;
        }
    }
        
    //ターンの最初の初期化処理
    public void StartTurn()
    {
        //カーソル設定
        _nowStep = 1;
        _nowCursol = 0;
        _cardMgr.SetCursolCurd(_nowCursol, _myColor);

        //めくられたカードの初期化
        for (int i = 0; i < 3; i++)
        {
            _turnCard[i] = -1;
        }

        isControl = true;
    }

    //カードをめくる処理 再帰しがち
    public void TurnCard()
    {
        //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
        //　選ぶカードを決める
        //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
        var _isRandom = true;
        var _selectCard = -1;//選ぶカード
        //脳内メモリに正解があるか探す
        if(_correctMemory[_nowStep - 1] != -1)
        {
           var ran = Random.Range(0.0f, 1.0f);
            if (ran < _correctMemoryRate) _selectCard = _correctMemory[_nowStep - 1];
            else
            {
                if (lookMemory[_nowStep - 1] != -1)
                {
                    var ranLook = Random.Range(0.0f, 1.0f);
                    if (ranLook < _lookMemoryRate)
                    {
                        _selectCard = lookMemory[_nowStep - 1];
                    }
                    else
                    {
                        var ran2 = Random.Range(0, 4);
                        _selectCard = ran2 + ((_nowStep - 1) * 4);

                        //前に同じ間違いをしていたら引き直す
                        for (int i = 0; i < _incorrectMemory.Length; i++)
                        {
                            if (_incorrectMemory[i] == _selectCard)
                            {
                                TurnCard();
                                return;
                            }
                        }

                        //既に勝った敵がめくっているカードなら引き直す
                        if (_cardMgr.GetCard(_selectCard).isCanTurn == false)
                        {
                            TurnCard();
                            return;
                        }
                    }
                }
                else
                {
                    if (lookMemory[_nowStep - 1] != -1)
                    {
                        var ranLook = Random.Range(0.0f, 1.0f);
                        if (ranLook < _lookMemoryRate)
                        {
                            _selectCard = lookMemory[_nowStep - 1];
                        }
                        else
                        {
                            var ran2 = Random.Range(0, 4);
                            _selectCard = ran2 + ((_nowStep - 1) * 4);

                            //前に同じ間違いをしていたら引き直す
                            for (int i = 0; i < _incorrectMemory.Length; i++)
                            {
                                if (_incorrectMemory[i] == _selectCard)
                                {
                                    TurnCard();
                                    return;
                                }
                            }

                            //既に勝った敵がめくっているカードなら引き直す
                            if (_cardMgr.GetCard(_selectCard).isCanTurn == false)
                            {
                                TurnCard();
                                return;
                            }
                        }
                    }
                    else
                    {
                        var ran2 = Random.Range(0, 4);
                        _selectCard = ran2 + ((_nowStep - 1) * 4);

                        //前に同じ間違いをしていたら引き直す
                        for (int i = 0; i < _incorrectMemory.Length; i++)
                        {
                            if (_incorrectMemory[i] == _selectCard)
                            {
                                TurnCard();
                                return;
                            }
                        }

                        //既に勝った敵がめくっているカードなら引き直す
                        if (_cardMgr.GetCard(_selectCard).isCanTurn == false)
                        {
                            TurnCard();
                            return;
                        }
                    }
                }
            }
        }
        else
        {
            if (lookMemory[_nowStep - 1] != -1)
            {
                var ranLook = Random.Range(0.0f, 1.0f);
                if (ranLook < _lookMemoryRate)
                {
                    _selectCard = lookMemory[_nowStep - 1];
                }
                else
                {
                    var ran2 = Random.Range(0, 4);
                    _selectCard = ran2 + ((_nowStep - 1) * 4);

                    //前に同じ間違いをしていたら引き直す
                    for (int i = 0; i < _incorrectMemory.Length; i++)
                    {
                        if (_incorrectMemory[i] == _selectCard)
                        {
                            TurnCard();
                            return;
                        }
                    }

                    //既に勝った敵がめくっているカードなら引き直す
                    if (_cardMgr.GetCard(_selectCard).isCanTurn == false)
                    {
                        TurnCard();
                        return;
                    }
                }
            }
            else
            {
                var ran2 = Random.Range(0, 4);
                _selectCard = ran2 + ((_nowStep - 1) * 4);

                //前に同じ間違いをしていたら引き直す
                for (int i = 0; i < _incorrectMemory.Length; i++)
                {
                    if (_incorrectMemory[i] == _selectCard)
                    {
                        TurnCard();
                        return;
                    }
                }

                //既に勝った敵がめくっているカードなら引き直す
                if (_cardMgr.GetCard(_selectCard).isCanTurn == false)
                {
                    TurnCard();
                    return;
                }
            }
        }

        //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
        //　移動
        //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
        DOVirtual.DelayedCall(2, () =>
        {
            _nowCursol = _selectCard;
            _cardMgr.SetCursolCurd(_nowCursol, _myColor);

            //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
            //　めくる
            //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
            isControl = false;
            //カーソル位置と照らし合わせてめくったカードの番号を持って置く
            _turnCard[_nowStep - 1] = _nowCursol;
            //正誤判定
            var _isCorrect = _cardMgr.SetClickCurd(_nowCursol, _myId, _nowStep);

            if (_isCorrect)
            {
                //正解数更新
                _correctMemory[_nowStep - 1] = _nowCursol;//脳のメモリに入れる
                if (correctAnswer < _nowStep)//前に正解していないところを正解したら
                {
                    correctAnswer = _nowStep;
                    
                    //間違いメモリー初期化
                    for (int i = 0; i < 3; i++)
                    {
                        _incorrectMemory[i] = -1;
                    }
                }
                _nowStep += 1;

                //勝った
                if (correctAnswer == 3)
                {
                    _turnController.SetWinner(_myId);
                    _cardMgr.SetCardCantTurn(_correctMemory);//カード裏返せなくする
                    _turnController.TurnChange();

                    Debug.Log("勝った敵脳内");
                    return;
                }
                else
                {
                    //カーソルの位置を次の段の左端にする
                    _nowCursol = (_nowStep - 1) * 4;
                    _cardMgr.SetCursolCurd(_nowCursol, _myColor);

                    DOVirtual.DelayedCall(0.5f, () =>
                    {
                        isControl = true;
                        TurnCard();
                    });
                }
            }
            else
            {
                //間違いメモリーに入れる
                for (int i = 0; i < 3; i++)
                {
                    if (_incorrectMemory[i] == -1) _incorrectMemory[i] = _nowCursol;
                }

                //ターン終了処理
                DOVirtual.DelayedCall(2, () =>
                {
                    _cardMgr.ResetCards(_turnCard);
                    _turnController.TurnChange();
                });
            }
        });
    }

    public int GetId() { return _myId; } 

    //他の人が自分のカードをめくって自分の位置にいれる処理欲しいね
}