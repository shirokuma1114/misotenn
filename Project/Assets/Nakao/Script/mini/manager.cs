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
        _Text.text = "���݂�����\nEnter�Ńv���C";
        _kaitou.text = " ";
        _amida_kakushi.enabled = true;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            //�����_���ł��݂����������߂�
            a = Random.Range(1, 3 + 1);//1�`3
    
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

            //�X�e�[�^�X�ύX
            _state = GameState.PLAY_NOW;
        } 
    }

    private void PlayNOWState()
    {
        _Text.text = "���݂�����";

        if (Input.GetKeyDown(KeyCode.A))
        {
            wk = 1;
            _kaitou.text = "�@";
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            wk = 2;
            _kaitou.text = "�A";
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            wk = 3;
            _kaitou.text = "�B";
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            wk = 4;
            _kaitou.text = "�C";
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
        b = Random.Range(1, 4 + 1);//1�`4

       

        //���ʏ���
        if (a == 1)
       {
            switch (wk)
            {
                //case 0:
                    //PlayNOWState();
                    //break;

                //2��
                case 1:
                    _p1.text = "2��";

                    switch (b)
                    {
                        case 1:
                            _p2.text = "1��";
                            _p3.text = "3��";
                            _p4.text = "4��";
                            break;

                        case 2:
                            _p2.text = "1��";
                            _p3.text = "4��";
                            _p4.text = "3��";
                            break;

                        case 3:
                            _p2.text = "4��";
                            _p3.text = "1��";
                            _p4.text = "3��";
                            break;

                        case 4:
                            _p2.text = "3��";
                            _p3.text = "4��";
                            _p4.text = "1��";
                            break;
                    }

                    wk = 0;
                    break;

                //3��
                case 2:
                    _p1.text = "3��";

                    switch (b)
                    {
                        case 1:
                            _p2.text = "1��";
                            _p3.text = "2��";
                            _p4.text = "4��";
                            break;

                        case 2:
                            _p2.text = "1��";
                            _p3.text = "4��";
                            _p4.text = "2��";
                            break;

                        case 3:
                            _p2.text = "4��";
                            _p3.text = "1��";
                            _p4.text = "2��";
                            break;

                        case 4:
                            _p2.text = "2��";
                            _p3.text = "4��";
                            _p4.text = "1��";
                            break;
                    }

                    wk = 0;
                    break;

                //4��
                case 3:
                    _p1.text = "4��";

                    switch (b)
                    {
                        case 1:
                            _p2.text = "1��";
                            _p3.text = "2��";
                            _p4.text = "3��";
                            break;

                        case 2:
                            _p2.text = "1��";
                            _p3.text = "3��";
                            _p4.text = "2��";
                            break;

                        case 3:
                            _p2.text = "3��";
                            _p3.text = "1��";
                            _p4.text = "2��";
                            break;

                        case 4:
                            _p2.text = "2��";
                            _p3.text = "3��";
                            _p4.text = "1��";
                            break;
                    }

                    wk = 0;
                    break;

                //1��
                case 4:
                    _p1.text = "1��";

                    switch (b)
                    {
                        case 1:
                            _p2.text = "3��";
                            _p3.text = "2��";
                            _p4.text = "4��";
                            break;

                        case 2:
                            _p2.text = "3��";
                            _p3.text = "4��";
                            _p4.text = "2��";
                            break;

                        case 3:
                            _p2.text = "4��";
                            _p3.text = "3��";
                            _p4.text = "2��";
                            break;

                        case 4:
                            _p2.text = "2��";
                            _p3.text = "4��";
                            _p4.text = "3��";
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

                //1��
                case 1:
                    _p1.text = "1��";
                    switch (b)
                    {
                        case 1:
                            _p2.text = "3��";
                            _p3.text = "2��";
                            _p4.text = "4��";
                            break;

                        case 2:
                            _p2.text = "3��";
                            _p3.text = "4��";
                            _p4.text = "2��";
                            break;

                        case 3:
                            _p2.text = "4��";
                            _p3.text = "3��";
                            _p4.text = "2��";
                            break;

                        case 4:
                            _p2.text = "2��";
                            _p3.text = "4��";
                            _p4.text = "3��";
                            break;
                    }

                    wk = 0;
                    break;

                //3��
                case 2:
                    _p1.text = "3��";

                    switch (b)
                    {
                        case 1:
                            _p2.text = "1��";
                            _p3.text = "2��";
                            _p4.text = "4��";
                            break;

                        case 2:
                            _p2.text = "1��";
                            _p3.text = "4��";
                            _p4.text = "2��";
                            break;

                        case 3:
                            _p2.text = "4��";
                            _p3.text = "1��";
                            _p4.text = "2��";
                            break;

                        case 4:
                            _p2.text = "2��";
                            _p3.text = "4��";
                            _p4.text = "1��";
                            break;
                    }

                    wk = 0;
                    break;

                //2��
                case 3:
                    _p1.text = "2��";

                    switch (b)
                    {
                        case 1:
                            _p2.text = "1��";
                            _p3.text = "3��";
                            _p4.text = "4��";
                            break;

                        case 2:
                            _p2.text = "1��";
                            _p3.text = "4��";
                            _p4.text = "3��";
                            break;

                        case 3:
                            _p2.text = "4��";
                            _p3.text = "1��";
                            _p4.text = "3��";
                            break;

                        case 4:
                            _p2.text = "3��";
                            _p3.text = "4��";
                            _p4.text = "1��";
                            break;
                    }

                    wk = 0;
                    break;

                //4��
                case 4:
                    _p1.text = "4��";

                    switch (b)
                    {
                        case 1:
                            _p2.text = "1��";
                            _p3.text = "2��";
                            _p4.text = "3��";
                            break;

                        case 2:
                            _p2.text = "1��";
                            _p3.text = "3��";
                            _p4.text = "2��";
                            break;

                        case 3:
                            _p2.text = "3��";
                            _p3.text = "1��";
                            _p4.text = "2��";
                            break;

                        case 4:
                            _p2.text = "2��";
                            _p3.text = "3��";
                            _p4.text = "1��";
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

                //2��
                case 1:
                    _p1.text = "2��";

                    switch (b)
                    {
                        case 1:
                            _p2.text = "1��";
                            _p3.text = "3��";
                            _p4.text = "4��";
                            break;

                        case 2:
                            _p2.text = "1��";
                            _p3.text = "4��";
                            _p4.text = "3��";
                            break;

                        case 3:
                            _p2.text = "4��";
                            _p3.text = "1��";
                            _p4.text = "3��";
                            break;

                        case 4:
                            _p2.text = "3��";
                            _p3.text = "4��";
                            _p4.text = "1��";
                            break;
                    }

                    wk = 0;
                    break;

                //2��
                case 2:
                    _p1.text = "2��";

                    switch (b)
                    {
                        case 1:
                            _p2.text = "1��";
                            _p3.text = "3��";
                            _p4.text = "4��";
                            break;

                        case 2:
                            _p2.text = "1��";
                            _p3.text = "4��";
                            _p4.text = "3��";
                            break;

                        case 3:
                            _p2.text = "4��";
                            _p3.text = "1��";
                            _p4.text = "3��";
                            break;

                        case 4:
                            _p2.text = "3��";
                            _p3.text = "4��";
                            _p4.text = "1��";
                            break;
                    }

                    wk = 0;
                    break;

                //4��
                case 3:
                    _p1.text = "4��";

                    switch (b)
                    {
                        case 1:
                            _p2.text = "1��";
                            _p3.text = "2��";
                            _p4.text = "3��";
                            break;

                        case 2:
                            _p2.text = "1��";
                            _p3.text = "3��";
                            _p4.text = "2��";
                            break;

                        case 3:
                            _p2.text = "3��";
                            _p3.text = "1��";
                            _p4.text = "2��";
                            break;

                        case 4:
                            _p2.text = "3��";
                            _p3.text = "4��";
                            _p4.text = "1��";
                            break;
                    }

                    wk = 0;
                    break;

                //3��
                case 4:
                    _p1.text = "3��";

                    switch (b)
                    {
                        case 1:
                            _p2.text = "1��";
                            _p3.text = "2��";
                            _p4.text = "4��";
                            break;

                        case 2:
                            _p2.text = "1��";
                            _p3.text = "4��";
                            _p4.text = "2��";
                            break;

                        case 3:
                            _p2.text = "4��";
                            _p3.text = "1��";
                            _p4.text = "2��";
                            break;

                        case 4:
                            _p2.text = "2��";
                            _p3.text = "4��";
                            _p4.text = "1��";
                            break;
                    }

                    wk = 0;
                    break;
            }
        }

        _state = GameState.RESULT;

        //�f�o�b�O�p
        //if (Input.GetKeyDown(KeyCode.Return))
        //{
        //    _state = GameState.TUTORIAL;
        //}
    }

    private void ResltState()
    {
        _Text.text = "���U���g\nEnter�ŃQ�[����";

        if (Input.GetKeyDown(KeyCode.Return))
        {
            _state = GameState.END;
        }  
    }

    private void EndState()
    {
        _miniGameConnection.EndMiniGame();
    }


    //���ʕt��
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
