using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameManager : MonoBehaviour
{
    [Header("地球回転テスト部分")]
    [SerializeField]
    private GameObject _earth;
    [SerializeField]
    private GameObject _targetPos;


    [Header("カードテスト部分")]
    [Space(20)]
    [SerializeField]
    private List<int> _cardNumberList;
    [SerializeField]
    private bool _cardCreate;
    [SerializeField]
    private GameObject _cardManager;


    [Header("BGM")]
    [Space(20)]
    [SerializeField]
    private BGMManager _bgmManage;

    // Start is called before the first frame update
    void Start()
    {
        _targetPos = null;
        _cardCreate = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(_targetPos)
        {
            _earth.GetComponent<EarthMove>().MoveToPosition(_targetPos.transform.localPosition);

            _targetPos = null;
        }


        if(_cardCreate)
        {
            _cardManager.GetComponent<MoveCardManager>().SetCardList(_cardNumberList);

            _cardCreate = false;
        }
    }
}
