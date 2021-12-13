using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject earth;
    [SerializeField] Vector3 _transPos;
    bool isRotation = true;

    // Update is called once per frame
    void Update()
    {
        //地球を回す
        if(isRotation) earth.transform.Rotate(new Vector3(0, -0.3f, 0));
        //スペースが押されたら地球を回すのをやめて日本にズームする
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isRotation = false;
            //カメラ位置補間
            transform.DOMove(_transPos, 2);
        }
    }
}
