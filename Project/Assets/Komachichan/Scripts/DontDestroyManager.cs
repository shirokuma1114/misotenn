using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class DontDestroyManager : MonoBehaviour
{
    private static DontDestroyManager instance = null;

    private List<CharacterData> _characters;

    private float _textSpeed;

    public float TextSpeed => _textSpeed;

    // Start is called before the first frame update
    void Awake()
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
}
