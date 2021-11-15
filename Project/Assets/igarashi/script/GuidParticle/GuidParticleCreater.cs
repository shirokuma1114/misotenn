using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidParticleCreater : MonoBehaviour
{
    private CharacterBase _character;

    private GuidParticle _particle;

    [SerializeField]
    private GameObject _guidParticlePrefab;


    void Start()
    {
        _character = GetComponent<CharacterBase>();
    }

    void Update()
    {
        CreateParticle();
    }

    private void CreateParticle()
    {
        if (_particle)
            return;

        _particle = Instantiate(_guidParticlePrefab, transform.localPosition,transform.rotation).GetComponent<GuidParticle>();
        _particle.InitStartSquare(_character.CurrentSquare);
    }
}
