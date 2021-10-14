using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    private Vector3 _center;

    // Start is called before the first frame update
    void Start()
    {
        _center = FindObjectOfType<EarthMove>().transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.up = transform.position - _center;
    }
}
