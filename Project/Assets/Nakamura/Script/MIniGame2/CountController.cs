using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CountController : MonoBehaviour
{
    public bool isCountTime { set; get; }
    public bool isCountFin { set; get; }
    [SerializeField] Text _cntText;
    [SerializeField] bool _isPlayer;

    private int _cntCake;

    void Start()
    {
        isCountTime = false;
        isCountFin = false;
        _cntCake = 0;
        if (_isPlayer) _cntText.text = Convert.ToString(_cntCake);
        else _cntText.text = "?";
    }

    void Update()
    {
        if (isCountTime && _isPlayer)
        {
            //���삪�ł���
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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isCountFin = false;//�J�E���g�I��
            }
        }
    }
    //���ʂ𑗂�
}