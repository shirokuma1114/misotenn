using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class cont : MonoBehaviour
{
    private MiniGameCharacter _controller;
    public MiniGameCharacter Character => _controller;
    private manager _manager;

    [SerializeField]
    private ui _playerUI;
    [SerializeField]
    private KeyCode _rendaKey;
    [SerializeField]
    private string _cakeName;
    public string CakeName => _cakeName;


    public void Init(MiniGameCharacter character, manager manager)
    {
        _controller = character;
        _manager = manager;

        _playerUI.SetPlayerName(character.Name);
        //_playerUI.SetRendaKeyEnable(!character.IsAutomatic);
    }

    //==================

    void Start()
    {
       
    }

    void Update()
    {
        if (!_manager)
            return;

        switch (_manager.State)
        {
            case manager.GameState.PLAY_NOW:
                if (_controller.IsAutomatic)
                    AutomaticPlay();
                else
                    HumanPlay();
                break;

            case manager.GameState.PLAY:
                //if (_rotateTimeCounter > _manager.PlayTime / _rendaCounter)
                //{
                //    _rotateTimeCounter = 0;
                //}

                //_rotateTimeCounter += Time.deltaTime;
                break;

            default:
                break;
        }
    }

    private void AutomaticPlay()
    {
        //_rendaCounter = Random.Range(20, 50);
    }

    private void HumanPlay()
    {
       // if (Input.GetKeyDown(_rendaKey) || _controller.Input.GetButton(""))
            //_rendaCounter++;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    //if (other.gameObject.name == "GoalCollider")
    //       // RotateCountUp();
    //}

    //private void RotateCountUp()
    //{
    //    //_rotateCounter++;
    //    //_playerUI.SetRotateCounter(_rotateCounter);
    //}
}
