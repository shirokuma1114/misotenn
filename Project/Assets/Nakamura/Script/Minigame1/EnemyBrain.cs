using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyBrain : Brain
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

    float _beforeTrigger;


    void Start()
    {
        isControl = false;
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

    void Update()
    {
        if (isControl)
        {
            //操作ができる
            if (_miniGameChara.Input == null) return;
            float viewButton = _miniGameChara.Input.GetAxis("Horizontal");
            if (_beforeTrigger == 0.0f)
            {
                if (viewButton < 0)
                {
                    if (_nowCursol > ((_nowStep - 1) * 4)) _nowCursol -= 1;
                    _cardMgr.SetCursolCurd(_nowCursol, _myColor);

                    if (Control_SE.Get_Instance()) Control_SE.Get_Instance().Play_SE("UI_Select");
                }
                if (viewButton > 0)
                {
                    if (_nowCursol < _nowStep * 4 - 1) _nowCursol += 1;
                    _cardMgr.SetCursolCurd(_nowCursol, _myColor);

                    if (Control_SE.Get_Instance()) Control_SE.Get_Instance().Play_SE("UI_Select");
                }
            }
            _beforeTrigger = viewButton;
            
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (_nowCursol > ((_nowStep - 1) * 4)) _nowCursol -= 1;
                _cardMgr.SetCursolCurd(_nowCursol, _myColor);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (_nowCursol < _nowStep * 4 - 1) _nowCursol += 1;
                _cardMgr.SetCursolCurd(_nowCursol, _myColor);
            }

            //スペースキー押したらCardの関数呼ぶ
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || _miniGameChara.Input.GetButtonDown("A"))
            {
                if (_cardMgr.GetCard(_nowCursol).isCanTurn == false) return;

                isControl = false;

                //カーソル位置と照らし合わせてめくったカードの番号を持って置く
                _turnCard[_nowStep - 1] = _nowCursol;

                //正誤判定
                var _isCorrect = _cardMgr.SetClickCurd(_nowCursol, _myId, _nowStep);

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

                        Debug.Log("勝った敵脳内");
                        return;
                    }
                    else
                    {
                        //カーソルの位置を次の段の左端にする
                        _nowCursol = (_nowStep - 1) * 4;
                        _cardMgr.SetCursolCurd(_nowCursol, _myColor);

                        DOVirtual.DelayedCall(0.5f, () => isControl = true);
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

        if(_miniGameChara.IsAutomatic == false) isControl = true;
    }

    //カードをめくる処理 再帰しがち
    public void TurnCard()
    {
        //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
        //　選ぶカードを決める
        //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
        var _isRandom = true;
        var _selectCard = -1;//選ぶカード

        //一度でも正解のカードを引いたことがあるか？
        //引いたことがある
        if(_correctMemory[_nowStep - 1] != -1)
        {
           var ran = Random.Range(0.0f, 1.0f);
            //自分のカードの位置を覚えてる
            if (ran < _correctMemoryRate) _selectCard = _correctMemory[_nowStep - 1];
            //自分のカードの位置を覚えてない＜完全ランダム＞
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
        //脳内に正解がない場合
        else
        {
            //自分のカードがめくられたのを見たことがある
            if (lookMemory[_nowStep - 1] != -1)
            {
                var ranLook = Random.Range(0.0f, 1.0f);
                //自分のカードの位置を思い出せた
                if (ranLook < _lookMemoryRate)
                {
                    _selectCard = lookMemory[_nowStep - 1];
                }
                //思い出せなかった＜完全ランダム＞
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
            //自分のカードのありかをみたことがない＜完全ランダム＞
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
        //　カーソル移動
        //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
        var _dis = _selectCard -_nowCursol;
     
        if(_dis != 0)
        {
            StartCoroutine(CursolChange(_dis));
        }

        //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
        //　２秒考えて、めくる
        //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
        DOVirtual.DelayedCall(2, () =>
        {
            _nowCursol = _selectCard;
            _cardMgr.SetCursolCurd(_nowCursol, _myColor);

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

    //カーソル移動コルーチン
    private IEnumerator CursolChange(int _dis)
    {
        while (true) // このGameObjectが有効な間実行し続ける
        {
            yield return new WaitForSeconds(0.8f);

            // 上で指定した秒毎に実行する
            if (_dis < 0)
            {
                _nowCursol -= 1;
                _dis += 1;
            }
            else
            {
                _nowCursol += 1;
                _dis -= 1;
            }
            _cardMgr.SetCursolCurd(_nowCursol, _myColor);

            if (_dis == 0) yield break;
        }
    }

    public int GetId() { return _myId; } 

    //AIの精度をあげるアイデア（実装してないやつ）
    //・自分のカードを見たことがないとき外す確率
    //・他の人がカードをめくった時、そのときの位置をメモリに入れておく
    //・↑の自分のカード以外の位置と覚えている確率をわける
}