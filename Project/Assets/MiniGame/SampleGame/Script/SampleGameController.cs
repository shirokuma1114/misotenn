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
    private float _rotateTimeCounter;
    private float _animTimeCounter;

    [SerializeField]
    private SampleGameControllerUI _playerUI;
    [SerializeField]
    private KeyCode _rendaKey;
    [SerializeField]
    private string _cakeName;
    public string CakeName => _cakeName;


    public void Init(MiniGameCharacter character,SampleMiniGameManager manager)
    {
        _controller = character;
        _manager = manager;

        _playerUI.SetPlayerName(character.Name);
        _playerUI.SetRendaKeyEnable(!character.IsAutomatic);
    }

    public void Go()
    {
        var earth = GameObject.Find("Earth");
        _rotateCenterObject = new GameObject();
        _rotateCenterObject.transform.position = earth.transform.position;
        _rotateCenterObject.transform.forward = -transform.right;
        transform.SetParent(_rotateCenterObject.transform);

        _rotateCenterObject.transform.DORotate(new Vector3(360 * _rendaCounter, 0, 0), _manager.PlayTime, RotateMode.LocalAxisAdd)
            .SetEase(Ease.InOutQuart);
    }
    
    //==================

    void Start()
    {
        _rendaCounter = 0;
        _rotateCounter = -1;
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
                if(_rotateTimeCounter > _manager.PlayTime / _rendaCounter)
                {
                    _rotateTimeCounter = 0;
                }

                _rotateTimeCounter += Time.deltaTime;
                break;

            default:
                break;
        }
    }

    private void AutomaticPlay()
    {
        _rendaCounter = Random.Range(20, 50);
    }

    private void HumanPlay()
    {
        if (Input.GetKeyDown(_rendaKey) || _controller.Input.GetButtonDown("A"))
            _rendaCounter++;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "GoalCollider")
            RotateCountUp();
    }

    private void RotateCountUp()
    {
        _rotateCounter++;
        _playerUI.SetRotateCounter(_rotateCounter);
    }
}
