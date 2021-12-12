using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CakeGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] cakes;
    [SerializeField] private int cakeNum;
    [SerializeField] private Text _missionText;
    [SerializeField] private GameObject _missionObj;
    [SerializeField] private GameObject[] _countObj;
    [SerializeField] CountController[] _countController;
    private int _nowCake = 0;
    private bool _isStart = false;
    private int _questCake;//今回数えるケーキ
    private int _numQuestCake;//今回数えるケーキの数

    void Start()
    {
        _questCake = Random.Range(0, 4);
        
        if(_questCake == 0)
        {
            _missionText.text = "フレジエ";
        }
        else if (_questCake == 1)
        {
            _missionText.text = "ガトーショコラ";
        }
        else if (_questCake == 2)
        {
            _missionText.text = "ショートケーキ";
        }
        else if (_questCake == 3)
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
        var randPosZ = Random.Range(-3, -9);
        var randCake = Random.Range(0, 4);

        Instantiate(cakes[randCake], new Vector3(10.0f, 0.4f, randPosZ), Quaternion.identity);

        if (randCake == _questCake) _numQuestCake += 1;
        _nowCake += 1;
    }

    private IEnumerator CakeGenerate()
    {
        while (true) 
        {
            var rand = Random.Range(0.5f, 2.0f);

            yield return new WaitForSeconds(rand);

            CreateCake();

            //終了フラグ
            if (_nowCake == cakeNum)
            {
                Debug.Log("終了フラグ");
                yield break;
            }
        }
    }
}