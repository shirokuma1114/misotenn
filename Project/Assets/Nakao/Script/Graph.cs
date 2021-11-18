using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Graph : MonoBehaviour
{
    private RectTransform m_rtView; //�O���t�쐬�p

    List<int> testData = new List<int> {
            10,15,20,90,70,50,40,45,60,55,50,45,30};

    bool a = false;

    private void Awake()
    {
        m_rtView = transform.Find("Ex1n2MainInFrame").GetComponent<RectTransform>();
        //CreateDot(new Vector2(200.0f, 200.0f));

        //List<int> testData = new List<int> {
        //    10,15,20,90,70,50,40,45,60,55,50,45,30};

        //�O���t�\��
        //ShowGraph(testData);
    }

    private void Update()
    {
        if(!a)
        {
            //�O���t�\��
            ShowGraph(testData);
            a = true;
        }
    }

    private GameObject CreateDot(Vector2 _position)
    {
        GameObject objDot = new GameObject("dot", typeof(Image));
        objDot.GetComponent<Image>().color = Color.black;
        objDot.transform.SetParent(m_rtView, false);
        RectTransform rtDot = objDot.GetComponent<RectTransform>();
        rtDot.anchoredPosition = _position;
        rtDot.sizeDelta = new Vector2(10.0f, 10.0f);
        rtDot.anchorMin = Vector2.zero;
        rtDot.anchorMax = Vector2.zero;
        return objDot;
    }

    private void ShowGraph(List<int> _dataist)
    {
        float fGraphHeight = m_rtView.sizeDelta.y;
        float fMaxY = 100.0f;   //Y�̍ő�l
        float fPitchX = 50.0f;  //X�̍��E�Ԋu
        float fOffsetX = 30.0f; //X�̓_�J�n�ʒu

        GameObject objLast = null;

        for (int i = 0; i < _dataist.Count; i++)
        {
            float fPosX = i * fPitchX + fOffsetX;
            float fPosY = (_dataist[i] / fMaxY) * fGraphHeight;
            GameObject objDot = CreateDot(new Vector2(fPosX, fPosY));

            if (objLast != null)
            {
                CreateLine(objLast.GetComponent<RectTransform>().anchoredPosition, //a
                           objDot.GetComponent<RectTransform>().anchoredPosition   //b
                           );
            }
            objLast = objDot;
        }
    }

    private void CreateLine(Vector2 _pointA, Vector2 _pointB)
    {
        GameObject objLine = new GameObject("dotLine", typeof(Image));
        objLine.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 0.5f);
        objLine.transform.SetParent(m_rtView, false);

        RectTransform rtLine = objLine.GetComponent<RectTransform>();

        Vector2 dir = (_pointB - _pointA).normalized;         //�x�N�g�����擾���Đ��K��
        float fDistance = Vector2.Distance(_pointA, _pointB); //�����擾

        rtLine.anchorMin = Vector2.zero;
        rtLine.anchorMax = Vector2.zero;

        rtLine.sizeDelta = new Vector2(fDistance, 5.0f);
        rtLine.localEulerAngles = new Vector3(
            0.0f, 0.0f,
            Vector2.SignedAngle(new Vector2(1.0f, 0.0f), dir));

        rtLine.anchoredPosition = _pointA + dir * fDistance * 0.5f;
    }
}
