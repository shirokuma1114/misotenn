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

    
    // Update is called once per frame
    void Update()
    {
        if(_protected)
        {
            if (_prevCharacterState != CharacterState.END && _character.State == CharacterState.END)
            {
                _turnEndCount++;
            }

            if (_turnEndCount >= _protectTurn)
            {
                _protected = false;
            }

            _prevCharacterState = _character.State;
        }        
    }


    public void ProtectStart(int protectTurn)
    {
        _protected = true;
        _protectTurn = protectTurn;
        _turnEndCount = -1;
    }

    public void SetCharacter(CharacterBase character)
    {
        _character = character;
        _prevCharacterState = _character.State;
    }

}
