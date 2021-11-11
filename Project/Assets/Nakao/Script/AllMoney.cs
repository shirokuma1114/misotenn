using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllMoney : MonoBehaviour
{
    public Image Ex1n2out;
    public Image Ex1n2in;
    //public Text Ex1n2text;

    public bool mn;
    public bool inb;

    private GameObject m_rtObj;
    private RectTransform m_rtView; //グラフ作成用

    //とりあえずリスト
    List<int> testData = new List<int> {
            10,15,20,90,70,50,40,45,60,55,50,45,30};

    private void Awake()
    {
        m_rtObj = GameObject.Find("EX1-2");
        m_rtView = m_rtObj.transform.Find("Ex1n2MainOutFrame/Ex1n2MainInFrame").GetComponent<RectTransform>();
        //m_rtView = transform.Find("view").GetComponent<RectTransform>();
        //CreateDot(new Vector2(200.0f, 200.0f));
    }

    // Start is called before the first frame update
    void Start()
    {
        mn = false;
        inb = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!mn)
        {
            Ex1n2out.enabled = false;
            Ex1n2in.enabled = false;
            //Ex1n2text.enabled = false;
           
            if(inb)
            {
                inb = false;
                DeleteGraph();
            }
           
        }
        else
        {
            Ex1n2out.enabled = true;
            Ex1n2in.enabled = true;
            //Ex1n2text.enabled = true;
            //Ex1n2text.text = " ";

            if(!inb)
            {
                inb = true;
                ShowGraph(testData);  //グラフ表示
            }
        }
    }

    public void OnClick()
    {
        mn = true;
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
        float fMaxY = 100.0f;   //Yの最大値
        float fPitchX = 50.0f;  //Xの左右間隔
        float fOffsetX = 30.0f; //Xの点開始位置

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

        Vector2 dir = (_pointB - _pointA).normalized;         //ベクトルを取得して正規化
        float fDistance = Vector2.Distance(_pointA, _pointB); //長さ取得

        rtLine.anchorMin = Vector2.zero;
        rtLine.anchorMax = Vector2.zero;

        rtLine.sizeDelta = new Vector2(fDistance, 5.0f);
        rtLine.localEulerAngles = new Vector3(
            0.0f, 0.0f,
            Vector2.SignedAngle(new Vector2(1.0f, 0.0f), dir));

        rtLine.anchoredPosition = _pointA + dir * fDistance * 0.5f;
    }

    private void DeleteGraph()
    {
        GameObject obj = GameObject.Find("Ex1n2MainInFrame");

        // 子オブジェクトをループして取得
        foreach (Transform child in obj.transform)
        {
            // 一つずつ破棄する
            Destroy(child.gameObject);
        }
    }
}
