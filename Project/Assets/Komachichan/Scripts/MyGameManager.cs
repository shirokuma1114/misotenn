using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
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
        CLEAR,
        NEXT_SCENE
    }

    struct CharacterInfo
    {
        CharacterBase character;
        int type;
    }

    [SerializeField]
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
    Animator _fadeAnimation;

    // Start is called before the first frame update
    void Start()
    {
        _fadeAnimation.Play("FadeIn");
        //_fadeAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime;
        
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
    }

    void CreateCharacters()
    {
        for(int i = 0; i < _createCharacterTypes.Count; i++)
        {

        }
    }

    void UpdateTurn()
    {
        _turnCount++;
        _statusWindow.SetTurn(_turnCount);
    }

    // Update is called once per frame
    void Update()
    {
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
        if (_camera.State == EarthMove.EarthMoveState.END)
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
            if(_entryPlugs[_turnIndex].Character.GetSouvenirTypeNum() == _needSouvenirType)
            {
                _phase = Phase.CLEAR;
                _messageWindow.SetMessage(_entryPlugs[_turnIndex].Character.Name + "　は　全てのお土産を制覇した！\n"
                    + _entryPlugs[_turnIndex].Character.Name + "　の勝利！", false);

                // このターンのおこづかい
                _entryPlugs[_turnIndex].Character.Log.SetMoenyByTurn(_entryPlugs[_turnIndex].Character.Money);

                // ログを設定
                SetCharacterLogToInfo();
                return;
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
            
            // このターンのおこづかい
            _entryPlugs[_turnIndex].Character.Log.SetMoenyByTurn(_entryPlugs[_turnIndex].Character.Money);
            
            _turnIndex++;
            if (_turnIndex >= _entryPlugs.Count)
            {
                _turnIndex = 0;

                // 合計ターン加算
                UpdateTurn();
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
            SceneManager.LoadScene("re_copy_copy.unity");
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
        _entryPlugs[_turnIndex].Character.Name = "こまち社長";

        if (_isManyManySouvenirs)
        {
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(1000, "コアラのマーチ", SouvenirType.OCEANIA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(1000, "コアラのマーチ", SouvenirType.OCEANIA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(1000, "コアラのマーチ", SouvenirType.OCEANIA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(1000, "コアラのマーチ", SouvenirType.OCEANIA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(1000, "コアラのマーチ", SouvenirType.OCEANIA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(1000, "コアラのマーチ", SouvenirType.OCEANIA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(1000, "コアラのマーチ", SouvenirType.OCEANIA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(1000, "コアラのマーチ", SouvenirType.OCEANIA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(1000, "コアラのマーチ", SouvenirType.OCEANIA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(1000, "コアラのマーチ", SouvenirType.OCEANIA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(1000, "コアラのマーチ", SouvenirType.OCEANIA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(1000, "コアラのマーチ", SouvenirType.OCEANIA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(1000, "コアラのマーチ", SouvenirType.OCEANIA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(1000, "コアラのマーチ", SouvenirType.OCEANIA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(1000, "コアラのマーチ", SouvenirType.OCEANIA));

            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(2000, "珈琲貴族", SouvenirType.SOUTH_AMERICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(2000, "珈琲貴族", SouvenirType.SOUTH_AMERICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(2000, "珈琲貴族", SouvenirType.SOUTH_AMERICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(2000, "珈琲貴族", SouvenirType.SOUTH_AMERICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(2000, "珈琲貴族", SouvenirType.SOUTH_AMERICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(2000, "珈琲貴族", SouvenirType.SOUTH_AMERICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(2000, "珈琲貴族", SouvenirType.SOUTH_AMERICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(2000, "珈琲貴族", SouvenirType.SOUTH_AMERICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(2000, "珈琲貴族", SouvenirType.SOUTH_AMERICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(2000, "珈琲貴族", SouvenirType.SOUTH_AMERICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(2000, "珈琲貴族", SouvenirType.SOUTH_AMERICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(2000, "珈琲貴族", SouvenirType.SOUTH_AMERICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(2000, "珈琲貴族", SouvenirType.SOUTH_AMERICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(2000, "珈琲貴族", SouvenirType.SOUTH_AMERICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(2000, "珈琲貴族", SouvenirType.SOUTH_AMERICA));

            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(10000, "女神像", SouvenirType.NORTH_AMERICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(10000, "女神像", SouvenirType.NORTH_AMERICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(10000, "女神像", SouvenirType.NORTH_AMERICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(10000, "女神像", SouvenirType.NORTH_AMERICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(10000, "女神像", SouvenirType.NORTH_AMERICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(10000, "女神像", SouvenirType.NORTH_AMERICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(10000, "女神像", SouvenirType.NORTH_AMERICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(10000, "女神像", SouvenirType.NORTH_AMERICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(10000, "女神像", SouvenirType.NORTH_AMERICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(10000, "女神像", SouvenirType.NORTH_AMERICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(10000, "女神像", SouvenirType.NORTH_AMERICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(10000, "女神像", SouvenirType.NORTH_AMERICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(10000, "女神像", SouvenirType.NORTH_AMERICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(10000, "女神像", SouvenirType.NORTH_AMERICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(10000, "女神像", SouvenirType.NORTH_AMERICA));

            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(2000, "オランジーナ", SouvenirType.EUROPE));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(2000, "オランジーナ", SouvenirType.EUROPE));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(2000, "オランジーナ", SouvenirType.EUROPE));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(2000, "オランジーナ", SouvenirType.EUROPE));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(2000, "オランジーナ", SouvenirType.EUROPE));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(2000, "オランジーナ", SouvenirType.EUROPE));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(2000, "オランジーナ", SouvenirType.EUROPE));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(2000, "オランジーナ", SouvenirType.EUROPE));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(2000, "オランジーナ", SouvenirType.EUROPE));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(2000, "オランジーナ", SouvenirType.EUROPE));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(2000, "オランジーナ", SouvenirType.EUROPE));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(2000, "オランジーナ", SouvenirType.EUROPE));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(2000, "オランジーナ", SouvenirType.EUROPE));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(2000, "オランジーナ", SouvenirType.EUROPE));

            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(20000, "メジェド様", SouvenirType.AFRICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(20000, "メジェド様", SouvenirType.AFRICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(20000, "メジェド様", SouvenirType.AFRICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(20000, "メジェド様", SouvenirType.AFRICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(20000, "メジェド様", SouvenirType.AFRICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(20000, "メジェド様", SouvenirType.AFRICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(20000, "メジェド様", SouvenirType.AFRICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(20000, "メジェド様", SouvenirType.AFRICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(20000, "メジェド様", SouvenirType.AFRICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(20000, "メジェド様", SouvenirType.AFRICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(20000, "メジェド様", SouvenirType.AFRICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(20000, "メジェド様", SouvenirType.AFRICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(20000, "メジェド様", SouvenirType.AFRICA));
            _entryPlugs[_turnIndex].Character.AddSouvenir(new Souvenir(20000, "メジェド様", SouvenirType.AFRICA));
        }

        _entryPlugs[_turnIndex].InitTurn();
        _phase = Phase.WAIT_TURN_END;
    }

    void InitStatus()
    {
        var startSquare = GameObject.Find("Japan").GetComponent<SquareBase>();

        for(int i = 0; i < _entryPlugs.Count; i++)
        {
            _entryPlugs[i].SetSelectWindow(_selectWindow);
            _entryPlugs[i].SetCharacter(_entryPlugs[i].GetComponent<CharacterBase>());

            var chara = _entryPlugs[i].Character;
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
            chara.Name = "敵" + i + "号";
            chara.SetCurrentSquare(startSquare);
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
        return Random.Range(_cardMinValue, _cardMaxValue + 1);
    }

    public void Move()
    {
        _entryPlugs[_turnIndex].Move();
    }

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
        if (characters[2].Souvenirs.Count == characters[1].Souvenirs.Count &&
            characters[2].Money == characters[1].Money)
        {
            addRank += 2;
        }
        else
        {
            rank++;
        }

        if (characters[2] == character) return rank;

        if (!(characters[3].Souvenirs.Count == characters[2].Souvenirs.Count &&
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
        return GetCharacters().OrderByDescending(x => x.GetSouvenirTypeNum()).ThenByDescending(x => x.Money).ToList();
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
