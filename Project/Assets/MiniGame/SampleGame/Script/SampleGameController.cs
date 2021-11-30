using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SampleGameController : MonoBehaviour
{
    private MiniGameCharacter _controller;
    private SampleMiniGameManager _manager;

    private int _rendaCounter;
    public int RendaCount => _rendaCounter;

    private GameObject _rotateCenterObject;
    private int _rotateCounter;
    private float _rotateTimeCounter;
    private float _animTimeCounter;

    [SerializeField]
    private SampleGameControllerUI _playerUI;


    public void Init(MiniGameCharacter character,SampleMiniGameManager manager)
    {
        _controller = character;
        _manager = manager;

        _playerUI.SetPlayerName(character.Name);
    }

    public void Go()
    {
        var earth = GameObject.Find("Earth");
        _rotateCenterObject = new GameObject();
        _rotateCenterObject.transform.position = earth.transform.position;
        _rotateCenterObject.transform.forward = -transform.right;
        transform.SetParent(_rotateCenterObject.transform);

        _rotateCenterObject.transform.DORotate(new Vector3(360 * _rendaCounter, 0, 0), _manager.PlayTime, RotateMode.LocalAxisAdd);
    }

    //==================

    void Start()
    {
        _rendaCounter = 0;
    }

    void Update()
    {
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
                    _rotateCounter++;
                    _playerUI.SetRotateCounter(_rotateCounter);

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
        _rendaCounter = Random.Range(40, 60);
    }

    private void HumanPlay()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            _rendaCounter++;
    }
}
