using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private BGMManager _bgmManager;
    [SerializeField]
    private AudioClip _nextClip;


    [Header("UI")]
    [Space(20)]
    [SerializeField]
    private SelectUI _selectUI;
    [SerializeField]
    private List<string> _elements;
    [SerializeField]
    private bool _open = false;

    [SerializeField]
    WindowBase _window;

    // Start is called before the first frame update
    void Start()
    {
        _targetPos = null;
        _cardCreate = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SceneManager.LoadScene(4);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            SceneManager.LoadScene(5);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            SceneManager.LoadScene(6);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            SceneManager.LoadScene(7);
        if (Input.GetKeyDown(KeyCode.Alpha5))
            SceneManager.LoadScene(8);

        if (_targetPos)
        {
            _earth.GetComponent<EarthMove>().MoveToPosition(_targetPos.transform.localPosition);

            _targetPos = null;
        }


        if(_cardCreate)
        {
            //_cardManager.GetComponent<MoveCardManager>().SetCardList(_cardNumberList);

            _cardCreate = false;
        }


        if(_nextClip)
        {
            _bgmManager.GetComponent<BGMManager>().SetNextBGMClip(_nextClip);

            _nextClip = null;
        }


        if(_open)
        {
            //_selectUI.Open(_elements);

            _open = false;
        }
    }
}
