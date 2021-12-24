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

    [SerializeField]
    private List<Transform> _playerTransforms;

    [SerializeField]
    private List<Transform> _uiTransforms;


    void Start()
    {
        _miniGameConnection = MiniGameConnection.Instance;

        var characters = _miniGameConnection.Characters;
        for(int iChara = 0; iChara < characters.Count; iChara++)
        {
            var character = characters[iChara];

            for (int iCar = 0; iCar < _cars.Count; iCar++)
            {
                var car = _cars[iCar];
                if (car.name == character.Name)
                {
                    SouvenirScrambleControllerBase controller;
                    if (character.IsAutomatic)
                    {
                        controller = car.AddComponent<SouvenirScrambleControllerAI>();
                    }
                    else
                    {
                        controller = car.AddComponent<SouvenirScrambleController>();
                    }

                    controller.gameObject.transform.position = _playerTransforms[iChara].position;
                    controller.gameObject.transform.rotation = _playerTransforms[iChara].rotation;
                    _ui[iCar].transform.position = _uiTransforms[iChara].transform.position;
                    controller.Init(character, this, _ui[iChara]);

                    _controllers.Add(controller);
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

            Control_SE.Get_Instance().Play_SE("Count");

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
        {
            Control_SE.Get_Instance().Play_SE("UI_Correct");
            _state = SouvenirScrambleState.END;
        }
    }

    void EndState()
    {
        _miniGameConnection.EndMiniGame();
    }

    private Dictionary<MiniGameCharacter, int> Ranking()
    {
        for(int i = 0; i < _controllers.Count;i++)
        {
            Debug.Log(_controllers.Count);
            Debug.Log(_controllers[i].Character.Name);
        }
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
