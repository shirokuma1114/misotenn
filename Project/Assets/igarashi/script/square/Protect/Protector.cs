using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protector : MonoBehaviour
{
    private CharacterBase _character;
    private CharacterState _prevCharacterState;
    
    private bool _protected;
    public bool IsProtected => _protected;

    private int _turnEndCount = -1;
    private int _protectTurn;

    private ProtectEffect _protectInstance;

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(_protected)
        {
            if (_prevCharacterState != CharacterState.WAIT && _character.State == CharacterState.WAIT)
            {
                _turnEndCount++;
            }

            if (_turnEndCount >= _protectTurn)
            {
                _protected = false;
                Destroy(_protectInstance.gameObject);
            }

            _prevCharacterState = _character.State;
        }        
    }


    public void ProtectStart(int protectTurn, ProtectEffect protect)
    {
        _protected = true;
        _protectTurn = protectTurn;
        _turnEndCount = -1;
        _protectInstance = protect;
    }

    public void SetCharacter(CharacterBase character)
    {
        _character = character;
        _prevCharacterState = _character.State;
    }

}
