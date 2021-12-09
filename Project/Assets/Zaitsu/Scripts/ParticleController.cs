using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    ParticleSystem _ps;

    void Start()
    {
        _ps = GetComponent<ParticleSystem>();
        SetEmission(0.0f);
    }

    public void SetEmission(float emission)
    {
        var em = _ps.emission;

        em.rateOverTimeMultiplier = emission;
    }
}