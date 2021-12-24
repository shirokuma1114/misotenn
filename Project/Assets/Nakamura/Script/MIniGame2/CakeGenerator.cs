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
    [SerializeField] private GameObject[] _controlUI;//����{�^���\��UI
    [SerializeField] private SandbyManager _standby;

    private int _nowCake = 0;
    private bool _isStart = false;
    private int _questCakeType;//���񐔂���P�[�L
    private GameObject _objCake;
    private int _numQuestCake;//���񐔂���P�[�L�̐�
    private Dictionary<int,int> _Answers = new Dictionary<int,int>(); //�v���C���[�̓���
    private bool[] _isCntFin;//�����I��������H�H

    private bool _isGameResultTrigger = false;//���U���g���o�����H

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
            _missionText.text = "�t���W�G";
            _objCake = Instantiate(_missionCakePrefabs[0], _missionCakeCanvas);
        }
        else if (_questCakeType == 1)
        {
            _missionText.text = "�U�b�n�g���e";
            _objCake = Instantiate(_missionCakePrefabs[1], _missionCakeCanvas);
        }
        else if (_questCakeType == 2)
        {
            _missionText.text = "�V���[�g�P�[�L";
            _objCake = Instantiate(_missionCakePrefabs[2], _missionCakeCanvas);
        }
        else if (_questCakeType == 3)
        {
            _missionText.text = "�A�b�v���p�C";
            _objCake = Instantiate(_missionCakePrefabs[3], _missionCakeCanvas);
        }
        _objCake.transform.position =   _missionCakeTrans.transform.position;
        _objCake.transform.localScale = _missionCakeTrans.transform.localScale;
        _objCake.transform.rotation = _missionCakeTrans.transform.rotation;

        _numQuestCake = 0;
    }

    void Update()
    {
        //�X�y�[�X�L�[�������ꂽ��n�܂�
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
        //�I��������J�E���g�^�C���ɓ���
        if(_isStart && _nowCake == cakeNum)
        {
            DOVirtual.DelayedCall(3, () =>
            {
                for (int i = 0; i < _countController.Length; i++)
                {
                    _countObj[i].SetActive(true);

                    //�I�[�g�̓G�͑���UI�\�����Ȃ�
                    if (_countController[i]._miniGameChara.IsAutomatic)
                    {
                        _controlUI[i].SetActive(false);
                    }

                    _countController[i].isCountTime = true;
                }
            });
        }

        //�݂�Ȃ������I������烊�U���g��\��
        if(_isGameResultTrigger)
        { 
            foreach(var x in _countController)
            {
                x.ShowSelectCount();
            }

            _countObj[4].SetActive(true);
            _answerText.text = Convert.ToString(_numQuestCake);
            _isGameResultTrigger = false;//�ĂԂ̂͂P��ł����̂Ō��ɖ߂�

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

        //�͈͓��ɃP�[�L��������ΐ���
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

            //�I���t���O
            if (_nowCake == cakeNum)
            {
               yield break;
            }
        }
    }

    //�P�[�L�����������Ƃ�`����
    public void DecreaseCake(int _type)
    {
        _nowCake -= 1;
        if (_type == _questCakeType) _numQuestCake -= 1;
    }

    //�������킹
    private void CheckAnswer()
    {
        _gameUiObj.SetActive(false);
        _objCake.SetActive(false);

        //Value�̍������Ȃ����Ƀ\�[�g����
        _Answers = _Answers.OrderBy(v => v.Value).ToDictionary(key => key.Key, val => val.Value);//�t�̏ꍇ��OrderByDescending
        Dictionary<MiniGameCharacter, int> rank = new Dictionary<MiniGameCharacter, int>();

        int ranking = 1;
        /*
        int cnt = 0;
        //_Answers.Keys��ID�������Ă���B
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
            // ����łȂ�
            if (currentAns < _Answers[ID])
            {
                ranking += nextRank;
                nextRank = 1;
            }
            
            // ����
            if (currentAns == _Answers[ID])
            {
                nextRank++;
            }
            
            rank.Add(_countController[ID]._miniGameChara, ranking);
            currentAns = _Answers[ID];
        }
        
        _miniGameResult.Display(rank);
    }

    //�v���C���[��G���Ő����I�������Ă�
    public void SetCntFin(int _myId, int _ans)
    {
        _Answers[_myId] = (_ans - _numQuestCake) * (_ans - _numQuestCake);//��Βl������
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