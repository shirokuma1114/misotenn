using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _cake; // ������4�Ԗڂ����3�Ԗڂ����2�Ԗڂ��O��4�Ԗڂ��l��
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
            // �I������
            _endFlg=true;
            Debug.Log("�I��");
        }
    }

    public void AddCake(GameObject cake)
    {
        _cake.Add(cake);
    }
}
