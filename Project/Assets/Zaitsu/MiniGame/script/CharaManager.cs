using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _cake; // こいつの4番目が一位3番目が二位2番目が三位4番目が四位
    public List<GameObject> Cake=>_cake;
    [SerializeField]
    public StartCounterGura _counterGura;
    public bool _countFlg;
    public bool _endFlg;

    void Awake()
    {
        _countFlg = false;
        _endFlg = false;
    }

    void Update()
    {
        _countFlg = _counterGura.CountFlg;

        if(_cake.Count>=3)
        {
            // 終了処理
            _endFlg=true;
            Debug.Log("終了");
        }
    }

    public void AddCake(GameObject cake)
    {
        _cake.Add(cake);
    }
}
