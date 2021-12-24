using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using DG.Tweening;

public class CakeGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] _missionCakePrefabs;
    [SerializeField] private GameObject[] _cakePrefabs;
    [SerializeField] private Transform _missionCakeCanvas;
    [SerializeField] private Transform _missionCakeTrans;
    [SerializeField] private Transform _gameMissionCakeTrans;
    [SerializeField] private int cakeNum;
    [SerializeField] private Text _missionText;
    [SerializeField] private Text _answerText;
    [SerializeField] private GameObject _missionObj;
    [SerializeField] private GameObject _gameUiObj;
    [SerializeField] private GameObject[] _countObj;
    [SerializeField] CountController[] _countController;
    [SerializeField] private MiniGameConnection _miniGameCorrection;
    [SerializeField] private MiniGameResult _miniGameResult;
    [SerializeField] private GameObject[] _controlUI;//操作ボタン表示UI
    [SerializeField] private SandbyManager _standby;

    private int _nowCake = 0;
    private bool _isStart = false;
    private int _questCakeType;//今回数えるケーキ
    private GameObject _objCake;
    private int _numQuestCake;//今回数えるケーキの数
    private Dictionary<int,int> _Answers = new Dictionary<int,int>(); //プレイヤーの答え
    private bool[] _isCntFin;//数え終わったか？？

    private bool _isGameResultTrigger = false;//リザルトを出すか？

    bool _isJoJo;
    void Start()
    {
        _miniGameCorrection = MiniGameConnection.Instance;

        _isCntFin = new bool[4];
        for(int i = 0; i < _isCntFin.Length;i++)
        {
            _isCntFin[i] = false; 
        }

        _questCakeType = UnityEngine.Random.Range(0, 4);
        _objCake = new GameObject();

        if (_questCakeType == 0)
        {
            _missionText.text = "フレジエ";
            _objCake = Instantiate(_missionCakePrefabs[0], _missionCakeCanvas);
        }
        else if (_questCakeType == 1)
        {
            _missionText.text = "ザッハトルテ";
            _objCake = Instantiate(_missionCakePrefabs[1], _missionCakeCanvas);
        }
        else if (_questCakeType == 2)
        {
            _missionText.text = "ショートケーキ";
            _objCake = Instantiate(_missionCakePrefabs[2], _missionCakeCanvas);
        }
        else if (_questCakeType == 3)
        {
            _missionText.text = "アップルパイ";
            _objCake = Instantiate(_missionCakePrefabs[3], _missionCakeCanvas);
        }
        _objCake.transform.position =   _missionCakeTrans.transform.position;
        _objCake.transform.localScale = _missionCakeTrans.transform.localScale;
        _objCake.transform.rotation = _missionCakeTrans.transform.rotation;

        _numQuestCake = 0;
    }

    void Update()
    {
        //スペースキーが押されたら始まる
        if (!_isStart && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("A") || Input.GetButtonDown("Start")) && !_standby.gameObject.activeSelf)
        {
            Control_SE.Get_Instance().Play_SE("Whistle");

            _isStart = true;
            _missionObj.SetActive(false);
            _gameUiObj.SetActive(true);

            _objCake.transform.position =   _gameMissionCakeTrans.transform.position;
            _objCake.transform.localScale = _gameMissionCakeTrans.transform.localScale;
            _objCake.transform.rotation =   _gameMissionCakeTrans.transform.rotation;

            StartCoroutine(CakeGenerate());
        }
        //終了したらカウントタイムに入る
        if(_isStart && _nowCake == cakeNum)
        {
            DOVirtual.DelayedCall(3, () =>
            {
                for (int i = 0; i < _countController.Length; i++)
                {
                    _countObj[i].SetActive(true);

                    //オートの敵は操作UI表示しない
                    if (_countController[i]._miniGameChara.IsAutomatic)
                    {
                        _controlUI[i].SetActive(false);
                    }

                    _countController[i].isCountTime = true;
                }
            });
        }

        //みんなが数え終わったらリザルトを表示
        if(_isGameResultTrigger)
        { 
            foreach(var x in _countController)
            {
                x.ShowSelectCount();
            }

            _countObj[4].SetActive(true);
            _answerText.text = Convert.ToString(_numQuestCake);
            _isGameResultTrigger = false;//呼ぶのは１回でいいので元に戻す

            DOVirtual.DelayedCall(3, () =>
            {
                CheckAnswer();
            });
            _isJoJo = true;
        }

        if (_isJoJo)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("A") || Input.GetButtonDown("Start"))
            {
                Control_SE.Get_Instance().Play_SE("UI_Correct");
                _miniGameCorrection.EndMiniGame();
            }
        }
    }

    void CreateCake()
    {
        var randPosZ = UnityEngine.Random.Range(-3, -9);
        var randCake = UnityEngine.Random.Range(0, 4);

        //範囲内にケーキが無ければ生成
        GameObject cake = Instantiate(_cakePrefabs[randCake], new Vector3(10.0f, 0.4f, randPosZ), Quaternion.identity);
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

    //答え合わせ
    private void CheckAnswer()
    {
        _gameUiObj.SetActive(false);
        _objCake.SetActive(false);

        //Valueの差が少ない順にソートする
        _Answers = _Answers.OrderBy(v => v.Value).ToDictionary(key => key.Key, val => val.Value);//逆の場合はOrderByDescending
        Dictionary<MiniGameCharacter, int> rank = new Dictionary<MiniGameCharacter, int>();

        int ranking = 1;
        /*
        int cnt = 0;
        //_Answers.KeysにIDが入っている。
        foreach (var ID in _Answers.Keys)
        {
            if(cnt > 0 && ID > 0 && (_Answers[ID] != _Answers[ID - 1])) ranking++;
            rank.Add(_countController[ID]._miniGameChara, ranking);
            cnt++;
        }*/

        int currentAns = int.MaxValue;
        int nextRank = 1;
        foreach (var ID in _Answers.Keys)
        {
            // 同列でない
            if (currentAns < _Answers[ID])
            {
                ranking += nextRank;
                nextRank = 1;
            }
            
            // 同列
            if (currentAns == _Answers[ID])
            {
                nextRank++;
            }
            
            rank.Add(_countController[ID]._miniGameChara, ranking);
            currentAns = _Answers[ID];
        }
        
        _miniGameResult.Display(rank);
    }

    //プレイヤーや敵側で数え終わったら呼ぶ
    public void SetCntFin(int _myId, int _ans)
    {
        _Answers[_myId] = (_ans - _numQuestCake) * (_ans - _numQuestCake);//絶対値を入れる
        _isCntFin[_myId] = true;

        var fin = 0;
        for(int i = 0;i < _isCntFin.Length;i++)
        {
            if (_isCntFin[i] == true) fin += 1; 
        }
        if (fin == 4) _isGameResultTrigger = true;
    }

    public int GetNumQuestCake()
    {
        return _numQuestCake;
    }
}