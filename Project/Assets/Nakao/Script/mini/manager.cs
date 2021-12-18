using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class manager : MonoBehaviour
{
    public enum GameState
    {
        TUTORIAL,
        PLAY_NOW,
        PLAY,
        RESULT,
        END,
    };
    private GameState _state;
    public GameState State => _state;

    private MiniGameConnection _miniGameConnection;

    [SerializeField]
    private Text _Text;

    [SerializeField]
    private List<SampleGameController> _miniGameControllers;

    //[SerializeField]
    //private List<cont> _miniGameControllers;

    [SerializeField]
    private MiniGameResult _miniGameResult;

    public Text _kaitou;
    public Image _amida;
    public Image _amida_kakushi;
    public Sprite[] _sprite;

    public Text _p1;
    public Text _p2;
    public Text _p3;
    public Text _p4;

    private int wk = 0;
    private int a = 0;

    private void Awake()
    {
        _state = GameState.TUTORIAL;
    }
    void Start()
    {
       _miniGameConnection = MiniGameConnection.Instance;

        //foreach (var c in _miniGameConnection.Characters)
        //{
        //    foreach (var mc in _miniGameControllers)
        //    {
        //        if (mc.CakeName == c.Name)
        //            mc.Init(c, this);
        //    }
        //}
    }

    void Update()
    {
        switch (_state)
        {
            case GameState.TUTORIAL:
                TutorialState();
                break;

            case GameState.PLAY_NOW:
                PlayNOWState();
                break;

            case GameState.PLAY:
                PlayState();
                break;

            case GameState.RESULT:
                ResltState();
                break;

            case GameState.END:
                EndState();
                break;
        }
    }

    private void TutorialState()
    {
        _Text.text = "あみだくじ\nEnterでプレイ";
        _kaitou.text = " ";
        _amida_kakushi.enabled = true;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            //ランダムであみだくじを決める
            a = Random.Range(1, 3 + 1);//1～3
    
            switch (a)
            {
                case 0:
                    //_amida.sprite = _sprite[0];
                    break;

                case 1:
                    _amida.sprite = _sprite[0];
                    break;

                case 2:
                    _amida.sprite = _sprite[1];
                    break;

                case 3:
                    _amida.sprite = _sprite[2];
                    break;

                default:
                    //_amida.sprite = _sprite[0];
                    break;

            }

            //ステータス変更
            _state = GameState.PLAY_NOW;
        } 
    }

    private void PlayNOWState()
    {
        _Text.text = "あみだくじ";

        if (Input.GetKeyDown(KeyCode.A))
        {
            wk = 1;
            _kaitou.text = "①";
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            wk = 2;
            _kaitou.text = "②";
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            wk = 3;
            _kaitou.text = "③";
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            wk = 4;
            _kaitou.text = "④";
        }

        if(wk > 0)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                _state = GameState.PLAY;
            }
        }
    }

    private void PlayState()
    {
        _amida_kakushi.enabled = false;

        int b = 0;
        b = Random.Range(1, 4 + 1);//1～4

       

        //順位処理
        if (a == 1)
       {
            switch (wk)
            {
                //case 0:
                    //PlayNOWState();
                    //break;

                //2位
                case 1:
                    _p1.text = "2位";

                    switch (b)
                    {
                        case 1:
                            _p2.text = "1位";
                            _p3.text = "3位";
                            _p4.text = "4位";
                            break;

                        case 2:
                            _p2.text = "1位";
                            _p3.text = "4位";
                            _p4.text = "3位";
                            break;

                        case 3:
                            _p2.text = "4位";
                            _p3.text = "1位";
                            _p4.text = "3位";
                            break;

                        case 4:
                            _p2.text = "3位";
                            _p3.text = "4位";
                            _p4.text = "1位";
                            break;
                    }

                    wk = 0;
                    break;

                //3位
                case 2:
                    _p1.text = "3位";

                    switch (b)
                    {
                        case 1:
                            _p2.text = "1位";
                            _p3.text = "2位";
                            _p4.text = "4位";
                            break;

                        case 2:
                            _p2.text = "1位";
                            _p3.text = "4位";
                            _p4.text = "2位";
                            break;

                        case 3:
                            _p2.text = "4位";
                            _p3.text = "1位";
                            _p4.text = "2位";
                            break;

                        case 4:
                            _p2.text = "2位";
                            _p3.text = "4位";
                            _p4.text = "1位";
                            break;
                    }

                    wk = 0;
                    break;

                //4位
                case 3:
                    _p1.text = "4位";

                    switch (b)
                    {
                        case 1:
                            _p2.text = "1位";
                            _p3.text = "2位";
                            _p4.text = "3位";
                            break;

                        case 2:
                            _p2.text = "1位";
                            _p3.text = "3位";
                            _p4.text = "2位";
                            break;

                        case 3:
                            _p2.text = "3位";
                            _p3.text = "1位";
                            _p4.text = "2位";
                            break;

                        case 4:
                            _p2.text = "2位";
                            _p3.text = "3位";
                            _p4.text = "1位";
                            break;
                    }

                    wk = 0;
                    break;

                //1位
                case 4:
                    _p1.text = "1位";

                    switch (b)
                    {
                        case 1:
                            _p2.text = "3位";
                            _p3.text = "2位";
                            _p4.text = "4位";
                            break;

                        case 2:
                            _p2.text = "3位";
                            _p3.text = "4位";
                            _p4.text = "2位";
                            break;

                        case 3:
                            _p2.text = "4位";
                            _p3.text = "3位";
                            _p4.text = "2位";
                            break;

                        case 4:
                            _p2.text = "2位";
                            _p3.text = "4位";
                            _p4.text = "3位";
                            break;
                    }

                    wk = 0;
                    break;
            }
       }
        if (a == 2)
        {
            switch (wk)
            {
                //case 0:
                    //PlayNOWState();
                    //break;

                //1位
                case 1:
                    _p1.text = "1位";
                    switch (b)
                    {
                        case 1:
                            _p2.text = "3位";
                            _p3.text = "2位";
                            _p4.text = "4位";
                            break;

                        case 2:
                            _p2.text = "3位";
                            _p3.text = "4位";
                            _p4.text = "2位";
                            break;

                        case 3:
                            _p2.text = "4位";
                            _p3.text = "3位";
                            _p4.text = "2位";
                            break;

                        case 4:
                            _p2.text = "2位";
                            _p3.text = "4位";
                            _p4.text = "3位";
                            break;
                    }

                    wk = 0;
                    break;

                //3位
                case 2:
                    _p1.text = "3位";

                    switch (b)
                    {
                        case 1:
                            _p2.text = "1位";
                            _p3.text = "2位";
                            _p4.text = "4位";
                            break;

                        case 2:
                            _p2.text = "1位";
                            _p3.text = "4位";
                            _p4.text = "2位";
                            break;

                        case 3:
                            _p2.text = "4位";
                            _p3.text = "1位";
                            _p4.text = "2位";
                            break;

                        case 4:
                            _p2.text = "2位";
                            _p3.text = "4位";
                            _p4.text = "1位";
                            break;
                    }

                    wk = 0;
                    break;

                //2位
                case 3:
                    _p1.text = "2位";

                    switch (b)
                    {
                        case 1:
                            _p2.text = "1位";
                            _p3.text = "3位";
                            _p4.text = "4位";
                            break;

                        case 2:
                            _p2.text = "1位";
                            _p3.text = "4位";
                            _p4.text = "3位";
                            break;

                        case 3:
                            _p2.text = "4位";
                            _p3.text = "1位";
                            _p4.text = "3位";
                            break;

                        case 4:
                            _p2.text = "3位";
                            _p3.text = "4位";
                            _p4.text = "1位";
                            break;
                    }

                    wk = 0;
                    break;

                //4位
                case 4:
                    _p1.text = "4位";

                    switch (b)
                    {
                        case 1:
                            _p2.text = "1位";
                            _p3.text = "2位";
                            _p4.text = "3位";
                            break;

                        case 2:
                            _p2.text = "1位";
                            _p3.text = "3位";
                            _p4.text = "2位";
                            break;

                        case 3:
                            _p2.text = "3位";
                            _p3.text = "1位";
                            _p4.text = "2位";
                            break;

                        case 4:
                            _p2.text = "2位";
                            _p3.text = "3位";
                            _p4.text = "1位";
                            break;
                    }

                    wk = 0;
                    break;
            }
        }
        if (a == 3)
        {
            switch (wk)
            {
                //case 0:
                    //PlayNOWState();
                    //break;

                //2位
                case 1:
                    _p1.text = "2位";

                    switch (b)
                    {
                        case 1:
                            _p2.text = "1位";
                            _p3.text = "3位";
                            _p4.text = "4位";
                            break;

                        case 2:
                            _p2.text = "1位";
                            _p3.text = "4位";
                            _p4.text = "3位";
                            break;

                        case 3:
                            _p2.text = "4位";
                            _p3.text = "1位";
                            _p4.text = "3位";
                            break;

                        case 4:
                            _p2.text = "3位";
                            _p3.text = "4位";
                            _p4.text = "1位";
                            break;
                    }

                    wk = 0;
                    break;

                //2位
                case 2:
                    _p1.text = "2位";

                    switch (b)
                    {
                        case 1:
                            _p2.text = "1位";
                            _p3.text = "3位";
                            _p4.text = "4位";
                            break;

                        case 2:
                            _p2.text = "1位";
                            _p3.text = "4位";
                            _p4.text = "3位";
                            break;

                        case 3:
                            _p2.text = "4位";
                            _p3.text = "1位";
                            _p4.text = "3位";
                            break;

                        case 4:
                            _p2.text = "3位";
                            _p3.text = "4位";
                            _p4.text = "1位";
                            break;
                    }

                    wk = 0;
                    break;

                //4位
                case 3:
                    _p1.text = "4位";

                    switch (b)
                    {
                        case 1:
                            _p2.text = "1位";
                            _p3.text = "2位";
                            _p4.text = "3位";
                            break;

                        case 2:
                            _p2.text = "1位";
                            _p3.text = "3位";
                            _p4.text = "2位";
                            break;

                        case 3:
                            _p2.text = "3位";
                            _p3.text = "1位";
                            _p4.text = "2位";
                            break;

                        case 4:
                            _p2.text = "3位";
                            _p3.text = "4位";
                            _p4.text = "1位";
                            break;
                    }

                    wk = 0;
                    break;

                //3位
                case 4:
                    _p1.text = "3位";

                    switch (b)
                    {
                        case 1:
                            _p2.text = "1位";
                            _p3.text = "2位";
                            _p4.text = "4位";
                            break;

                        case 2:
                            _p2.text = "1位";
                            _p3.text = "4位";
                            _p4.text = "2位";
                            break;

                        case 3:
                            _p2.text = "4位";
                            _p3.text = "1位";
                            _p4.text = "2位";
                            break;

                        case 4:
                            _p2.text = "2位";
                            _p3.text = "4位";
                            _p4.text = "1位";
                            break;
                    }

                    wk = 0;
                    break;
            }
        }

        _state = GameState.RESULT;

        //デバッグ用
        //if (Input.GetKeyDown(KeyCode.Return))
        //{
        //    _state = GameState.TUTORIAL;
        //}
    }

    private void ResltState()
    {
        _Text.text = "リザルト\nEnterでゲームへ";

        if (Input.GetKeyDown(KeyCode.Return))
        {
            _state = GameState.END;
        }  
    }

    private void EndState()
    {
        _miniGameConnection.EndMiniGame();
    }


    //順位付け
    //private Dictionary<MiniGameCharacter, int> Ranking()
    //{
    //    Dictionary<MiniGameCharacter, int> dispCharacters = new Dictionary<MiniGameCharacter, int>();
    //    List<SampleGameController> workCharacters = new List<SampleGameController>();
    //    workCharacters.AddRange(_miniGameControllers);

    //    List<SampleGameController> sameRanks = new List<SampleGameController>();
    //    for (int rank = 1; rank <= 4;)
    //    {
    //        int maxRenda = -1;
    //        foreach (var chara in workCharacters)
    //        {
    //            if (chara.RendaCount > maxRenda)
    //            {
    //                sameRanks.Clear();

    //                sameRanks.Add(chara);
    //                maxRenda = chara.RendaCount;
    //            }
    //            else if (chara.RendaCount == maxRenda)
    //            {
    //                sameRanks.Add(chara);
    //            }
    //        }

    //        foreach (var ranker in sameRanks)
    //        {
    //            dispCharacters.Add(ranker.Character, rank);
    //            workCharacters.Remove(ranker);
    //        }
    //        rank += sameRanks.Count;
    //    }

    //    return dispCharacters;
    //}
}
