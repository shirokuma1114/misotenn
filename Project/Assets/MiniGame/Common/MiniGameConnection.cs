using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MiniGameConnection : MonoBehaviour
{
    static private MiniGameConnection _instance;
    static public MiniGameConnection Instance => _instance;

    //ゲームシーン上のアクティブなすべてのオブジェクト保存
    private List<GameObject> _gameSceneActiveObjectTmp = new List<GameObject>();

    //ミニゲームのシーン情報
    private Scene _playingMiniGameScene;
    public Scene PlayingMiniGameScene => _playingMiniGameScene;

    //ゲームシーンのプレイヤーの情報が入ったリスト
    private List<MiniGameCharacter> _characters = new List<MiniGameCharacter>();
    public List<MiniGameCharacter> Characters => _characters;


    //ミニゲームのシーン名リスト
    [SerializeField]
    private List<string> _miniGameSceneNames;
    public List<string> MiniGameSceneNames => _miniGameSceneNames;

    //ミニゲームに入った時に非アクティブにしないリスト
    [SerializeField]
    private List<GameObject> _disactiveExceptionList;

    [SerializeField]
    private Animator _fadeAnimation;

    [Header("デバッグモード")]
    [Space(10)]
    [SerializeField]
    private bool _debugMode;
    [SerializeField]
    private List<DebugMiniGameCharacter> _debugCharactors;


    //ミニゲームをシーン名で再生
    //※シーンはbuildSettingでAddすること
    public void StartMiniGame(string miniGamesSceneName)
    {
        if (_debugMode)
        {
            Debug.Log("デバッグモード中");
            return;
        }
        else　if (_playingMiniGameScene.isLoaded)
        {
            Debug.Log("ミニゲームが再生です");
            return;
        }

        RideMiniGameCharacter();

        AllObjectsDisactive();

        _fadeAnimation.Play("FadeOut");

        LoadSceneParameters param = new LoadSceneParameters(LoadSceneMode.Additive);
        _playingMiniGameScene = SceneManager.LoadScene(miniGamesSceneName, param);
        StartCoroutine("MiniGameSceneActivate");
    }

    //ミニゲームを_miniGameSceneNamesからランダムで再生
    public void StartRandomMiniGame()
    {
        int randomIndex = Random.Range(0, _miniGameSceneNames.Count);
        StartMiniGame(_miniGameSceneNames[randomIndex]);
    }

    //ミニゲームを終了
    public void EndMiniGame()
    {
        if(_debugMode)
        {
            Debug.Log("デバッグモード中");
            return;
        }
        else if (!_playingMiniGameScene.isLoaded)
        {
            Debug.Log("ミニゲームが再生されていません");
            return;
        }

        _fadeAnimation.Play("FadeOut");
        Invoke("MiniGameUnload", 1.0f);
    }

    //======================

    private void Awake()
    {
        if (_debugMode)
        {
            List<MiniGameConnection> sameComponent = new List<MiniGameConnection>();
            sameComponent.AddRange(FindObjectsOfType<MiniGameConnection>());

            if (sameComponent.Count > 1)
            {
                Destroy(gameObject);
                return;
            }

            foreach (var debugChara in _debugCharactors)
                _characters.Add(new DebugMiniGameCharacter(debugChara));
        }

        _instance = this;
    }
    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            StartRandomMiniGame();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            EndMiniGame();
        }
    }


    //シーンのすべてのオブジェクトを非アクティブにしてそれを保存しておく
    private void AllObjectsDisactive()
    {
        var allObjects = FindObjectsOfType<GameObject>();

        foreach(var obj in allObjects)
        {
            if (!obj.activeSelf)
                continue;
            if (_disactiveExceptionList.Contains(obj.transform.root.gameObject))
                continue;
            if (obj.name == "[DOTween]")
                continue;

            _gameSceneActiveObjectTmp.Add(obj);
            obj.SetActive(false);
        }
    }

    private void RideMiniGameCharacter()
    {
        if (_debugMode)
            return;


        _characters.Clear();

        var gameSceneCharacters = new List<CharacterBase>();
        //gameSceneCharacters.AddRange(FindObjectsOfType<CharacterBase>());
        gameSceneCharacters.AddRange(FindObjectOfType<MyGameManager>().GetCharacters());

        foreach(var chara in gameSceneCharacters)
        {
            _characters.Add(new MiniGameCharacter(chara));
        }
    }

    private IEnumerator MiniGameSceneActivate()
    {
        while (!_playingMiniGameScene.isLoaded)
        {
            yield return null;
        }
        _fadeAnimation.Play("FadeIn");
        SceneManager.SetActiveScene(_playingMiniGameScene);
    }

    private void MiniGameUnload()
    {
        if (_playingMiniGameScene.IsValid())
        {
            SceneManager.UnloadSceneAsync(_playingMiniGameScene.buildIndex);
        }

        foreach (var obj in _gameSceneActiveObjectTmp)
        {
            obj.SetActive(true);
        }
        _gameSceneActiveObjectTmp.Clear();

        _fadeAnimation.Play("FadeIn");
    }
}
