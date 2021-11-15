using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DontDestroyManager : MonoBehaviour
{
    private static DontDestroyManager instance = null;

    public struct CharacterInfo
    {
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

    public void Init(List<CharacterBase> characters)
    {
        _characters.Clear();

        foreach(var x in characters)
        {
            _characters.Add(new CharacterInfo());
            var charaInfo = _characters.Last();
            charaInfo._money = x.Money;
            charaInfo._useEventNumByType = new int[(int)SquareEventType.EVENT_TYPE_MAX];
            charaInfo._addMoneyByTurn = new List<int>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetRank(int index)
    {
        return 0;
    }

    public void SetUseEventNum(int index, int num)
    {

    }

    public int GetUseEventNum(int index)
    {
        return 0;
    }

    public void SetLapCount(int index, int lapCount)
    {
        
    }

    public int GetLaoCount(int index)
    {
        return 0;
    }

    public void SetSouvenirNum(int index, int souvenirNum)
    {

    }

    public int GetSouvenirNum(int index)
    {
        return 0;
    }
    
    public void SetMoeny(int index, int money)
    {

    }

    public int GetMoney(int index, int money)
    {
        return 0;
    }

    public void SetUseEventNumByType(int index)
    {

    }

    public int GetUseEventNumByType(int index)
    {
        return 0;
    }

    public void SetAddMoenyByTurn(int index)
    {

    }



}
