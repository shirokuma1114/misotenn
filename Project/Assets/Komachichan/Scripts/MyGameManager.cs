using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

public class MyGameManager : MonoBehaviour
{
    enum Phase
    {
        NONE,
        AWAKE,
        INIT,
        WAIT_TURN_END,
        FADE_OUT,
        MOVE_CAMERA,
        MINI_GAME,
        CLEAR,
        NEXT_SCENE
    }

    struct CharacterInfo
    {
        CharacterBase character;
        int type;
    }
    
    List<CharacterType> _createCharacterTypes;

    [Header("移動カード最小値")]
    [SerializeField]
    private int _cardMinValue;

    [Header("移動カード最大値")]
    [SerializeField]
    private int _cardMaxValue;

    [Header("移動カード初期手札枚数")]
    [SerializeField]
    private int _initCardNum;

    [Header("初期所持金")]
    [SerializeField]
    private int _initMoney;

    [Header("登場キャラクターリスト")]
    [SerializeField]
    private List<CharacterControllerBase> _entryPlugs;

    [SerializeField]
    EarthMove _camera;

    private Phase _phase;

    private int _turnIndex = 0;
    
    [SerializeField]
    SimpleFade _fade;

    [SerializeField]
    StatusWindow _statusWindow;

    [SerializeField]
    MessageWindow _messageWindow;

    [SerializeField]
    SelectWindow _selectWindow;

    private int _turnCount = 0;

    [SerializeField]
    CameraInterpolation _cameraInterpole;

    [SerializeField]
    EarthFreeRotation _earthFreeRotation;

    [Header("勝利に必要なお土産の数")]
    [SerializeField]
    private int _needSouvenirType;


    [Header("相手の初期カードいじれるモード")]
    [SerializeField]
    bool _isFixedMode;

    [SerializeField]
    List<int> _cardValues = new List<int>();

    [Header("大量のお土産を抱えてスタート")]
    [SerializeField]
    bool _isManyManySouvenirs;

    [SerializeField]
    bool _isMiniGameDebug;

    [SerializeField]
    bool _isMiniGameSkip;
    
    [SerializeField]
    Animator _fadeAnimation;

    [SerializeField]
    GameObject _startSquare;

    [SerializeField]
    List<GameObject> _cakePrefabs;

    [SerializeField]
    MiniGameConnection _miniGameConnection;

    [SerializeField]
    MiniGameRandomManager _miniManager;

    [SerializeField]
    GameObject _fireWork;
    [SerializeField]
    GameObject _cloud;

    // Start is called before the first frame update
    void Start()
    {
        _fadeAnimation.Play("FadeIn");
        //_fadeAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime;
        CreateCharacters();

        UpdateTurn();

        // お小遣い移動カード初期化
        InitStatus();

        // ログ初期化
        FindObjectOfType<DontDestroyManager>().Init(_entryPlugs);

        _camera.MoveToPosition(_entryPlugs[_turnIndex].Character.CurrentSquare.GetPosition(), 500);



        _phase = Phase.AWAKE;

        // 初回ターン
  

        foreach(var x in FindObjectsOfType<SquareBase>())
        {
            x.SetInOut();
        }

        _fireWork.SetActive(false);
    }

