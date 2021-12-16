using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public class ChangeText : MonoBehaviour
{
    //text
    public Text text;
    public Image image1;
    public Image image2;
    public Image image3;
    public Image image4;

    [SerializeField]
    private Text _p1ItemText;
    [SerializeField]
    private Text _p2ItemText;
    [SerializeField]
    private Text _p3ItemText;
    [SerializeField]
    private Text _p4ItemText;

    [SerializeField]
    private Text _p1NumText;
    [SerializeField]
    private Text _p2NumText;
    [SerializeField]
    private Text _p3NumText;
    [SerializeField]
    private Text _p4NumText;

    public Image Top1;
    public Image Top2;
    public Image Back;
    public Image P1out;
    public Image P1in;
    public Image P2out;
    public Image P2in;
    public Image P3out;
    public Image P3in;
    public Image P4out;
    public Image P4in;

    public Image P1mout;
    public Image P1min;
    public Image P2mout;
    public Image P2min;
    public Image P3mout;
    public Image P3min;
    public Image P4mout;
    public Image P4min;

    public Image ReturnImg;
    public Image Sideout;
    public Image Sidein;

    public Image EX1out;
    public Image EX1in;
    public Image EX2out;
    public Image EX2in;
    public Image EX3out;
    public Image EX3in;
    public Image EX4out;
    public Image EX4in;
    public Image EX5out;
    public Image EX5in;
    public Image EX6out;
    public Image EX6in;
    //public Image EX7out;
    //public Image EX7in;
    //public Image EX8out;
    //public Image EX8in;

    public Text EX1text;
    public Text EX2text;
    public Text EX3text;
    public Text EX4text;
    public Text EX5text;
    public Text EX6text;
    //public Text EX7text;
    //public Text EX8text;
    public Text EX1text2;
    public Text EX2text2;
    public Text EX3text2;
    public Text EX4text2;
    public Text EX5text2;
    public Text EX6text2;
    //public Text EX7text2;
    //public Text EX8text2;

    public Image EX1n2out;
    public Image EX1n2in;
    public Image EXScroll;
    public Image EXScOut;
    public Image EXBar;
    public Image EXBarIn;

    public ScrollRect myScrollRect;
    public Scrollbar newScrollBar;
    public float MoveBar = 0.01f;

    private GameObject m_rtObj;
    private RectTransform m_rtView; //グラフ作成用

    [SerializeField]
    private List<Text> _selectTexts;


    [SerializeField]
    Animator _fadeAnimation;

    bool _isFadeOut;

    //決定Key用
    int Choice = 0;
    bool NextPage = false;
    bool isNextMove = false;
    bool ExPage1 = false;
    bool ExPage2 = false;
    int wk = 0;

    int _selectIndex;
   

    CharacterData[] _characters;

    //とりあえずリスト
    List<int> testData = new List<int> {
            10,15,20,90,70,50,40,45,60,55,50,45,30,40,50,20,15,45,30,70,90,15};

    //Awake
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
        _fadeAnimation.Play("FadeIn");

        InitCharacterInfo();

        ShowRank();

        //{
        //    P1out.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        //    P1in.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        //    P2out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        //    P2in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        //    P3out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        //    P3in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        //    P4out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        //    P4in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        //    ReturnImg.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        //    Sideout.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        //    Sidein.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        //}

        {
            P1out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            P1in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            P2out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            P2in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            P3out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            P3in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            P4out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            P4in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            ReturnImg.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            Sideout.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            Sidein.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }

    private void InitCharacterInfo()
    {


        var manager = FindObjectOfType<DontDestroyManager>();
        _characters = new CharacterData[4];


        if (manager)
        {
            var charaOriginData = manager.GetCharacterData();
            for (int i = 0; i < 4; i++)
            {
                _characters[i] = new CharacterData();
                _characters[i]._characterName = charaOriginData[i]._characterName;
                _characters[i]._lapCount = charaOriginData[i]._lapCount;
                _characters[i]._money = charaOriginData[i]._money;
                _characters[i]._useEventNumByType = new int[(int)SquareEventType.EVENT_TYPE_MAX];
                Array.Copy(charaOriginData[i]._character.Log.GetUseEventNum(), _characters[i]._useEventNumByType, _characters[i]._useEventNumByType.Length);
                _characters[i]._moneyByTurn = new List<int>();
                foreach (var x in charaOriginData[i]._character.Log.GetMoneyByTurn())
                {
                    _characters[i]._moneyByTurn.Add(x);
                }
                _characters[i]._rank = charaOriginData[i]._rank;
                _characters[i]._souvenirNum = charaOriginData[i]._souvenirNum;
                _selectTexts[i].text = _characters[i]._characterName;
            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                _characters[i] = new CharacterData();
                _characters[i]._characterName = "ああ" + i;
                _characters[i]._lapCount = 0;
                _characters[i]._money = 999;
                _characters[i]._useEventNumByType = new int[(int)SquareEventType.EVENT_TYPE_MAX];
                _characters[i]._moneyByTurn = new List<int>();
                _characters[i]._moneyByTurn.Add(999);
                _characters[i]._rank = i;
                _characters[i]._souvenirNum = 0;
                _selectTexts[i].text = _characters[i]._characterName;
            }
        }
    }

    void ShowRank()
    {
        var rankSortedCharacters = _characters;
        rankSortedCharacters = _characters.OrderByDescending(x => x._souvenirNum).ThenByDescending(x => x._money).ToArray();

        for (int i = 0; i < rankSortedCharacters.Count(); i++)
        {
            if (i == 0)
            {
                _p1ItemText.text = rankSortedCharacters[i]._characterName;
                _p1NumText.text = "";
            }
            if (i == 1)
            {
                _p2ItemText.text = rankSortedCharacters[i]._characterName;
                _p2NumText.text = "";
            }
            if (i == 2)
            {
                _p3ItemText.text = rankSortedCharacters[i]._characterName;
                _p3NumText.text = "";
            }
            if (i == 3)
            {
                _p4ItemText.text = rankSortedCharacters[i]._characterName;
                _p4NumText.text = "";
            }
        }
    }

    //CreateDot
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

    //ShowGraph
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

            //軸作成
            //RectTransform rtLabelX = Instantiate(m_templateLabelX, m_rtView2);
            //rtLabelX.anchoredPosition = new Vector2(fPosX, 0.0f);
        }
    }

    //CreateLine
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

    //DeleteGraph
    private void DeleteGraph()
    {
        //GameObject obj = GameObject.Find("Ex1n2MainInFrame");
        GameObject obj = GameObject.Find("Content"); //ココより下の子をdeleteする

        // 子オブジェクトをループして取得
        foreach (Transform child in obj.transform)
        {
            // 一つずつ破棄する
            Destroy(child.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!ExPage1)
        {
            EX1n2out.enabled = false;
            EX1n2in.enabled = false;
            EXScroll.enabled = false;
            EXScOut.enabled = false;
            EXBar.enabled = false;
            EXBarIn.enabled = false;

            P1mout.enabled = true;
            P1min.enabled = true;
            _p1ItemText.enabled = true;
            _p1NumText.enabled = true;
            P2mout.enabled = true;
            P2min.enabled = true;
            _p2ItemText.enabled = true;
            _p2NumText.enabled = true;
            P3mout.enabled = true;
            P3min.enabled = true;
            _p3ItemText.enabled = true;
            _p3NumText.enabled = true;
            P4mout.enabled = true;
            P4min.enabled = true;
            _p4ItemText.enabled = true;
            _p4NumText.enabled = true;
        }
        else
        {

            EX1n2out.enabled = true;
            EX1n2in.enabled = true;
            EXScroll.enabled = true;
            EXScOut.enabled = true;
            EXBar.enabled = true;
            EXBarIn.enabled = true;

            P1mout.enabled = false;
            P1min.enabled = false;
            _p1ItemText.enabled = false;
            _p1NumText.enabled = false;
            P2mout.enabled = false;
            P2min.enabled = false;
            _p2ItemText.enabled = false;
            _p2NumText.enabled = false;
            P3mout.enabled = false;
            P3min.enabled = false;
            _p3ItemText.enabled = false;
            _p3NumText.enabled = false;
            P4mout.enabled = false;
            P4min.enabled = false;
            _p4ItemText.enabled = false;
            _p4NumText.enabled = false;
        }
        if (!ExPage2)
        {
            EX1out.enabled = false;
            EX2out.enabled = false;
            EX3out.enabled = false;
            EX4out.enabled = false;
            EX5out.enabled = false;
            EX6out.enabled = false;
            //EX7out.enabled = false;
            //EX8out.enabled = false;
            EX1in.enabled = false;
            EX2in.enabled = false;
            EX3in.enabled = false;
            EX4in.enabled = false;
            EX5in.enabled = false;
            EX6in.enabled = false;
            //EX7in.enabled = false;
            //EX8in.enabled = false;
            EX1text.enabled = false;
            EX2text.enabled = false;
            EX3text.enabled = false;
            EX4text.enabled = false;
            EX5text.enabled = false;
            EX6text.enabled = false;
            //EX7text.enabled = false;
            //EX8text.enabled = false;
            EX1text2.enabled = false;
            EX2text2.enabled = false;
            EX3text2.enabled = false;
            EX4text2.enabled = false;
            EX5text2.enabled = false;
            EX6text2.enabled = false;
            //EX7text2.enabled = false;
            //EX8text2.enabled = false;

            P1mout.enabled = true;
            P1min.enabled = true;
            _p1ItemText.enabled = true;
            _p1NumText.enabled = true;
            P2mout.enabled = true;
            P2min.enabled = true;
            _p2ItemText.enabled = true;
            _p2NumText.enabled = true;
            P3mout.enabled = true;
            P3min.enabled = true;
            _p3ItemText.enabled = true;
            _p3NumText.enabled = true;
            P4mout.enabled = true;
            P4min.enabled = true;
            _p4ItemText.enabled = true;
            _p4NumText.enabled = true;
        }
        else
        {
            EX1out.enabled = true;
            EX2out.enabled = true;
            EX3out.enabled = true;
            EX4out.enabled = true;
            EX5out.enabled = true;
            EX6out.enabled = true;
            //EX7out.enabled = true;
            //EX8out.enabled = true;
            EX1in.enabled = true;
            EX2in.enabled = true;
            EX3in.enabled = true;
            EX4in.enabled = true;
            EX5in.enabled = true;
            EX6in.enabled = true;
            //EX7in.enabled = true;
            //EX8in.enabled = true;
            EX1text.enabled = true;
            EX2text.enabled = true;
            EX3text.enabled = true;
            EX4text.enabled = true;
            EX5text.enabled = true;
            EX6text.enabled = true;
            //EX7text.enabled = true;
            //EX8text.enabled = true;
            EX1text2.enabled = true;
            EX2text2.enabled = true;
            EX3text2.enabled = true;
            EX4text2.enabled = true;
            EX5text2.enabled = true;
            EX6text2.enabled = true;
            //EX7text2.enabled = true;
            //EX8text2.enabled = true;

            P1mout.enabled = false;
            P1min.enabled = false;
            _p1ItemText.enabled = false;
            _p1NumText.enabled = false;
            P2mout.enabled = false;
            P2min.enabled = false;
            _p2ItemText.enabled = false;
            _p2NumText.enabled = false;
            P3mout.enabled = false;
            P3min.enabled = false;
            _p3ItemText.enabled = false;
            _p3NumText.enabled = false;
            P4mout.enabled = false;
            P4min.enabled = false;
            _p4ItemText.enabled = false;
            _p4NumText.enabled = false;
        }
            if (_isFadeOut)
            {
                if (_fadeAnimation.GetCurrentAnimatorClipInfo(0)[0].clip.name == "FadeOut" && _fadeAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    SceneManager.LoadScene("Title");
                }
                return;
            }


            if (Input.GetKeyDown(KeyCode.Space))
            {
                _fadeAnimation.Play("FadeOut");
                _isFadeOut = true;
                if (Control_SE.Get_Instance()) Control_SE.Get_Instance().Play_SE("UI_Correct");
            }

            bool isMove = false;
            if (Input.GetKeyDown(KeyCode.A))
            {
                _selectIndex = Mathf.Max(--_selectIndex, 0);
                isMove = true;

                P1mout.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                P1min.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                P2mout.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                P2min.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                wk = 0;

                if (Control_SE.Get_Instance()) Control_SE.Get_Instance().Play_SE("UI_Select");
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                _selectIndex = Mathf.Min(++_selectIndex, _characters.Length);
                isMove = true;

                P1mout.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                P1min.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                P2mout.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                P2min.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                wk = 0;

                if (Control_SE.Get_Instance()) Control_SE.Get_Instance().Play_SE("UI_Select");
            }

            if (isMove)
            {
                if (_selectIndex == 0)
                {
                    P1out.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                    P1in.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                    P2out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    P2in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    P3out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    P3in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    P4out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    P4in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    ReturnImg.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    Sideout.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    Sidein.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }
                if (_selectIndex == 1)
                {
                    P1out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    P1in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    P2out.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                    P2in.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                    P3out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    P3in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    P4out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    P4in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    ReturnImg.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    Sideout.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    Sidein.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }
                if (_selectIndex == 2)
                {
                    P1out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    P1in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    P2out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    P2in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    P3out.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                    P3in.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                    P4out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    P4in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    ReturnImg.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    Sideout.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    Sidein.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }
                if (_selectIndex == 3)
                {
                    P1out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    P1in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    P2out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    P2in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    P3out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    P3in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    P4out.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                    P4in.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                    ReturnImg.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    Sideout.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    Sidein.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }

            }

            //戻るキー
            if (Input.GetKeyDown(KeyCode.X))
            {
                ShowRank();
                image1.enabled = true;
                image2.enabled = true;
                image3.enabled = true;
                image4.enabled = true;

                Top1.GetComponent<Image>().color = new Color32(50, 55, 19, 255);
                Top2.GetComponent<Image>().color = new Color32(244, 255, 182, 255);
                Back.GetComponent<Image>().color = new Color32(244, 255, 182, 255);

                P1mout.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                P1min.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                P2mout.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                P2min.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                Choice = 0;
                NextPage = false;
                ExPage1 = false;
                ExPage2 = false;

                DeleteGraph(); //グラフ消去

                if (Control_SE.Get_Instance()) Control_SE.Get_Instance().Play_SE("UI_Close");
            }

        //スクロールバーを動かす
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //myScrollRect.verticalNormalizedPosition -= 0.01f;
            newScrollBar.value -= MoveBar;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            //myScrollRect.verticalNormalizedPosition += 0.01f;
            newScrollBar.value += MoveBar;
        }

        if (NextPage)
            {
                isNextMove = false;
                if (Input.GetKeyDown(KeyCode.W))
                {
                    isNextMove = true;
                    wk = 1;

                    P1out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    P1in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    P2out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    P2in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    P3out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    P3in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    P4out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    P4in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    ReturnImg.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    Sideout.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    Sidein.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                    if (Control_SE.Get_Instance()) Control_SE.Get_Instance().Play_SE("UI_Select");
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    isNextMove = true;
                    wk = 2;

                    P1out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    P1in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    P2out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    P2in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    P3out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    P3in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    P4out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    P4in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    ReturnImg.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    Sideout.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    Sidein.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                    if (Control_SE.Get_Instance()) Control_SE.Get_Instance().Play_SE("UI_Select");
                }

                if (isNextMove)
                {
                    if (wk == 1)
                    {
                        P1mout.transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);
                        P1min.transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);
                        P2mout.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        P2min.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    }
                    if (wk == 2)
                    {
                        P1mout.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        P1min.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        P2mout.transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);
                        P2min.transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);
                    }
                }
            }



            //if (Input.GetKeyDown(KeyCode.Escape))    //戻るキー
            //{
            //    //text.text = "結果発表";
            //    //image1.enabled = true;
            //    //image2.enabled = true;
            //    //image3.enabled = true;
            //    //image4.enabled = true;
            //    //P1text.text = "Player1                                順位条件";
            //    //P2text.text = "Player2                                順位条件";
            //    //P3text.text = "Player3                                順位条件";
            //    //P4text.text = "Player4                                順位条件";
            //    //Top1.GetComponent<Image>().color = new Color32(50, 55, 19, 255);
            //    //Top2.GetComponent<Image>().color = new Color32(244, 255, 182, 255);
            //    //Back.GetComponent<Image>().color = new Color32(244, 255, 182, 255);

            //    P1out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            //    P1in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            //    P2out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            //    P2in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            //    P3out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            //    P3in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            //    P4out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            //    P4in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            //    ReturnImg.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            //    Sideout.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            //    Sidein.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            //    Choice = 5;
            //}
            //if (Input.GetKeyDown(KeyCode.Alpha6))    //タイトルキー
            //{
            //    P1out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            //    P1in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            //    P2out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            //    P2in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            //    P3out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            //    P3in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            //    P4out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            //    P4in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            //    ReturnImg.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            //    Sideout.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            //    Sidein.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            //    Choice = 6;
            //}

            if (Input.GetKeyDown(KeyCode.Return))    //決定キー
            {
                if(Control_SE.Get_Instance())Control_SE.Get_Instance().Play_SE("UI_Correct");
                if (wk == 0)
                {
                    if (_selectIndex == 0)
                    {
                        text.text = _characters[_selectIndex]._characterName;
                        image1.enabled = false;
                        image2.enabled = false;
                        image3.enabled = false;
                        image4.enabled = false;

                        _p1ItemText.text = "総資産";
                        _p2ItemText.text = "イベントマス使用回数";
                        _p3ItemText.text = "周回数";
                        _p4ItemText.text = "お土産数";

                        _p1NumText.text = _characters[_selectIndex]._money + "円";
                        _p2NumText.text = _characters[_selectIndex]._useEventNumByType.Sum() + "回";
                        _p3NumText.text = _characters[_selectIndex]._lapCount + "回";
                        _p4NumText.text = _characters[_selectIndex]._souvenirNum + "個";

                        Top1.GetComponent<Image>().color = new Color32(101, 21, 21, 255);
                        Top2.GetComponent<Image>().color = new Color32(185, 66, 66, 255);
                        Back.GetComponent<Image>().color = new Color32(185, 66, 66, 255);
                        P1out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        P1in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                        P1mout.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        P1min.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        P2mout.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        P2min.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                        Choice = 1;
                        NextPage = true;
                        ExPage1 = false;
                        ExPage2 = false;
                        //wk = 0;
                        DeleteGraph(); //グラフ消去
                    }
                    if (_selectIndex == 1)
                    {
                        text.text = _characters[_selectIndex]._characterName;
                        image1.enabled = false;
                        image2.enabled = false;
                        image3.enabled = false;
                        image4.enabled = false;

                    _p1ItemText.text = "総資産";
                    _p2ItemText.text = "イベントマス使用回数";
                    _p3ItemText.text = "周回数";
                    _p4ItemText.text = "お土産数";

                    _p1NumText.text = _characters[_selectIndex]._money + "円";
                    _p2NumText.text = _characters[_selectIndex]._useEventNumByType.Sum() + "回";
                    _p3NumText.text = _characters[_selectIndex]._lapCount + "回";
                    _p4NumText.text = _characters[_selectIndex]._souvenirNum + "個";

                    Top1.GetComponent<Image>().color = new Color32(21, 21, 101, 255);
                        Top2.GetComponent<Image>().color = new Color32(62, 115, 185, 255);
                        Back.GetComponent<Image>().color = new Color32(62, 115, 185, 255);
                        P2out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        P2in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                        P1mout.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        P1min.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        P2mout.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        P2min.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                        Choice = 2;
                        NextPage = true;
                        ExPage1 = false;
                        ExPage2 = false;
                        //wk = 0;
                        DeleteGraph(); //グラフ消去
                    }
                    if (_selectIndex == 2)
                    {
                        text.text = _characters[_selectIndex]._characterName;
                        image1.enabled = false;
                        image2.enabled = false;
                        image3.enabled = false;
                        image4.enabled = false;

                    _p1ItemText.text = "総資産";
                    _p2ItemText.text = "イベントマス使用回数";
                    _p3ItemText.text = "周回数";
                    _p4ItemText.text = "お土産数";

                    _p1NumText.text = _characters[_selectIndex]._money + "円";
                    _p2NumText.text = _characters[_selectIndex]._useEventNumByType.Sum() + "回";
                    _p3NumText.text = _characters[_selectIndex]._lapCount + "回";
                    _p4NumText.text = _characters[_selectIndex]._souvenirNum + "個";

                    Top1.GetComponent<Image>().color = new Color32(11, 60, 11, 255);
                        Top2.GetComponent<Image>().color = new Color32(52, 180, 105, 255);
                        Back.GetComponent<Image>().color = new Color32(52, 180, 105, 255);
                        P3out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        P3in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                        P1mout.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        P1min.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        P2mout.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        P2min.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                        Choice = 3;
                        NextPage = true;
                        ExPage1 = false;
                        ExPage2 = false;
                        //wk = 0;
                        DeleteGraph(); //グラフ消去
                    }
                    if (_selectIndex == 3)
                    {
                        text.text = _characters[_selectIndex]._characterName;
                        image1.enabled = false;
                        image2.enabled = false;
                        image3.enabled = false;
                        image4.enabled = false;

                    _p1ItemText.text = "総資産";
                    _p2ItemText.text = "イベントマス使用回数";
                    _p3ItemText.text = "周回数";
                    _p4ItemText.text = "お土産数";

                    _p1NumText.text = _characters[_selectIndex]._money + "円";
                    _p2NumText.text = _characters[_selectIndex]._useEventNumByType.Sum() + "回";
                    _p3NumText.text = _characters[_selectIndex]._lapCount + "回";
                    _p4NumText.text = _characters[_selectIndex]._souvenirNum + "個";

                    Top1.GetComponent<Image>().color = new Color32(101, 71, 21, 255);
                        Top2.GetComponent<Image>().color = new Color32(195, 195, 55, 255);
                        Back.GetComponent<Image>().color = new Color32(195, 195, 55, 255);
                        P4out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        P4in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                        P1mout.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        P1min.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        P2mout.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        P2min.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                        Choice = 4;
                        NextPage = true;
                        ExPage1 = false;
                        ExPage2 = false;
                        //wk = 0;
                        DeleteGraph(); //グラフ消去
                    }
                }
                if (wk == 1) //W
                {
                    NextPage = false;
                    ExPage1 = true;
                    if (Choice == 1)
                    {
                        //バグ用
                        P1mout.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        P1min.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        P2mout.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        P2min.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                        ShowGraph(_characters[_selectIndex]._moneyByTurn);  //グラフ表示
                    }
                    if (Choice == 2)
                    {
                        //バグ用
                        P1mout.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        P1min.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        P2mout.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        P2min.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                    ShowGraph(_characters[_selectIndex]._moneyByTurn);  //グラフ表示
                }
                    if (Choice == 3)
                    {
                        //バグ用
                        P1mout.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        P1min.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        P2mout.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        P2min.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                    ShowGraph(_characters[_selectIndex]._moneyByTurn);  //グラフ表示
                }
                    if (Choice == 4)
                    {
                        //バグ用
                        P1mout.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        P1min.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        P2mout.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        P2min.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                    ShowGraph(_characters[_selectIndex]._moneyByTurn);  //グラフ表示
                }

                }
                if (wk == 2) //S
                {
                    NextPage = false;
                    ExPage2 = true;
                    if (Choice == 1)
                    {
                    EX1text.text = "カジノマス";
                    EX1text2.text = _characters[_selectIndex]._useEventNumByType[0] + "回";
                    EX2text.text = "強制ゴールマス";
                    EX2text2.text = _characters[_selectIndex]._useEventNumByType[1] + "回";
                    EX3text.text = "進むマス";
                    EX3text2.text = _characters[_selectIndex]._useEventNumByType[2] + "回";
                    EX4text.text = "防御マス";
                    EX4text2.text = _characters[_selectIndex]._useEventNumByType[3] + "回";
                    EX5text.text = "いただきマス";
                    EX5text2.text = _characters[_selectIndex]._useEventNumByType[4] + "回";
                    EX6text.text = "全員ワープマス";
                    EX6text2.text = _characters[_selectIndex]._useEventNumByType[5] + "回";

                    //EX7text.text = "ゴールマス";
                    //EX7text2.text = _characters[_selectIndex]._useEventNumByType[0] + "回";
                    //EX8text.text = "おこずかいマス";
                    //EX8text2.text = _characters[_selectIndex]._useEventNumByType[0] + "回";
                }
                    if (Choice == 2)
                    {
                    EX1text.text = "カジノマス";
                    EX1text2.text = _characters[_selectIndex]._useEventNumByType[0] + "回";
                    EX2text.text = "強制ゴールマス";
                    EX2text2.text = _characters[_selectIndex]._useEventNumByType[1] + "回";
                    EX3text.text = "進むマス";
                    EX3text2.text = _characters[_selectIndex]._useEventNumByType[2] + "回";
                    EX4text.text = "防御マス";
                    EX4text2.text = _characters[_selectIndex]._useEventNumByType[3] + "回";
                    EX5text.text = "いただきマス";
                    EX5text2.text = _characters[_selectIndex]._useEventNumByType[4] + "回";
                    EX6text.text = "全員ワープマス";
                    EX6text2.text = _characters[_selectIndex]._useEventNumByType[5] + "回";
                    //EX7text.text = "ゴールマス";
                    //EX7text2.text = _characters[_selectIndex]._useEventNumByType[0] + "回";
                    //EX8text.text = "おこずかいマス";
                    //EX8text2.text = _characters[_selectIndex]._useEventNumByType[0] + "回";
                }
                    if (Choice == 3)
                    {
                    EX1text.text = "カジノマス";
                    EX1text2.text = _characters[_selectIndex]._useEventNumByType[0] + "回";
                    EX2text.text = "強制ゴールマス";
                    EX2text2.text = _characters[_selectIndex]._useEventNumByType[1] + "回";
                    EX3text.text = "進むマス";
                    EX3text2.text = _characters[_selectIndex]._useEventNumByType[2] + "回";
                    EX4text.text = "防御マス";
                    EX4text2.text = _characters[_selectIndex]._useEventNumByType[3] + "回";
                    EX5text.text = "いただきマス";
                    EX5text2.text = _characters[_selectIndex]._useEventNumByType[4] + "回";
                    EX6text.text = "全員ワープマス";
                    EX6text2.text = _characters[_selectIndex]._useEventNumByType[5] + "回";
                    //EX7text.text = "ゴールマス";
                    //EX7text2.text = _characters[_selectIndex]._useEventNumByType[0] + "回";
                    //EX8text.text = "おこずかいマス";
                    //EX8text2.text = _characters[_selectIndex]._useEventNumByType[0] + "回";
                }
                    if (Choice == 4)
                    {
                    EX1text.text = "カジノマス";
                    EX1text2.text = _characters[_selectIndex]._useEventNumByType[0] + "回";
                    EX2text.text = "強制ゴールマス";
                    EX2text2.text = _characters[_selectIndex]._useEventNumByType[1] + "回";
                    EX3text.text = "進むマス";
                    EX3text2.text = _characters[_selectIndex]._useEventNumByType[2] + "回";
                    EX4text.text = "防御マス";
                    EX4text2.text = _characters[_selectIndex]._useEventNumByType[3] + "回";
                    EX5text.text = "いただきマス";
                    EX5text2.text = _characters[_selectIndex]._useEventNumByType[4] + "回";
                    EX6text.text = "全員ワープマス";
                    EX6text2.text = _characters[_selectIndex]._useEventNumByType[5] + "回";
                    //EX7text.text = "ゴールマス";
                    //EX7text2.text = _characters[_selectIndex]._useEventNumByType[0] + "回";
                    //EX8text.text = "おこずかいマス";
                    //EX8text2.text = _characters[_selectIndex]._useEventNumByType[0] + "回";
                    }
                }


                //if (Choice == 5)
                //{
                //    text.text = "結果発表";
                //    image1.enabled = true;
                //    image2.enabled = true;
                //    image3.enabled = true;
                //    image4.enabled = true;
                //    //P1text.text = "Player1                                順位条件";
                //    //P2text.text = "Player2                                順位条件";
                //    //P3text.text = "Player3                                順位条件";
                //    //P4text.text = "Player4                                順位条件";
                //    Top1.GetComponent<Image>().color = new Color32(50, 55, 19, 255);
                //    Top2.GetComponent<Image>().color = new Color32(244, 255, 182, 255);
                //    Back.GetComponent<Image>().color = new Color32(244, 255, 182, 255);
                //    ReturnImg.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                //    //Choice = 0;
                //}
                //else if (Choice == 6)
                //{
                //    //タイトルに戻す
                //    //ChangeScene
                //    //Choice = 0;
                //}
            }
     }
   }
