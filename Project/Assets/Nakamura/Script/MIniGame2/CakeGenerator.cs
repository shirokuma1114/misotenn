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
    private int _questCakeType;//���񐔂���P�[�L
    private int _numQuestCake;//���񐔂���P�[�L�̐�
    private Dictionary<int,int> _Answers = new Dictionary<int,int>(); //�v���C���[�̓���

    void Start()
    {

        _questCakeType = UnityEngine.Random.Range(0, 4);
        
        if(_questCakeType == 0)
        {
            _missionText.text = "�t���W�G";
        }
        else if (_questCakeType == 1)
        {
            _missionText.text = "�K�g�[�V���R��";
        }
        else if (_questCakeType == 2)
        {
            _missionText.text = "�V���[�g�P�[�L";
        }
        else if (_questCakeType == 3)
        {
            _missionText.text = "�A�b�v���p�C";
        }

        _numQuestCake = 0;
    }

    void Update()
    {
        //�X�y�[�X�L�[�������ꂽ��n�܂�
        if (!_isStart && Input.GetKeyDown(KeyCode.Space))
        {
            _isStart = true;
            _missionObj.SetActive(false);

            CheckAnswer();//debug

            StartCoroutine(CakeGenerate());
        }
        //�I��������J�E���g�^�C���ɓ���
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

        //�͈͓��ɃP�[�L��������ΐ���
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

    public void SetAnswer(int _myID,int cnt)
    {
        _Answers[_myID] = cnt;
    }

    //�������킹
    private void CheckAnswer()
    {
        //�ł΂���
        _numQuestCake = 2;
        // Add(ID,��������)
        _Answers.Add(0,50-_numQuestCake);//key = ID,Value = �����Ƃ̍�
        _Answers.Add(1,3-_numQuestCake);
        _Answers.Add(2,15-_numQuestCake);
        _Answers.Add(3,700-_numQuestCake);

        //Value�̍������Ȃ����Ƀ\�[�g����
        _Answers = _Answers.OrderBy(v => v.Value).ToDictionary(key => key.Key, val => val.Value);//�t�̏ꍇ��OrderByDescending

        Dictionary<MiniGameCharacter, int> rank = new Dictionary<MiniGameCharacter, int>();
        
        int ranking = 1;

        //_Answers.Keys��ID�������Ă���B
        foreach (var ID in _Answers.Keys)
        {
            Debug.Log("�L�[��" + ID + "�ł��B");
            Debug.Log(_countController[ID].miniGameChara);
            Debug.Log(ranking);
            rank.Add(_countController[ID].miniGameChara, ranking);
            ranking++;
        }
        _miniGameResult.gameObject.SetActive(true);
        _miniGameResult.Display(rank);
        ////�P�[�L���\��
        //_countObj[4].SetActive(true);
        //_answerText.text = Convert.ToString(_numQuestCake);
    }
}