    void CreateCharacters()
    {
        var characterTypes = FindObjectOfType<DontDestroyManager>().GetCharacterTypes();

        foreach(var x in characterTypes)
        {
            GameObject obj = null;
            if(x == CharacterType.PLAYER1)
            {
                obj = Instantiate(_cakePrefabs[0], _startSquare.transform);
                //obj.AddComponent<CharacterBase>();
                var co = obj.AddComponent<PlayerController>();
                var ch = obj.AddComponent<CharacterBase>();
                co.SetCharacter(ch);
                ch.Name = "フレジエ";
                ch.SetInputController(new KomachiInput(0));
                
            }
            if(x == CharacterType.PLAYER2)
            {
                obj = Instantiate(_cakePrefabs[1], _startSquare.transform);
                var co = obj.AddComponent<PlayerController>();
                var ch = obj.AddComponent<CharacterBase>();
                co.SetCharacter(ch);
                ch.Name = "ザッハトルテ";
                ch.SetInputController(new KomachiInput(1));
                
            }
            if(x == CharacterType.PLAYER3)
            {
                obj = Instantiate(_cakePrefabs[2], _startSquare.transform);
                var co = obj.AddComponent<PlayerController>();
                var ch = obj.AddComponent<CharacterBase>();
                co.SetCharacter(ch);
                ch.Name = "ショートケーキ";
                ch.SetInputController(new KomachiInput(2));
                
            }
            if(x == CharacterType.PLAYER4)
            {
                obj = Instantiate(_cakePrefabs[3], _startSquare.transform);
                var co = obj.AddComponent<PlayerController>();
                var ch = obj.AddComponent<CharacterBase>();
                co.SetCharacter(ch);
                ch.Name = "アップルパイ";
                ch.SetInputController(new KomachiInput(3));
                
            }
            if (x == CharacterType.COM1)
            {
                obj = Instantiate(_cakePrefabs[0], _startSquare.transform);
                var co = obj.AddComponent<AIController>();
                var ch = obj.AddComponent<CharacterBase>();
                ch.Name = "フレジエ";
                co.SetCharacter(ch);

            }
            if (x == CharacterType.COM2)
            {
                obj = Instantiate(_cakePrefabs[1], _startSquare.transform);
                var co = obj.AddComponent<AIController>();
                var ch = obj.AddComponent<CharacterBase>();
                ch.Name = "ザッハトルテ";
                co.SetCharacter(ch);

            }
            if(x == CharacterType.COM3)
            {
                obj = Instantiate(_cakePrefabs[2], _startSquare.transform);
                var co = obj.AddComponent<AIController>();
                var ch = obj.AddComponent<CharacterBase>();
                co.SetCharacter(ch);
                ch.Name = "ショートケーキ";
            }
            if(x == CharacterType.COM4)
            {
                obj = Instantiate(_cakePrefabs[3], _startSquare.transform);
                var co = obj.AddComponent<AIController>();
                var ch = obj.AddComponent<CharacterBase>();
                ch.Name = "アップルパイ";
                co.SetCharacter(ch);
            }
            
            var tr = obj.GetComponent<Transform>();
            tr.localPosition = new Vector3(0f, 1.3f, 0f);
            tr.Rotate(0f, -90f, 0f);
            tr.localScale = new Vector3(25f, 25f, 25f);
            _entryPlugs.Add(obj.GetComponent<CharacterControllerBase>());
        }
    }
    
    public void FindInitCharacterTypes(CharacterType[] characterTypes)
    {
        _createCharacterTypes = characterTypes.ToList();
    }

    void UpdateTurn()
    {
        _turnCount++;
        _statusWindow.SetTurn(_turnCount);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(_phase);

        if(_phase == Phase.AWAKE)
        {
            PhaseAwake();
        }
        if(_phase == Phase.INIT)
        {
            PhaseInit();
        }
        if(_phase == Phase.WAIT_TURN_END)
        {
            PhaseWaitTurnEnd();
        }
        if(_phase == Phase.FADE_OUT)
        {
            PhaseFadeOut();
        }
        if(_phase == Phase.MINI_GAME)
        {
            PhaseMiniGame();
        }
        if(_phase == Phase.MOVE_CAMERA)
        {
            PhaseMoveCamera();
        }
        if(_phase == Phase.CLEAR)
        {
            PhaseClear();
        }
        if(_phase == Phase.NEXT_SCENE)
        {
            PhaseNextScene();
        }
    }

    void PhaseAwake()
    {
        if (_camera.State == EarthMove.EarthMoveState.END && true)
        {
            InitTurn();
        }
    }

    void PhaseInit()
    {
        _entryPlugs[_turnIndex].Character.AddMovingCard(GetRandomRange());
        _entryPlugs[_turnIndex].Character.SetWaitEnable(false);
        // 止まっているキャラクターの整列
        foreach (var x in _entryPlugs) x.Character.Alignment();
        _entryPlugs[_turnIndex].InitTurn();
        _phase = Phase.WAIT_TURN_END;

    }

