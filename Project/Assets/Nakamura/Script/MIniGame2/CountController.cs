using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CountController : MonoBehaviour
{
    public bool isCountTime { set; get; }
    public bool isCountFin { set; get; }
    public MiniGameCharacter miniGameChara;
    [SerializeField] Text _cntText;
    [SerializeField] int _id;
    [SerializeField] bool _isPlayer;
    [SerializeField] private MiniGameConnection _miniGameConnection;
    [SerializeField] private CakeGenerator _cakeGenerator;

    private int _cntCake;//�J�E���g��

    void Start()
    {
        miniGameChara = _miniGameConnection.Characters[_id];

        isCountTime = false;
        isCountFin = false;
        _cntCake = 0;
        if (_isPlayer) _cntText.text = Convert.ToString(_cntCake);
        else _cntText.text = "?";
    }

    void Update()
    {
        if (isCountTime && !isCountFin && !miniGameChara.IsAutomatic)
        {
            //���삪�ł���v���C���[
            if (_id == 0)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    _cntCake += 1;
                    _cntText.text = Convert.ToString(_cntCake);
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    if (_cntCake > 0) _cntCake -= 1;
                    _cntText.text = Convert.ToString(_cntCake);
                }
                //�X�y�[�X�L�[��������Card�̊֐��Ă�
                if (Input.GetKeyDown(KeyCode.X))
                {
                    //�P�[�L�̃J�E���g
                    _cakeGenerator.SetAnswer(_id, _cntCake);
                    isCountFin = true;//�J�E���g�I��
                }
            }
            //���삪�ł���G�l�~�[�P
            if (_id == 1)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    _cntCake += 1;
                    _cntText.text = Convert.ToString(_cntCake);
                }
                if (Input.GetKeyDown(KeyCode.F))
                {
                    if (_cntCake > 0) _cntCake -= 1;
                    _cntText.text = Convert.ToString(_cntCake);
                }
                //�X�y�[�X�L�[��������Card�̊֐��Ă�
                if (Input.GetKeyDown(KeyCode.V))
                {
                    //�P�[�L�̃J�E���g
                    _cakeGenerator.SetAnswer(_id, _cntCake);
                    isCountFin = true;//�J�E���g�I��
                }
            }
            //���삪�ł���G�l�~�[2
            if (_id == 2)
            {
                if (Input.GetKeyDown(KeyCode.Y))
                {
                    _cntCake += 1;
                    _cntText.text = Convert.ToString(_cntCake);
                }
                if (Input.GetKeyDown(KeyCode.H))
                {
                    if (_cntCake > 0) _cntCake -= 1;
                    _cntText.text = Convert.ToString(_cntCake);
                }
                //�X�y�[�X�L�[��������Card�̊֐��Ă�
                if (Input.GetKeyDown(KeyCode.N))
                {
                    //�P�[�L�̃J�E���g
                    _cakeGenerator.SetAnswer(_id, _cntCake);
                    isCountFin = true;//�J�E���g�I��
                }
            }
            //���삪�ł���G�l�~�[3
            if (_id == 3)
            {
                if (Input.GetKeyDown(KeyCode.I))
                {
                    _cntCake += 1;
                    _cntText.text = Convert.ToString(_cntCake);
                }
                if (Input.GetKeyDown(KeyCode.K))
                {
                    if (_cntCake > 0) _cntCake -= 1;
                    _cntText.text = Convert.ToString(_cntCake);
                }
                //�X�y�[�X�L�[��������Card�̊֐��Ă�
                if (Input.GetKeyDown(KeyCode.M))
                {
                    //�P�[�L�̃J�E���g
                    _cakeGenerator.SetAnswer(_id, _cntCake);
                    isCountFin = true;//�J�E���g�I��
                }
            }
        }
        else if(isCountTime && !isCountFin && miniGameChara.IsAutomatic)
        {
            int rand = UnityEngine.Random.Range(5, 8);
            _cntText.text = Convert.ToString(rand);
            _cakeGenerator.SetAnswer(_id, _cntCake);
            isCountFin = true;//�J�E���g�I��
        }
    }
}