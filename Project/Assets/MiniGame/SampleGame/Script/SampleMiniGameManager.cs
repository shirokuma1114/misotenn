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

    [SerializeField]
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


    void Start()
    {        
        int i = 0;
        foreach(var c in _miniGameConnection.Characters)
        {
            _miniGameControllers[i].Init(c,this);
            i++;
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
        _tenukiText.text = "チュートリアル画面\nEnterでプレイ";

        if (Input.GetKeyDown(KeyCode.Return))
            _state = SampleGameState.PLAY_RENDA;
    }

    private void PlayRendaState()
    {
        _tenukiText.text = "スペース連打！\nTime:" + (_rendaTime - _rendaTimeCounter).ToString();

        if (_rendaTimeCounter > _rendaTime)
        {
            //_playersを順位で並び替え
            _miniGameControllers.Sort
                (
                    (a, b) => { return b.RendaCount - a.RendaCount; }
                );

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
            _state = SampleGameState.RESULT;

        _playTimeCounter += Time.deltaTime;
    }

    private void ResltState()
    {
        _tenukiText.text = "リザルト\nEnterでゲームシーンへ戻る";

        if (Input.GetKeyDown(KeyCode.Return))
            _state = SampleGameState.END;
    }

    private void EndState()
    {
        _miniGameConnection.EndMiniGame();
    }
}
