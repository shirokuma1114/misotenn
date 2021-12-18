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

    private int _cntCake;//カウント数

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
            //操作ができるプレイヤー
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
                //スペースキー押したらCardの関数呼ぶ
                if (Input.GetKeyDown(KeyCode.X))
                {
                    //ケーキのカウント
                    _cakeGenerator.SetAnswer(_id, _cntCake);
                    isCountFin = true;//カウント終了
                }
            }
            //操作ができるエネミー１
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
                //スペースキー押したらCardの関数呼ぶ
                if (Input.GetKeyDown(KeyCode.V))
                {
                    //ケーキのカウント
                    _cakeGenerator.SetAnswer(_id, _cntCake);
                    isCountFin = true;//カウント終了
                }
            }
            //操作ができるエネミー2
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
                //スペースキー押したらCardの関数呼ぶ
                if (Input.GetKeyDown(KeyCode.N))
                {
                    //ケーキのカウント
                    _cakeGenerator.SetAnswer(_id, _cntCake);
                    isCountFin = true;//カウント終了
                }
            }
            //操作ができるエネミー3
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
                //スペースキー押したらCardの関数呼ぶ
                if (Input.GetKeyDown(KeyCode.M))
                {
                    //ケーキのカウント
                    _cakeGenerator.SetAnswer(_id, _cntCake);
                    isCountFin = true;//カウント終了
                }
            }
        }
        else if(isCountTime && !isCountFin && miniGameChara.IsAutomatic)
        {
            int rand = UnityEngine.Random.Range(5, 8);
            _cntText.text = Convert.ToString(rand);
            _cakeGenerator.SetAnswer(_id, _cntCake);
            isCountFin = true;//カウント終了
        }
    }
}