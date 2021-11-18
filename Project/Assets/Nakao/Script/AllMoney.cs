using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllMoney : MonoBehaviour
{
    public Image Ex1n2out;
    public Image Ex1n2in;
    public Image ExScroll;
    public Image ExScOut;
    // public Image ExScIn;
    public Image ExBar;
    public Image ExBarIn;
    //public Image ExBarOut;
    //public Text Ex1n2text;

    public bool mn;
    public bool inb;

    private GameObject m_rtObj;
    private RectTransform m_rtView; //�O���t�쐬�p
    //private RectTransform m_rtView2;
    //private RectTransform m_templateLabelX;
    //private RectTransform m_templateLabelY;

    //�Ƃ肠�������X�g
    List<int> testData = new List<int> {
            10,15,20,90,70,50,40,45,60,55,50,45,30,40,50,20,15,45,30,70,90,15};

    private void Awake()
    {
        m_rtObj = GameObject.Find("EX1-2");
        //m_rtView = m_rtObj.transform.Find("Ex1n2MainOutFrame/Ex1n2MainInFrame").GetComponent<RectTransform>();
        m_rtView = m_rtObj.transform.Find("Scroll_View/Viewport/Content").GetComponent<RectTransform>();
        //m_rtView = transform.Find("view").GetComponent<RectTransform>();
        
        //m_rtView2 = m_rtObj.transform.Find("Ex1n2MainOutFrame/Ex1n2el").GetComponent<RectTransform>();
        //m_templateLabelX = m_rtObj.transform.Find("Ex1n2MainOutFrame/Ex1n2MainInFrame/1n2Text").GetComponent<RectTransform>();
        //m_templateLabelY = m_rtObj.transform.Find("Ex1n2MainOutFrame/Ex1n2MainInFrame/1n2Text2").GetComponent<RectTransform>();

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
            ExScroll.enabled = false;
            ExScOut.enabled = false;
            //ExScIn.enabled = false;
            ExBar.enabled = false;
            ExBarIn.enabled = false;
            //ExBarOut.enabled = false;
            //Ex1n2text.enabled = false;

            if (inb)
            {
                inb = false;
                DeleteGraph();
            }
           
        }
        else
        {
            Ex1n2out.enabled = true;
            Ex1n2in.enabled = true;
            ExScroll.enabled = true;
            ExScOut.enabled = true;
            //ExScIn.enabled = true;
            ExBar.enabled = true;
            ExBarIn.enabled = true;
            //ExBarOut.enabled = true;
            //Ex1n2text.enabled = true;
            //Ex1n2text.text = " ";

            if (!inb)
            {
                inb = true;
                ShowGraph(testData);  //�O���t�\��
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

            //���쐬
            //RectTransform rtLabelX = Instantiate(m_templateLabelX, m_rtView2);
            //rtLabelX.anchoredPosition = new Vector2(fPosX, 0.0f);
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

    private void DeleteGraph()
    {
        //GameObject obj = GameObject.Find("Ex1n2MainInFrame");
        GameObject obj = GameObject.Find("Content");

        // �q�I�u�W�F�N�g�����[�v���Ď擾
        foreach (Transform child in obj.transform)
        {
            // ����j������
            Destroy(child.gameObject);
        }
    }
}
