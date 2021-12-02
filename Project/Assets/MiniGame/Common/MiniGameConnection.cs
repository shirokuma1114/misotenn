using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MiniGameConnection : MonoBehaviour
{
    //�Q�[���V�[����̃A�N�e�B�u�Ȃ��ׂẴI�u�W�F�N�g�ۑ�
    private List<GameObject> _gameSceneActiveObjectTmp = new List<GameObject>();

    //�~�j�Q�[���̃V�[�����
    private Scene _playingMiniGameScene;
    public Scene PlayingMiniGameScene => _playingMiniGameScene;

    //�Q�[���V�[���̃v���C���[�̏�񂪓��������X�g
    private List<MiniGameCharacter> _characters = new List<MiniGameCharacter>();
    public List<MiniGameCharacter> Characters => _characters;


    //�~�j�Q�[���̃V�[�������X�g
    [SerializeField]
    private List<string> _miniGameSceneNames;
    public List<string> MiniGameSceneNames => _miniGameSceneNames;

    //�~�j�Q�[���ɓ��������ɔ�A�N�e�B�u�ɂ��Ȃ����X�g
    [SerializeField]
    private List<GameObject> _disactiveExceptionList;

    [Header("�f�o�b�O���[�h")]
    [Space(10)]
    [SerializeField]
    private bool _debugMode;
    [SerializeField]
    private List<DebugMiniGameCharacter> _debugCharactors;


    //�~�j�Q�[�����V�[�����ōĐ�
    //���V�[����buildSetting��Add���邱��
    public void StartMiniGame(string miniGamesSceneName)
    {
        if (_debugMode)
        {
            Debug.Log("�f�o�b�O���[�h��");
            return;
        }
        else�@if (_playingMiniGameScene.isLoaded)
        {
            Debug.Log("�~�j�Q�[�����Đ��ł�");
            return;
        }

        RideMiniGameCharacter();

        AllObjectsDisactive();

        LoadSceneParameters param = new LoadSceneParameters(LoadSceneMode.Additive);
        _playingMiniGameScene = SceneManager.LoadScene(miniGamesSceneName, param);
        StartCoroutine("MiniGameSceneActivate");
    }

    //�~�j�Q�[����_miniGameSceneNames���烉���_���ōĐ�
    public void StartRandomMiniGame()
    {
        int randomIndex = Random.Range(0, _miniGameSceneNames.Count);
        StartMiniGame(_miniGameSceneNames[randomIndex]);
    }

    //�~�j�Q�[�����I��
    public void EndMiniGame()
    {
        if(_debugMode)
        {
            Debug.Log("�f�o�b�O���[�h��");
            return;
        }
        else if (!_playingMiniGameScene.isLoaded)
        {
            Debug.Log("�~�j�Q�[�����Đ�����Ă��܂���");
            return;
        }

        SceneManager.UnloadSceneAsync(_playingMiniGameScene.buildIndex);

        foreach (var obj in _gameSceneActiveObjectTmp)
        {
            obj.SetActive(true);
        }
        _gameSceneActiveObjectTmp.Clear();
    }

    //======================

    void Start()
    {
        if(_debugMode)
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


    //�V�[���̂��ׂẴI�u�W�F�N�g���A�N�e�B�u�ɂ��Ă����ۑ����Ă���
    private void AllObjectsDisactive()
    {
        var allObjects = FindObjectsOfType<GameObject>();

        foreach(var obj in allObjects)
        {
            if (!obj.activeSelf)
                continue;
            if (_disactiveExceptionList.Contains(obj))
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
        gameSceneCharacters.AddRange(FindObjectsOfType<CharacterBase>());

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
        
        SceneManager.SetActiveScene(_playingMiniGameScene);
    }
}
