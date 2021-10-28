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
        //transform.DOScale(new Vector3(0, 0, 0), 0.2f);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isDefaultScale)
            {
                transform.DOScale(new Vector3(2, 4, 2), 0.2f);
                isDefaultScale = false;
            }
            else if (!isDefaultScale)
            {
                transform.DOScale(new Vector3(10, 10, 10), 0.2f);
                isDefaultScale = true;
            }
        }
    

         transform.Rotate(new Vector3(0.01f, -0.1f, 0.01f));
    }
}
