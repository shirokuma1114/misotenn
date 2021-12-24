using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SouvenirScrambleManager : MonoBehaviour
{
    public enum SouvenirScrambleState
    {
        TUTORIAL,
        START_COUNT,
        PLAY,
        RESULT,
        END,
    };
    private SouvenirScrambleState _state;
    public SouvenirScrambleState State => _state;

    private MiniGameConnection _miniGameConnection;

    private List<SouvenirScrambleControllerBase> _controllers = new List<SouvenirScrambleControllerBase>();

    [SerializeField]
    private Text _startCounterText;
    private float _startCounter;
    [SerializeField]
    private Image _startCounterBG;

    [SerializeField]
    private float _playTime;
    private float _playTimeCounter;
    public float PlayTimeCounter => _playTimeCounter;

    [SerializeField]
    private List<GameObject> _cars;
    [SerializeField]
    private List<SouvenirScrambleControllerUI> _ui;

    [SerializeField]
    private MiniGameResult _result;

    [SerializeField]
    private GameObject _cloud;

    [SerializeField]
    private ParticleController _speedLine;

    [SerializeField]
    private SandbyManager _standby;


    void Start()
    {
        _miniGameConnection = MiniGameConnection.Instance;

        foreach (var chara in _miniGameConnection.Characters)
        {
            for(int i = 0; i < _cars.Count;i++)
            {
                var car = _cars[i];
                if (car.name == chara.Name)
                {
                    SouvenirScrambleControllerBase sc;
                    if (chara.IsAutomatic)
                    {
                        sc = car.AddComponent<SouvenirScrambleControllerAI>();
                    }
                    else
                    {
                        sc = car.AddComponent<SouvenirScrambleController>();
                    }

                    _controllers.Add(sc);
                    sc.Init(chara, this,_ui[i]);
                }
            }
        }

        _startCounterText.enabled = false;

        _state = SouvenirScrambleState.TUTORIAL;
    }

    void Update()
    {
        switch (_state)
        {
            case SouvenirScrambleState.TUTORIAL:
                TutorialState();
                break;

            case SouvenirScrambleState.START_COUNT:
                StartCountState();
                break;

            case SouvenirScrambleState.PLAY:
                PlayState();
                break;

            case SouvenirScrambleState.RESULT:
                ResltState();
                break;

            case SouvenirScrambleState.END:
                EndState();
                break;
        }
    }


    void TutorialState()
    {
        if (!_standby.gameObject.activeSelf)
        {
            _startCounter = 4;
            _startCounterText.enabled = true;

            _speedLine.SetEmission(10.0f);

            _state = SouvenirScrambleState.START_COUNT;
        }
    }

    void StartCountState()
    {
        _startCounterText.text = Mathf.FloorToInt(_startCounter).ToString();

        if (_startCounter < 1)
        {
            _startCounterText.enabled = false;
            _startCounterBG.enabled = false;

            _state = SouvenirScrambleState.PLAY;
        }

        _startCounter -= Time.deltaTime;
    }

    void PlayState()
    {
        if(_playTimeCounter > _playTime)
        {
            _result.Display(Ranking());

            _cloud.SetActive(false);

            _speedLine.SetEmission(0.0f);

            _state = SouvenirScrambleState.RESULT;
        }

        _playTimeCounter += Time.deltaTime;
    }

    void ResltState()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Start") || Input.GetButtonDown("A"))
            _state = SouvenirScrambleState.END;
    }

    void EndState()
    {
        _miniGameConnection.EndMiniGame();
    }

    private Dictionary<MiniGameCharacter, int> Ranking()
    {
        Dictionary<MiniGameCharacter, int> dispCharacters = new Dictionary<MiniGameCharacter, int>();
        List<SouvenirScrambleControllerBase> workCharacters = new List<SouvenirScrambleControllerBase>();
        workCharacters.AddRange(_controllers);

        List<SouvenirScrambleControllerBase> sameRanks = new List<SouvenirScrambleControllerBase>();
        for (int rank = 1; rank <= 4;)
        {
            int maxRenda = -1;
            foreach (var chara in workCharacters)
            {
                if (chara.Point > maxRenda)
                {
                    sameRanks.Clear();

                    sameRanks.Add(chara);
                    maxRenda = chara.Point;
                }
                else if (chara.Point == maxRenda)
                {
                    sameRanks.Add(chara);
                }
            }

            foreach (var ranker in sameRanks)
            {
                dispCharacters.Add(ranker.Character, rank);
                workCharacters.Remove(ranker);
            }
            rank += sameRanks.Count;
        }

        return dispCharacters;
    }
}
