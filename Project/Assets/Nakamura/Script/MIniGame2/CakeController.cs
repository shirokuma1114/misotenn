using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CakeController : MonoBehaviour
{
    private float _speed = -2;

    void Start()
    {
        
    }

    void Update()
    {
        //À•W‚ğ‘‚«Š·‚¦‚é
        transform.position += new Vector3(_speed, 0, 0) * Time.deltaTime;

        if(transform.position.x < -15.0f) Destroy(this.gameObject);
    }
}
