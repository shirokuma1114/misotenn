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
        //�n������
        if(isRotation) earth.transform.Rotate(new Vector3(0, -0.3f, 0));
        //�X�y�[�X�������ꂽ��n�����񂷂̂���߂ē��{�ɃY�[������
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isRotation = false;
            //�J�����ʒu���
            transform.DOMove(_transPos, 2);
        }
    }
}
