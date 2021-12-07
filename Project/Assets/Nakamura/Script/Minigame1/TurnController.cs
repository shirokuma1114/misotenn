using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnController : MonoBehaviour
{
    [SerializeField] private Text _turnText;
    [SerializeField] private PlayerBrain _player;
    [SerializeField] private EnemyBrain[] _enemy; 
    [SerializeField] private Image _nowTurnArrow;

    public int[] gameOrder = new int[4];//0～３のキャラクターIDが入る
    private int[] _winner = new int[4];//勝った人のIDを入れておく
    
    private int _nowTurnCharaId = 0;
    private bool _isGameEnd = false;
    
   public void Init()
    {
        //ゲームの順番を決める
        for(int i = 0;i < gameOrder.Length;i++)
        {
            gameOrder[i] = i;
            //後でランダム化する
        }

        for (int i = 0; i < _winner.Length; i++)
        {
            _winner[i] = -1;
        }

        // テキストの表示を入れ替える
        _turnText.text = "こまち社長　の番です";
        _player.StartTurn();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isGameEnd)
        {
            //リザルトキャンバスを出す
        }
    }

    //ターンを変える関数(キャラクターが呼び出す)
    public void TurnChange()
    {
        //今のターンと比べて位置を把握してそれによって移動数変える
        var _allowPosY = 0.0f;
        var rect = _nowTurnArrow.gameObject.GetComponent<RectTransform>();

        if (_nowTurnCharaId < 3) _nowTurnCharaId += 1;
        else _nowTurnCharaId = 0;

        //もう勝ってる人ならスキップする
        for(int i = 0;i < _winner.Length;i++)
        {
            if(_winner[i] == _nowTurnCharaId)
            {
                Debug.Log("スキップ" + _nowTurnCharaId);
                TurnChange();//再帰...
                return;
            }
        }

        switch (gameOrder[_nowTurnCharaId])
        {
            //プレイヤー
            case 0:
                _turnText.text = "こまち社長　の番です";
     
                rect.localPosition += new Vector3(0, 120, 0);

                _player.StartTurn();
                break;

            //エネミー１
            case 1:
                _turnText.text = "敵１号　の番です";
                rect.localPosition += new Vector3(0, -40, 0);
                _enemy[0].StartTurn();
                _enemy[0].TurnCard();
                break;

            //エネミー２
            case 2:
                _turnText.text = "敵２号　の番です";
                rect.localPosition += new Vector3(0, -40, 0);

                _enemy[1].StartTurn();
                _enemy[1].TurnCard();
                break;

            //エネミー３
            case 3:
                _turnText.text = "敵３号　の番です";
                rect.localPosition += new Vector3(0, -40, 0);

                _enemy[2].StartTurn();
                _enemy[2].TurnCard();
                break;
        }
    }

    //勝った人を保存
    public void SetWinner(int _id)
    {
        for(int i = 0;i < _winner.Length;i++)
        {
            if (_winner[i] == -1)
            {
                _winner[i] = _id;
                break;
            }
        }
    }
    
    //ルーレット用アニメーション関数
}
