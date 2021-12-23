using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnController : MonoBehaviour
{
    [SerializeField] private Text _turnText;
    //[SerializeField] private PlayerBrain _player;
    [SerializeField] private EnemyBrain[] _enemy; 
    [SerializeField] private Image _nowTurnArrow;
    [SerializeField] private Text[] _names;
    [SerializeField] private MiniGameConnection _miniGameConnection;
    [SerializeField] private MiniGameResult _miniGameResult;

    public int[] gameOrder = new int[4];//0～３のキャラクターIDが入る
    private int[] _winner = new int[4];//勝った人のIDを入れておく
    
    private int _nowTurnOrder = 0;
    private bool _isGameEnd = false;

    private bool _isJoJo;

    [SerializeField]
    private SandbyManager _standby;

    private bool _isTutorial = true;
    public bool IsTutorial => _isTutorial;
    private bool _isGameStart = false;

   public void Init()
   {
        _miniGameConnection = MiniGameConnection.Instance;

        //_player._miniGameChara = _miniGameConnection.Characters[0];
        for (int i = 0; i < _enemy.Length; i++)
        {
            _enemy[i]._miniGameChara = _miniGameConnection.Characters[i];
        }
        //ゲームの順番を決める
        turnRoulette(gameOrder);
       
        for (int i = 0; i < _winner.Length; i++)
        {
            _winner[i] = -1;
        }

        for(int i = 0;i < _names.Length;i++)
        {
            _names[i].text = _enemy[i]._miniGameChara.Name;
        }

        _turnText.text = "の番です";
        TurnChange();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_standby.gameObject.activeSelf && !_isGameStart)
        {
            Init();

            _isTutorial = false;
            _isGameStart = true;
        }

        if (_isGameEnd)
        {
            //リザルトを出す
            Dictionary<MiniGameCharacter, int> rank = new Dictionary<MiniGameCharacter, int>();
            for (int i = 0; i < _winner.Length; i++)
            {
               if(_winner[i] == 0)
               {
                   rank.Add(_enemy[0]._miniGameChara, (i + 1));
               }
               else if (_winner[i] == 1)
               {
                   rank.Add(_enemy[1]._miniGameChara, (i + 1));
               }
               else if (_winner[i] == 2)
               {
                   rank.Add(_enemy[2]._miniGameChara, (i + 1));
               }
               else if (_winner[i] == 3)
               {
                   rank.Add(_enemy[3]._miniGameChara, (i + 1));
               }
            }
            //_miniGameResult.
            _miniGameResult.Display(rank);

            _isGameEnd = false;

            _isJoJo = true;

        }

        if (_isJoJo)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("A") || Input.GetButtonDown("Start"))
            {
                _miniGameConnection.EndMiniGame();
            }
        }
    }

    //ターンを変える関数(キャラクターが呼び出す)
    public void TurnChange()
    {
        if (_isGameEnd) return;

        //今のターンと比べて位置を把握してそれによって移動数変える
        var _allowPosY = 0.0f;
        var rect = _nowTurnArrow.gameObject.GetComponent<RectTransform>();

        //もう勝ってる人ならスキップする
        var _isSkip = false;
        for(int i = 0;i < _winner.Length;i++)
        {
            if(_winner[i] == gameOrder[_nowTurnOrder])
            {
                _isSkip = true;
                break;
            }
        }
        if(_isSkip)
        {
            Debug.Log("スキップ" + gameOrder[_nowTurnOrder]);
            if (_nowTurnOrder < 3) _nowTurnOrder += 1;
            else _nowTurnOrder = 0;
            TurnChange();//再帰...
            return;
        }

        Debug.Log(gameOrder[_nowTurnOrder] + "のターン");

        switch (gameOrder[_nowTurnOrder])
        {
            //プレイヤー
            case 0:
                _turnText.text = _enemy[0]._miniGameChara.Name + "　の番です";
                rect.localPosition = new Vector3(-270, 180, 0);
                _enemy[0].StartTurn();
                if (_enemy[0]._miniGameChara.IsAutomatic == true) _enemy[0].TurnCard();
                break;

            //エネミー１
            case 1:
                _turnText.text = _enemy[1]._miniGameChara.Name + "　の番です";
                rect.localPosition = new Vector3(-270, 140, 0);
                _enemy[1].StartTurn();
                if(_enemy[1]._miniGameChara.IsAutomatic == true) _enemy[1].TurnCard();
                break;

            //エネミー２
            case 2:
                _turnText.text = _enemy[2]._miniGameChara.Name + "　の番です";
                rect.localPosition = new Vector3(-270, 100, 0);

                _enemy[2].StartTurn();
                if (_enemy[2]._miniGameChara.IsAutomatic == true) _enemy[2].TurnCard();
                break;

            //エネミー３
            case 3:
                _turnText.text = _enemy[3]._miniGameChara.Name + "　の番です";
                rect.localPosition = new Vector3(-270, 60, 0);

                _enemy[3].StartTurn();
                if (_enemy[3]._miniGameChara.IsAutomatic == true) _enemy[3].TurnCard();
                break;
        }


        if (_nowTurnOrder < 3) _nowTurnOrder += 1;
        else _nowTurnOrder = 0;
    }

    //勝った人を保存
    public void SetWinner(int _id)
    {
        for(int i = 0;i < _winner.Length;i++)
        {
            if (_winner[i] == -1)
            {
                _winner[i] = _id;
                if (i == _winner.Length - 1) _isGameEnd = true;
                break;
            }
        }
    }
    
    //ルーレット用アニメーション関数
    private int[] turnRoulette(int[] _order)
    {
        int ran = Random.Range(0, 4);

        for(int i = 0;i < _order.Length;i++)
        {
            if(ran + i > 3) _order[i] = (ran + i) - 4;
            else _order[i] = ran + i;
        }

        return _order;
    }
}