using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealEffectManager : MonoBehaviour
{
    private StealEffect _stealEffect;

    private bool _effectPlaying = false;
    public bool IsEffectPlaying => _effectPlaying;

    [SerializeField]
    private GameObject _stealEffectPrefab;

    public void EffectStart(CharacterBase from,CharacterBase to,Sprite souvenirSprite)
    {
        _stealEffect = Instantiate(_stealEffectPrefab, from.transform.position, new Quaternion(0, 0, 0, 0)).GetComponent<StealEffect>();
        _stealEffect.Init(from.transform.position, from.transform.up, to.transform.position, to.transform.up,souvenirSprite);

        _effectPlaying = true;
    }

    //=======================

    void Start()
    {
        
    }

    void Update()
    {
        if (!_effectPlaying)
            return;

        if(_stealEffect.IsAnimEnd)
        {
            Destroy(_stealEffect.gameObject);

            _effectPlaying = false;
        }
    }
}