    void PhaseWaitTurnEnd()
    {
        if (_entryPlugs[_turnIndex].Character.State == CharacterState.END)
        {
            // ６種類全て集めた
            if(_entryPlugs[_turnIndex].Character.GetSouvenirTypeNum() >= _needSouvenirType)
            {
                _phase = Phase.CLEAR;
                Camera.main.GetComponent<CameraInterpolation>().Enter_Event();
                _fireWork.SetActive(true); // 花火表示（クリア演出）
                _cloud.SetActive(false);
                _messageWindow.SetMessage(_entryPlugs[_turnIndex].Character.Name + "　は　" +  _needSouvenirType.ToString() + "つの種類のお土産を集めた！\n"
                    + _entryPlugs[_turnIndex].Character.Name + "　の勝利！", _entryPlugs[_turnIndex].Character, true);

                // このターンのおこづかい
                _entryPlugs[_turnIndex].Character.Log.SetMoenyByTurn(_entryPlugs[_turnIndex].Character.Money);

                // ログを設定
                SetCharacterLogToInfo();
                return;
            }

            if (_isMiniGameDebug)
            {
                _phase = Phase.MINI_GAME;
                _miniGameConnection.StartRandomMiniGame();

                //_phase = Phase.MINI_GAME;
                //_miniManager.SetEnable(true);
                //_miniManager.StartMiniGamneRand();
                return;
            }

            if (_turnIndex + 1 >= _entryPlugs.Count)
            {
                if (_isMiniGameSkip)
                {

                    var list = _entryPlugs.OrderBy(a => Guid.NewGuid()).ToList();

                    list[0].Character.AddMoney(5000);
                    list[1].Character.AddMoney(3000);
                    list[2].Character.AddMoney(1000);
                }
                else
                {
                    // ミニゲームモード起動
                    _phase = Phase.MINI_GAME;
                    _miniManager.SetEnable(true);
                    _miniManager.StartMiniGamneRand();
                    return;
                }
            }

            _phase = Phase.FADE_OUT;
            _fade.FadeStart(30, true);

        }
    }

    void PhaseFadeOut()
    {
        if (!_fade.IsFade)
        {
            _phase = Phase.MOVE_CAMERA;

            // 現在のキャラクターを止める
            _entryPlugs[_turnIndex].Character.SetWaitEnable(true);
            _turnIndex++;
            if (_turnIndex >= _entryPlugs.Count)
            {
                _turnIndex = 0;

                // 合計ターン加算
                UpdateTurn();

                foreach (var x in _entryPlugs)
                {
                    x.Character.Log.SetMoenyByTurn(x.Character.Money);
                }
            }

            //次の人の止まっているマス座標
            _camera.MoveToPosition(_entryPlugs[_turnIndex].Character.CurrentSquare.GetPosition(), 500);
        }
    }

    void PhaseMiniGame()
    {
        if (_miniGameConnection.IsMiniGameFinished())
        {
            _miniManager.SetEnable(false);
            _phase = Phase.MOVE_CAMERA;

            // 現在のキャラクターを止める
            _entryPlugs[_turnIndex].Character.SetWaitEnable(true);

            _turnIndex++;
            if (_turnIndex >= _entryPlugs.Count)
            {
                _turnIndex = 0;

                // 合計ターン加算
                UpdateTurn();

                foreach (var x in _entryPlugs)
                {
                    x.Character.Log.SetMoenyByTurn(x.Character.Money);
                }
            }

            //次の人の止まっているマス座標
            _camera.MoveToPosition(_entryPlugs[_turnIndex].Character.CurrentSquare.GetPosition(), 500);
        }
    }

    void PhaseMoveCamera()
    {
        if (_camera.State == EarthMove.EarthMoveState.END)
        {
            _phase = Phase.INIT;
            _fade.FadeStart(30);
            _cameraInterpole.Set_Second(0.01f);
            _cameraInterpole.Set_NextCamera(0);
        }
    }

