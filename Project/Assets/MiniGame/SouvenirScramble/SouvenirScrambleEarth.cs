using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SouvenirScrambleEarth : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.position,new Vector3(-1, 0, 0), _rotateSpeed * Time.deltaTime);
    }
}
