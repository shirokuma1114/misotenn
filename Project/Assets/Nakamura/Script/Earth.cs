using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Earth : MonoBehaviour
{
    private bool isDefaultScale;


    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
         transform.Rotate(new Vector3(0.0f, 0.0f, -0.1f));
    }
}
