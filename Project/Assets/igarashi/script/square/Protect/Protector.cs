using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protector : MonoBehaviour
{
    private CharacterBase _character;

    private CharacterControllerBase _controller;
    private CharacterControllerBase.EventState _prevControllerState;
    
    private bool _protected;
    public bool IsProtected => _protected;

    private int _turnEndCount = 0;
    private int _protectTurn;

    private ProtectEffect _protectInstance;

    private void Start()
    {
        _controller = GetComponent<CharacterControllerBase>();
        _prevControllerState = _controller.State;
    }

    // Update is called once per frame
    void Update()
    {
        if (_protected)
        {
            if (_prevControllerState != CharacterControllerBase.EventState.SELECT && _controller.State == CharacterControllerBase.EventState.SELECT)
            {
                _turnEndCount++;
            }

            if (_turnEndCount >= _protectTurn)
            {
                _protected = false;
                Destroy(_protectInstance.gameObject);
            }

            _prevControllerState = _controller.State;
        }        
    }


    public void ProtectStart(int protectTurn, ProtectEffect protect)
    {
        _protected = true;
        _protectTurn = protectTurn;
        _turnEndCount = 0;
        _protectInstance = protect;
    }

    public void SetCharacter(CharacterBase character)
    {
        _character = character;
    }

}
