using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protector : MonoBehaviour
{
    private CharacterBase _character;
    private CharacterState _prevCharacterState;

    private bool _protected;
    public bool IsProtected => _protected;

    private int _turnEndCount;
    private int _protectTurn;

    // Start is called before the first frame update
    void Start()
    {
        _character = GetComponent<CharacterBase>();
        _prevCharacterState = _character.State;

        _protected = false;

        _turnEndCount = -1;
    }

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
}
