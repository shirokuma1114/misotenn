using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameResult : MonoBehaviour
{
    private List<KeyValuePair<MiniGameCharacter, int>> _characterRankList = new List<KeyValuePair<MiniGameCharacter, int>>();


    [Space(10)]
    [Header("順位によりもらえるお金 element0が1位")]
    [SerializeField]
    private List<int> _rankMoney;

    [Space(10)]
    [SerializeField]
    private List<GameObject> _cakePrefab;
    private List<GameObject> _cakeInstance = new List<GameObject>();

    [SerializeField]
    private List<GameObject> _cakeTransforms;
    [SerializeField]
    private List<Text> _addMoneyTexts;
    [SerializeField]
    private List<Text> _rankTexts;



    /// <summary>
    /// リザルト表示
    /// </summary>
    /// <param name="characterRankList">キャラクター,順位</param>
    public void Display(Dictionary<MiniGameCharacter, int> characterRankList)
    {
        foreach (var charaRank in characterRankList)
            _characterRankList.Add(new KeyValuePair<MiniGameCharacter, int>(charaRank.Key, charaRank.Value));

        GetComponent<Canvas>().worldCamera = Camera.main;
        gameObject.SetActive(true);

        SetUpCakes();
    }

    //================

    void Start()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        
    }

    private void SetUpCakes()
    {
        _characterRankList.Sort
            (
                (a, b) =>
                {
                    return a.Value - b.Value;
                }
            );

        for(int i = 0;i < _characterRankList.Count;i++)
        {
            foreach (var prefab in _cakePrefab)
            {
                if (prefab.name == _characterRankList[i].Key.Name)
                {
                    _cakeInstance.Add(Instantiate(prefab,transform));
                    break;
                }
            }

            _cakeInstance[i].transform.position = _cakeTransforms[i].transform.position;
            _cakeInstance[i].transform.localScale = _cakeTransforms[i].transform.localScale;
            _cakeInstance[i].transform.rotation = _cakeTransforms[i].transform.rotation;

            _addMoneyTexts[i].text = "+" + _rankMoney[_characterRankList[i].Value - 1].ToString();
            _rankTexts[i].text = _characterRankList[i].Value.ToString() + "位";
        }
    }
}
