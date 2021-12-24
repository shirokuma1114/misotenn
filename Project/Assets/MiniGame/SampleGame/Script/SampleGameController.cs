using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SampleGameController : MonoBehaviour
{
    private MiniGameCharacter _controller;
    public MiniGameCharacter Character => _controller;
    private SampleMiniGameManager _manager;

    private int _rendaCounter;
    public int RendaCount => _rendaCounter;

    private GameObject _rotateCenterObject;
    private int _rotateCounter;
    public int RotateCount => _rotateCounter;
    private float _prevRotX;
    private float _defaultRotX;

    [SerializeField]
    private SampleGameControllerUI _playerUI;
    public SampleGameControllerUI UI => _playerUI;

    private KeyCode _rendaKey;

    [SerializeField]
    private string _cakeName;
    public string CakeName => _cakeName;

    [SerializeField]
    private GameObject _rendaEffectPrefab;

    private const float AI_RENDA_INTERVAL = 0.1f;
    private float _aiRendaIntervalCounter;


    public void Init(MiniGameCharacter character,SampleMiniGameManager manager,KeyCode rendaKey)
    {
        _controller = character;
        _manager = manager;
        _rendaKey = rendaKey;

        _playerUI.SetPlayerName(character.Name);
        _playerUI.SetPlayerIcon(character.Icon);
        _playerUI.SetRendaKeyEnable(!character.IsAutomatic);
    }

    public void Go()
    {
        var earth = GameObject.Find("Earth");
        _rotateCenterObject = new GameObject();
        _rotateCenterObject.transform.position = earth.transform.position;
        _rotateCenterObject.transform.forward = -transform.right;
        transform.SetParent(_rotateCenterObject.transform);

        _defaultRotX = _prevRotX = _rotateCenterObject.transform.localRotation.x - 0.001f;

        _rotateCenterObject.transform.DORotate(new Vector3(360 * _rendaCounter + 0.1f, 0, 0), _manager.PlayTime, RotateMode.LocalAxisAdd)
            .SetEase(Ease.InOutQuart);

        Debug.Log(_cakeName + ":" + _rendaCounter);
    }
    
    //==================

    void Start()
    {
        _rendaCounter = 0;
        _rotateCounter = 0;        
    }

    void Update()
    {
        if (!_manager)
            return;

        switch (_manager.State)
        {
            case SampleMiniGameManager.SampleGameState.PLAY_RENDA:
                if (_controller.IsAutomatic)
                    AutomaticPlay();
                else
                    HumanPlay();
                break;

            case SampleMiniGameManager.SampleGameState.PLAY:
                float nowRotX = Mathf.Abs(_rotateCenterObject.transform.localRotation.x);

                if ((nowRotX < -_defaultRotX && _prevRotX > -_defaultRotX) || (nowRotX > _defaultRotX && _prevRotX < _defaultRotX))
                {
                    RotateCountUp();

                    if (_cakeName == "�t���W�G")
                        Control_SE.Get_Instance().Play_SE("Wind").pitch = 1.5f;
                }

                _prevRotX = nowRotX;
                break;

            default:
                break;
        }
    }

    private void AutomaticPlay()
    {
        _rendaCounter = Random.Range(20, 50);

        if(_aiRendaIntervalCounter >= AI_RENDA_INTERVAL)
        {
            Instantiate(_rendaEffectPrefab, transform);
            _aiRendaIntervalCounter = 0.0f;
            Control_SE.Get_Instance().Play_SE("Power").pitch = 1.8f;
        }
        _aiRendaIntervalCounter += Time.deltaTime;
    }

    private void HumanPlay()
    {
        if (Input.GetKeyDown(_rendaKey) || _controller.Input.GetButtonDown("A"))
        {
            _rendaCounter++;

            Instantiate(_rendaEffectPrefab,transform);

            Control_SE.Get_Instance().Play_SE("Power").pitch = 1.8f;
        }
    }

    private void RotateCountUp()
    {
        _rotateCounter++;
        _playerUI.SetRotateCounter(_rotateCounter);
    }
}
