using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CakeController : MonoBehaviour
{
    private CakeGenerator _cakeGenerator;
    public int MyType { set; get; }
    private float _speed = -3;

    void Start()
    {
        _cakeGenerator = GameObject.Find("GameManager").GetComponent<CakeGenerator>();
    }

    void Update()
    {
        //À•W‚ğ‘‚«Š·‚¦‚é
        transform.position += new Vector3(_speed, 0, 0) * Time.deltaTime;

        //©g‚ğÁ‚·
        if(transform.position.x < -15.0f) Destroy(this.gameObject);
    }

    void OnTriggerStay(Collider other)
    {
        Destroy(this.gameObject);
        _cakeGenerator.DecreaseCake(MyType);
    }
}
