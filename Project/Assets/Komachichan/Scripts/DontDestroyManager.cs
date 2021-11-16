using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class DontDestroyManager : MonoBehaviour
{
    private static DontDestroyManager instance = null;

    public class CharacterInfo
    {
        public CharacterBase _character;
        public string _characterName;
        public int _rank;
        public int _lapCount;
        public int _souvenirNum;
        public int _money;
        public int[] _useEventNumByType;
        public List<int> _addMoneyByTurn;
    }

    private List<CharacterInfo> _characters;

    // Start is called before the first frame update
    void Start()
    {
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
    }

    public void Init(List<CharacterControllerBase> characters)
    {
        _characters = new List<CharacterInfo>();
        
        foreach(var x in characters)
        {
            _characters.Add(new CharacterInfo());
            var charaInfo = _characters.Last();
            charaInfo._character = x.Character;
            charaInfo._useEventNumByType = new int[(int)SquareEventType.EVENT_TYPE_MAX];
            charaInfo._addMoneyByTurn = new List<int>();
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

        }

    }

    public void SetAddMoenyByTurn(CharacterBase character)
    {

    }

    public int GetRank(CharacterBase character)
    {
        return _characters.Find(x => x._character == character)._rank;
    }

    public int GetUseEventNum(CharacterBase character)
    {
        return _characters.Find(x => x._character == character)._useEventNumByType.Sum();
    }

    public int GetUseEventNumByType(CharacterBase character, SquareEventType eventType)
    {
        return _characters.Find(x => x._character == character)._useEventNumByType[(int)eventType];
    }

    public int GetLapCount(CharacterBase character)
    {
        return _characters.Find(x => x._character == character)._lapCount;
    }

    public int GetSouvenirNum(CharacterBase character)
    {
        return _characters.Find(x => x._character == character)._souvenirNum;
    }
    
    public int GetMoney(CharacterBase character, int money)
    {
        return _characters.Find(x => x._character == character)._money;
    }
}
