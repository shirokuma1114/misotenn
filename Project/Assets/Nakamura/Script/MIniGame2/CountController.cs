using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CountController : MonoBehaviour
{
    public bool isCountTime { set; get; }
    public bool isCountFin { set; get; }
    public MiniGameCharacter _miniGameChara;
    [SerializeField] Text _cntText;
    [SerializeField] Text _nameText;
    [SerializeField] int _id;
    [SerializeField] bool _isPlayer;
    [SerializeField] private MiniGameConnection _miniGameConnection;
    [SerializeField] private CakeGenerator _cakeGenerator;

    private int _cntCake;//カウント数

    float _beforeTrigger;

    int _AISelectCount;

    void Start()
    {
        _miniGameConnection = MiniGameConnection.Instance;

        _miniGameChara = _miniGameConnection.Characters[_id];
        _nameText.text = _miniGameChara.Name;

        isCountTime = false;
        isCountFin = false;
        _cntCake = 0;
        if (_isPlayer) _cntText.text = Convert.ToString(_cntCake);
        else _cntText.text = "0";
        
        if (_miniGameChara.IsAutomatic) _cntText.text = "?";
    }

    void Update()
    {
        if (isCountTime && !isCountFin && !_miniGameChara.IsAutomatic)
        {
            //操作ができるプレイヤー
            if (_miniGameChara.Name == "フレジエ")
            {
                float viewButton = _miniGameChara.Input.GetAxis("Vertical");
                
                if(_beforeTrigger == 0.0f)
                {
                    if(viewButton < 0.0f)
                    {
                        Control_SE.Get_Instance().Play_SE("UI_Select");
                        _cntCake += 1;
                        _cntText.text = Convert.ToString(_cntCake);
                    }
                    if(viewButton > 0.0f)
                    {
                        Control_SE.Get_Instance().Play_SE("UI_Select");
                        if (_cntCake > 0) _cntCake -= 1;
                        _cntText.text = Convert.ToString(_cntCake);
                    }
                }
                _beforeTrigger = viewButton;
                
                if (Input.GetKeyDown(KeyCode.W))
                {
                    Control_SE.Get_Instance().Play_SE("UI_Select");
                    _cntCake += 1;
                    _cntText.text = Convert.ToString(_cntCake);
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    Control_SE.Get_Instance().Play_SE("UI_Select");
                    if (_cntCake > 0) _cntCake -= 1;
                    _cntText.text = Convert.ToString(_cntCake);
                }

                //スペースキー押したらCardの関数呼ぶ
                if (Input.GetKeyDown(KeyCode.X) || _miniGameChara.Input.GetButtonDown("A"))
                {
                    //ケーキのカウント
                    Control_SE.Get_Instance().Play_SE("UI_Correct");
                    _cakeGenerator.SetCntFin(_id, _cntCake);
                    isCountFin = true;//カウント終了
                    _cntText.color = Color.yellow;
                }
            }
            //操作ができるエネミー１
            if (_miniGameChara.Name == "ザッハトルテ")
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    Control_SE.Get_Instance().Play_SE("UI_Select");

                    _cntCake += 1;
                    _cntText.text = Convert.ToString(_cntCake);
                }
                if (Input.GetKeyDown(KeyCode.F))
                {
                    Control_SE.Get_Instance().Play_SE("UI_Select");

                    if (_cntCake > 0) _cntCake -= 1;
                    _cntText.text = Convert.ToString(_cntCake);
                }

                float viewButton = _miniGameChara.Input.GetAxis("Vertical");
                if (_beforeTrigger == 0.0f)
                {
                    if (viewButton < 0.0f)
                    {
                        Control_SE.Get_Instance().Play_SE("UI_Select");

                        _cntCake += 1;
                        _cntText.text = Convert.ToString(_cntCake);
                    }
                    if (viewButton > 0.0f)
                    {
                        Control_SE.Get_Instance().Play_SE("UI_Select");
                        if (_cntCake > 0) _cntCake -= 1;
                        _cntText.text = Convert.ToString(_cntCake);
                    }
                }
                _beforeTrigger = viewButton;
                
                //スペースキー押したらCardの関数呼ぶ
                if (Input.GetKeyDown(KeyCode.V) || _miniGameChara.Input.GetButtonDown("A"))
                {
                    //ケーキのカウント
                    Control_SE.Get_Instance().Play_SE("UI_Correct");
                    _cakeGenerator.SetCntFin(_id, _cntCake);
                    isCountFin = true;//カウント終了
                    _cntText.color = Color.yellow;
                }
            }
            //操作ができるエネミー2
            if (_miniGameChara.Name == "ショートケーキ")
            {
                if (Input.GetKeyDown(KeyCode.Y))
                {
                    Control_SE.Get_Instance().Play_SE("UI_Select");
                    _cntCake += 1;
                    _cntText.text = Convert.ToString(_cntCake);
                }
                if (Input.GetKeyDown(KeyCode.H))
                {
                    Control_SE.Get_Instance().Play_SE("UI_Select");
                    if (_cntCake > 0) _cntCake -= 1;
                    _cntText.text = Convert.ToString(_cntCake);
                }

                float viewButton = _miniGameChara.Input.GetAxis("Vertical");
                if (_beforeTrigger == 0.0f)
                {
                    if (viewButton < 0.0f)
                    {
                        Control_SE.Get_Instance().Play_SE("UI_Select");
                        _cntCake += 1;
                        _cntText.text = Convert.ToString(_cntCake);
                    }
                    if (viewButton > 0.0f)
                    {
                        Control_SE.Get_Instance().Play_SE("UI_Select");
                        if (_cntCake > 0) _cntCake -= 1;
                        _cntText.text = Convert.ToString(_cntCake);
                    }
                }
                _beforeTrigger = viewButton;


                //スペースキー押したらCardの関数呼ぶ
                if (Input.GetKeyDown(KeyCode.N) || _miniGameChara.Input.GetButtonDown("A"))
                {
                    //ケーキのカウント
                    Control_SE.Get_Instance().Play_SE("UI_Correct");
                    _cakeGenerator.SetCntFin(_id, _cntCake);
                    isCountFin = true;//カウント終了
                    _cntText.color = Color.yellow;
                }
            }
            //操作ができるエネミー3
            if (_miniGameChara.Name == "アップルパイ")
            {
                if (Input.GetKeyDown(KeyCode.I))
                {
                    Control_SE.Get_Instance().Play_SE("UI_Select");
                    _cntCake += 1;
                    _cntText.text = Convert.ToString(_cntCake);
                }
                if (Input.GetKeyDown(KeyCode.K))
                {
                    Control_SE.Get_Instance().Play_SE("UI_Select");
                    if (_cntCake > 0) _cntCake -= 1;
                    _cntText.text = Convert.ToString(_cntCake);
                }

                float viewButton = _miniGameChara.Input.GetAxis("Vertical");
                if (_beforeTrigger == 0.0f)
                {
                    if (viewButton < 0.0f)
                    {
                        Control_SE.Get_Instance().Play_SE("UI_Select");

                        _cntCake += 1;
                        _cntText.text = Convert.ToString(_cntCake);
                    }
                    if (viewButton > 0.0f)
                    {
                        Control_SE.Get_Instance().Play_SE("UI_Select");
                        if (_cntCake > 0) _cntCake -= 1;
                        _cntText.text = Convert.ToString(_cntCake);
                    }
                }
                _beforeTrigger = viewButton;

                //スペースキー押したらCardの関数呼ぶ
                if (Input.GetKeyDown(KeyCode.M) || _miniGameChara.Input.GetButtonDown("A"))
                {
                    //ケーキのカウント
                    Control_SE.Get_Instance().Play_SE("UI_Correct");
                    _cakeGenerator.SetCntFin(_id, _cntCake);
                    isCountFin = true;//カウント終了
                    _cntText.color = Color.yellow;
                }
            }
        }
        //操作が出来ないエネミー(CPUは完全ランダム)
        else if(isCountTime && !isCountFin && _miniGameChara.IsAutomatic)
        {
            int rand = UnityEngine.Random.Range(_cakeGenerator.GetNumQuestCake() - 2, _cakeGenerator.GetNumQuestCake() + 3);
            _cntCake = rand;
            Debug.Log(rand);
            //_cntText.text = Convert.ToString(rand);
            //_cntText.text = "?";
            _AISelectCount = rand;
            _cakeGenerator.SetCntFin(_id, _cntCake);
            isCountFin = true;//カウント終了
        }
    }

    public void ShowSelectCount()
    {
        _cntText.color = Color.white;
        if (!_miniGameChara.IsAutomatic) return;
        _cntText.text = _AISelectCount.ToString();
    }

}