    void PhaseClear()
    {
        if (!_messageWindow.IsDisplayed)
        {
            // シーン遷移
            _fadeAnimation.Play("FadeOut");
            //_fade.FadeStart(30, true, true, "re_copy");
            _phase = Phase.NEXT_SCENE;
        }
    }

    void PhaseNextScene()
    {
        if(_fadeAnimation.GetCurrentAnimatorClipInfo(0)[0].clip.name == "FadeOut" && _fadeAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            SceneManager.LoadScene("Result");
            _phase = Phase.NONE;
        }
    }

    void InitTurn()
    {

        for(int i = 0; i < _entryPlugs.Count; i++)
        {
            _entryPlugs[i].Character.SetWaitEnable(true);
            if (i == 0) continue;
            _entryPlugs[i].Character.InitAlignment(i - 1);
        }

        _entryPlugs[_turnIndex].Character.SetWaitEnable(false);
        _entryPlugs[_turnIndex].Character.AddMovingCard(GetRandomRange());
        
        if (_isManyManySouvenirs)
        {
            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.OCEANIA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.OCEANIA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.OCEANIA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.OCEANIA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.OCEANIA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.OCEANIA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.OCEANIA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.OCEANIA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.OCEANIA));

            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.NORTH_AMERICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.NORTH_AMERICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.NORTH_AMERICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.NORTH_AMERICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.NORTH_AMERICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.NORTH_AMERICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.NORTH_AMERICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.NORTH_AMERICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.NORTH_AMERICA));
            
            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.SOUTH_AMERICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.SOUTH_AMERICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.SOUTH_AMERICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.SOUTH_AMERICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.SOUTH_AMERICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.SOUTH_AMERICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.SOUTH_AMERICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.SOUTH_AMERICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.SOUTH_AMERICA));
            
            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.EUROPE));
            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.EUROPE));
            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.EUROPE));
            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.EUROPE));
            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.EUROPE));
            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.EUROPE));
            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.EUROPE));
            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.EUROPE));
            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.EUROPE));
            
            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.AFRICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.AFRICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.AFRICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.AFRICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.AFRICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.AFRICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.AFRICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.AFRICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(SouvenirType.AFRICA));
            
        }

        _entryPlugs[_turnIndex].InitTurn();
        _phase = Phase.WAIT_TURN_END;
    }

    void InitStatus()
    {

        for(int i = 0; i < _entryPlugs.Count; i++)
        {
            _entryPlugs[i].SetSelectWindow(_selectWindow);
            _entryPlugs[i].SetCharacter(_entryPlugs[i].GetComponent<CharacterBase>());
            var chara = _entryPlugs[i].Character;
            _entryPlugs[i].GetComponent<Protector>().SetCharacter(chara);
            
            if (_isFixedMode && chara.IsAutomatic)
            {
                EditMovingCards(chara);
            }
            else
            {
                for (int j = 0; j < _initCardNum; j++)
                {
                    chara.AddMovingCard(GetRandomRange());
                }
            }

            chara.SetCurrentSquare(_startSquare.GetComponent<SquareBase>());
            chara.AddMoney(_initMoney);
            chara.LapCount = 0;
        }
        _turnIndex = 0;
    }

    void EditMovingCards(CharacterBase character)
    {
        character.AddMovingCard(_cardValues[0]);
        character.AddMovingCard(_cardValues[1]);
        character.AddMovingCard(_cardValues[2]);
        character.AddMovingCard(_cardValues[3]);
    }

    int GetRandomRange()
    {
        return UnityEngine.Random.Range(_cardMinValue, _cardMaxValue + 1);
    }

    public void Move()
    {
        _entryPlugs[_turnIndex].Move();
    }

    // 0 1 2 3
    public int GetRank(CharacterBase character)
    {
        // 順位はお土産種類＋おこづかい
        var characters = GetRankSortedCharacters();
        Debug.Assert(characters.Count() == 4);

        int rank = 0;

        // 一位は1人だけ
        if (characters.First() == character) return rank;

        rank++;
        if (characters[1] == character) return rank;

        // 2位からの同列を考慮したランキング 起こり得るパターンは
        // 1222 1224 1233 1234
        int addRank = 0;
        if (characters[2].GetSouvenirTypeNum() == characters[1].GetSouvenirTypeNum() && characters[2].Souvenirs.Count == characters[1].Souvenirs.Count &&
            characters[2].Money == characters[1].Money)
        {
            addRank += 2;
        }
        else
        {
            rank++;
        }

        if (characters[2] == character) return rank;

        if (!(characters[3].GetSouvenirTypeNum() == characters[2].GetSouvenirTypeNum() && characters[3].Souvenirs.Count == characters[2].Souvenirs.Count &&
            characters[3].Money == characters[2].Money))
        {
            rank++;
        }


        return rank + addRank;
    }

    private void SetCharacterLogToInfo()
    {
        var info = FindObjectOfType<DontDestroyManager>();
        
        foreach(var x in _entryPlugs)
        {
            info.SetRank(x.Character, GetRank(x.Character));
            //x.Character.SetLogToInfo(info);
        }
        info.SetLogByCharacter();
    }

    public bool HasSouvenirByCharacters(CharacterBase omitCharacter = null)
    {
        foreach (var x in _entryPlugs)
        {
            if (omitCharacter == x) continue;
            if (x.Character.Souvenirs.Count > 0) return true;
        }
        return false;
    }

    public bool IsAllCharacterProtected(CharacterBase omitCharacter = null)
    {
        foreach (var x in _entryPlugs)
        {
            if (omitCharacter == x) continue;
            if (!x.GetComponent<Protector>().IsProtected && x.Character.Souvenirs.Count > 0) return false;
        }
        return true;
    }

    public bool CanClearByCharacter(CharacterBase character)
    {
        // 必要種類全て集めたか
        return character.GetSouvenirTypeNum() >= _needSouvenirType ? true : false;
    }

    public void ChangeClearScene(CharacterBase character)
    {
        if (!CanClearByCharacter(character)) return;

        _phase = Phase.CLEAR;
        _messageWindow.SetMessage(character.Name + "　は全てのお土産を制覇した！\n"
            + character.Name + "の勝利！", character);

        // このターンのおこづかい
        character.Log.SetMoenyByTurn(character.Money);

        // ログを設定
        SetCharacterLogToInfo();
    }

    // リーチのキャラクターが存在するか
    public bool ExistsReach()
    {
        // プレイヤーの中で5種類お土産を持っていて、持ってないお土産マスに止まれる移動カードを持っているか
        foreach(var x in _entryPlugs)
        {
            if (x.Character.GetSouvenirTypeNum() == _needSouvenirType - 1) return true;
        }

        return false;
    }

    public List<CharacterBase> GetCharacters(CharacterBase omitCharacter = null)
    {
        var characters = new List<CharacterBase>();

        foreach (var x in _entryPlugs)
        {
            if (omitCharacter == x) continue;
            characters.Add(x.Character);
        }
        return characters;
        
    }
    
    public List<CharacterBase> GetRankSortedCharacters()
    {
        // 順位決めは お土産の種類数 << お土産の数 << 所持金
        return GetCharacters().OrderByDescending(x => x.GetSouvenirTypeNum()).ThenByDescending(x => x.Souvenirs.Count).ThenByDescending(x => x.Money).ToList();
    }
    // 勝利条件に必要な数
    public int GetNeedSouvenirType()
    {
        return _needSouvenirType;
    }

    public void EnableFreeRotation(bool enable)
    {
        if (enable)
        {
            _earthFreeRotation.TrunOn(_entryPlugs[_turnIndex].Character);
        }
        else
        {
            _earthFreeRotation.TrunOff();
            // 元のカメラ位置に移動
            _camera.MoveToPosition(_entryPlugs[_turnIndex].Character.CurrentSquare.GetPosition(), 5.0f);
        }
    }

}
