using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SampleMiniGameManager: MonoBehaviour
{
    public enum SampleGameState
    {
        TUTORIAL,
        PLAY_RENDA,
        PLAY,
        RESULT,
        END,
    };
    private SampleGameState _state;
    public SampleGameState State => _state;

    private MiniGameConnection _miniGameConnection;

    [SerializeField]
    private Text _tenukiText;

    [SerializeField]
    private List<SampleGameController> _miniGameControllers;

    [SerializeField]
    private float _rendaTime;
    private float _rendaTimeCounter;

    [SerializeField]
    private float _playTime;
    public float PlayTime => _playTime;
    private float _playTimeCounter;
    public float PlayTimeCounter => _playTimeCounter;

    [SerializeField]
    private MiniGameResult _miniGameResult;

    private void Awake()
    {
        _state = SampleGameState.TUTORIAL;
    }
    void Start()
    {
        _miniGameConnection = MiniGameConnection.Instance;
        
        foreach(var c in _miniGameConnection.Characters)
        {
            foreach(var mc in _miniGameControllers)
            {
                if(mc.CakeName == c.Name)
                    mc.Init(c, this);
            }            
        }

        _rendaTimeCounter = 0;
    }

    void Update()
    {        
        switch(_state)
        {
            case SampleGameState.TUTORIAL:
                TutorialState();
                break;

            case SampleGameState.PLAY_RENDA:
                PlayRendaState();
                break;

            case SampleGameState.PLAY:
                PlayState();
                break;

            case SampleGameState.RESULT:
                ResltState();
                break;

            case SampleGameState.END:
                EndState();
                break;
        }
    }

    private void TutorialState()
    {
        _tenukiText.text = "Enterでスタート";

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Start"))
            _state = SampleGameState.PLAY_RENDA;
    }

    private void PlayRendaState()
    {
        _tenukiText.text = "スペース連打！\nTime:" + (_rendaTime - _rendaTimeCounter).ToString();

        if (_rendaTimeCounter > _rendaTime)
        {
            foreach (var p in _miniGameControllers)
            {
                p.Go();
            }

            _state = SampleGameState.PLAY;
        }

        _rendaTimeCounter += Time.deltaTime;
    }

    private void PlayState()
    {
        _tenukiText.text = "走る！";

        if (_playTimeCounter > _playTime)
        {          
            _miniGameResult.Display(Ranking());

            _state = SampleGameState.RESULT;
        }

        _playTimeCounter += Time.deltaTime;
    }

    private void ResltState()
    {
        _tenukiText.text = "リザルト\nEnterでゲームシーンへ戻る";

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Start") || Input.GetButtonDown("A"))
            _state = SampleGameState.END;
    }

    private void EndState()
    {
        _miniGameConnection.EndMiniGame();
    }


    //順位付け
    private Dictionary<MiniGameCharacter, int> Ranking()
    {
        Dictionary<MiniGameCharacter, int> dispCharacters = new Dictionary<MiniGameCharacter, int>();
        List<SampleGameController> workCharacters = new List<SampleGameController>();
        workCharacters.AddRange(_miniGameControllers);

        List<SampleGameController> sameRanks = new List<SampleGameController>();
        for(int rank = 1; rank <= 4;)
        {
            int maxRenda = -1;
            foreach (var chara in workCharacters)
            {
                if(chara.RendaCount > maxRenda)
                {
                    sameRanks.Clear();

                    sameRanks.Add(chara);
                    maxRenda = chara.RendaCount;
                }
                else if(chara.RendaCount == maxRenda)
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
