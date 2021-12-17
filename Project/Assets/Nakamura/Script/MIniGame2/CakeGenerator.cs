using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;


public class CakeGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] cakes;
    [SerializeField] private int cakeNum;
    [SerializeField] private Text _missionText;
    [SerializeField] private Text _answerText;
    [SerializeField] private GameObject _missionObj;
    [SerializeField] private GameObject[] _countObj;
    [SerializeField] CountController[] _countController;
    [SerializeField] private MiniGameConnection _miniGameCorrection;
    [SerializeField] private MiniGameResult _miniGameResult;

    private int _nowCake = 0;
    private bool _isStart = false;
    private int _questCakeType;//今回数えるケーキ
    private int _numQuestCake;//今回数えるケーキの数
    private Dictionary<int,int> _Answers = new Dictionary<int,int>(); //プレイヤーの答え

    void Start()
    {

        _questCakeType = UnityEngine.Random.Range(0, 4);
        
        if(_questCakeType == 0)
        {
            _missionText.text = "フレジエ";
        }
        else if (_questCakeType == 1)
        {
            _missionText.text = "ガトーショコラ";
        }
        else if (_questCakeType == 2)
        {
            _missionText.text = "ショートケーキ";
        }
        else if (_questCakeType == 3)
        {
            _missionText.text = "アップルパイ";
        }

        _numQuestCake = 0;
    }

    void Update()
    {
        //スペースキーが押されたら始まる
        if (!_isStart && Input.GetKeyDown(KeyCode.Space))
        {
            _isStart = true;
            _missionObj.SetActive(false);

            CheckAnswer();//debug

            StartCoroutine(CakeGenerate());
        }
        //終了したらカウントタイムに入る
        if(_isStart && _nowCake == cakeNum)
        {
            for(int i = 0;i < _countController.Length;i++)
            {
                _countObj[i].SetActive(true);
                _countController[i].isCountTime = true;
            }
        }
    }

    void CreateCake()
    {
        var randPosZ = UnityEngine.Random.Range(-3, -9);
        var randCake = UnityEngine.Random.Range(0, 4);

        //範囲内にケーキが無ければ生成
        GameObject cake = Instantiate(cakes[randCake], new Vector3(10.0f, 0.4f, randPosZ), Quaternion.identity);
        CakeController cakeCs = cake.GetComponent<CakeController>();
        cakeCs.MyType = randCake;

        if (randCake == _questCakeType) _numQuestCake += 1;
        _nowCake += 1;
    }

    private IEnumerator CakeGenerate()
    {
        while (true) 
        {
            var rand = UnityEngine.Random.Range(0.5f, 1.2f);

            yield return new WaitForSeconds(rand);

            CreateCake();

            //終了フラグ
            if (_nowCake == cakeNum)
            {
               yield break;
            }
        }
    }

    //ケーキが減ったことを伝える
    public void DecreaseCake(int _type)
    {
        _nowCake -= 1;
        if (_type == _questCakeType) _numQuestCake -= 1;
    }

    public void SetAnswer(int _myID,int cnt)
    {
        _Answers[_myID] = cnt;
    }

    //答え合わせ
    private void CheckAnswer()
    {
        //でばっぐ
        _numQuestCake = 2;
        // Add(ID,数えた数)
        _Answers.Add(0,50-_numQuestCake);//key = ID,Value = 正解との差
        _Answers.Add(1,3-_numQuestCake);
        _Answers.Add(2,15-_numQuestCake);
        _Answers.Add(3,700-_numQuestCake);

        //Valueの差が少ない順にソートする
        _Answers = _Answers.OrderBy(v => v.Value).ToDictionary(key => key.Key, val => val.Value);//逆の場合はOrderByDescending

        Dictionary<MiniGameCharacter, int> rank = new Dictionary<MiniGameCharacter, int>();
        
        int ranking = 1;

        //_Answers.KeysにIDが入っている。
        foreach (var ID in _Answers.Keys)
        {
            Debug.Log("キーは" + ID + "です。");
            Debug.Log(_countController[ID].miniGameChara);
            Debug.Log(ranking);
            rank.Add(_countController[ID].miniGameChara, ranking);
            ranking++;
        }
        _miniGameResult.gameObject.SetActive(true);
        _miniGameResult.Display(rank);
        ////ケーキ数表示
        //_countObj[4].SetActive(true);
        //_answerText.text = Convert.ToString(_numQuestCake);
    }
}