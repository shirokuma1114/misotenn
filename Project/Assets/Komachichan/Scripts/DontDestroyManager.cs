using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class DontDestroyManager : MonoBehaviour
{
    private static DontDestroyManager instance = null;

    private CharacterType[] _characterTypes;

    private List<CharacterData> _characters;

    private float _textSpeed;

    public float TextSpeed => _textSpeed;

    public enum DebugMode
    {
        RELEASE,
        DEBUG,
        FULL_AUTO
    }

    [SerializeField]
    private DebugMode _isDebug;



    // Start is called before the first frame update
    void Awake()
    {
        if (_isDebug == DebugMode.DEBUG)
        {
            _characterTypes = new CharacterType[4];
            _characterTypes[0] = CharacterType.PLAYER1;
            _characterTypes[1] = CharacterType.COM2;
            _characterTypes[2] = CharacterType.COM3;
            _characterTypes[3] = CharacterType.COM4;
        }

        if(_isDebug == DebugMode.FULL_AUTO)
        {
            _characterTypes = new CharacterType[4];
            _characterTypes[0] = CharacterType.COM1;
            _characterTypes[1] = CharacterType.COM2;
            _characterTypes[2] = CharacterType.COM3;
            _characterTypes[3] = CharacterType.COM4;
        }

        Debug.Log(Input.GetJoystickNames().Length);

        foreach(var x in Input.GetJoystickNames())
        {
            //Debug.Log(x);
        }

        if (instance == null)
        {
            instance = this;
        }
        if (this != instance)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);

        _textSpeed = MessageWindow.DEFAULT_TEXT_SPEED;
    }

    public void Init(List<CharacterControllerBase> characters)
    {
        _characters = new List<CharacterData>();
        
        foreach(var x in characters)
        {
            _characters.Add(new CharacterData());
            var charaInfo = _characters.Last();
            charaInfo._character = x.Character;
            charaInfo._useEventNumByType = new int[(int)SquareEventType.EVENT_TYPE_MAX];
            charaInfo._moneyByTurn = new List<int>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRank(CharacterBase character, int rank)
    {
        _characters.Find(x => x._character == character)._rank = rank;
    }

    public void SetLogByCharacter()
    {
        // キャラクターから情報を取得する
        foreach(var x in _characters)
        {
            x._money = x._character.Money;
            x._lapCount = x._character.LapCount;
            x._characterName = x._character.Name;
            x._souvenirNum = x._character.Souvenirs.Count;
            Array.Copy(x._character.Log.GetUseEventNum(), x._useEventNumByType, x._useEventNumByType.Length);
            foreach(var y in x._character.Log.GetMoneyByTurn())
            {
                x._moneyByTurn.Add(y);
            }
        }

    }

    public CharacterData[] GetCharacterData()
    {
        return _characters.ToArray();
    }

    public void SetInitCharacterTypes(CharacterType[] characterTypes)
    {
        _characterTypes = characterTypes;
    }

    public List<CharacterType> GetCharacterTypes()
    {
        return _characterTypes.ToList();
    }
}
