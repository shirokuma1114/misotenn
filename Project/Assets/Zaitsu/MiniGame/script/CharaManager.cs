using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharaManager : MonoBehaviour
{
    private MiniGameConnection _miniGameConnection;

    [SerializeField]
    List<GameObject> _players;

    [SerializeField]
    private List<GameObject> _cake; // ‚±‚¢‚Â‚Ì4”Ô–Ú‚ªˆêˆÊ3”Ô–Ú‚ª“ñˆÊ2”Ô–Ú‚ªŽOˆÊ4”Ô–Ú‚ªŽlˆÊ
    public List<GameObject> Cake=>_cake;
    [SerializeField]
    public StartCounterGura _counterGura;
    public bool _countFlg;
    public bool _endFlg;

    [SerializeField]
    private SandbyManager _stanby;

    [SerializeField]
    private CubeManager _cube;

    bool _isResult;

    bool _isStandBy;

    [SerializeField]
    private MiniGameResult _miniGameResult;

    void Awake()
    {
        _countFlg = false;
        _endFlg = false;
    }

    void Start()
    {
        _miniGameConnection = MiniGameConnection.Instance;
        
        foreach (var c in _miniGameConnection.Characters)
        {
            foreach(var x in _players)
            {
                if (c.Name == x.name)
                {
                    if (c.IsAutomatic)
                    {
                        var cpu = x.AddComponent<GuraCPUCon>();
                        cpu._cake.AddRange(_players.Where(y => y != x).ToList());
                        cpu._chara = this;
                        cpu._character = c;
                    }
                    else
                    {
                        var pl = x.AddComponent<GuraPlayerCon>();
                        pl._chara = this;
                        pl._character = c;
                    }
                }
            }
        }

        _stanby.gameObject.SetActive(true);
        _isStandBy = true;
    }

    void Update()
    {
        _countFlg = _counterGura.CountFlg;

        if (!_stanby.gameObject.activeSelf && _isStandBy)
        {
            _counterGura.PlayCount();
            _cube.PlayMove();
            _isStandBy = false;
        }

        if (_isResult)
        {
            if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Start") || Input.GetButtonDown("A"))
            {
                Control_SE.Get_Instance().Play_SE("UI_Correct");
                _miniGameConnection.EndMiniGame();
            }

        }

        if(_cake.Count>=3 && !_endFlg)
        {
            // I—¹ˆ—
            _endFlg=true;

            foreach(var x in _players)
            {
                if(_cake.Where(y => y == x).Count() == 0)
                {
                    AddCake(x);
                    break;
                }
            }
            _cake.Reverse();

            var rank = new Dictionary<MiniGameCharacter, int>();
            for(int i = 0; i < 4; i++)
            {
                if (_cake[i].GetComponent<GuraCPUCon>())
                {
                    rank.Add(_cake[i].GetComponent<GuraCPUCon>()._character, i + 1);
                }
                if (_cake[i].GetComponent<GuraPlayerCon>())
                {
                    rank.Add(_cake[i].GetComponent<GuraPlayerCon>()._character, i + 1);
                }

            }

            _miniGameResult.Display(rank);

            _isResult = true;
        }
    }

    public void AddCake(GameObject cake)
    {
        _cake.Add(cake);
    }
}
