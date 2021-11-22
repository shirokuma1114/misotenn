using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonChangeP4Text : MonoBehaviour
{
    public Text text;

    public Image image1;
    public Image image2;
    public Image image3;
    public Image image4;

    public Text P1text;
    public Text P2text;
    public Text P3text;
    public Text P4text;

    public Image Top1;
    public Image Top2;
    public Image Back;

    public Image P1out;
    public Image P2out;
    public Image P3out;
    public Image P4out;
    public Image P1in;
    public Image P2in;
    public Image P3in;
    public Image P4in;

    public bool TriggerButton;

    //private float Timer;
    //private float StopTimer = 0.5f;

    public int Number = 4;

    GameObject obj;
    AllIvent script;
    GameObject obj2;

    GameObject obj3;
    AllMoney script2;
    GameObject obj4;

    //キーボード用
    GameObject obj5;
    ButtonChangeP1Text script3;

    GameObject obj6;
    ButtonChangeP2Text script4;

    GameObject obj7;
    ButtonChangeP3Text script5;

    GameObject obj8;
    ButtonChangeBackText script6;

    // Start is called before the first frame update
    void Start()
    {
        //MenuNumber1
        obj = GameObject.Find("1MainInFrame");
        script = obj.GetComponent<AllIvent>();
        obj2 = GameObject.Find("1MainOutFrame");

        //MenuNumber2
        obj3 = GameObject.Find("3MainInFrame");
        script2 = obj.GetComponent<AllMoney>();
        if (script2 == null)
        {
            script2 = obj3.GetComponent<AllMoney>();
        }
        obj4 = GameObject.Find("3MainOutFrame");

        //KeyBord
        obj5 = GameObject.Find("1SelectInFrame");
        script3 = obj5.GetComponent<ButtonChangeP1Text>();
        if (script3 == null)
        {
            script3 = obj5.GetComponent<ButtonChangeP1Text>();
        }

        obj6 = GameObject.Find("2SelectInFrame");
        script4 = obj6.GetComponent<ButtonChangeP2Text>();
        if (script4 == null)
        {
            script4 = obj6.GetComponent<ButtonChangeP2Text>();
        }

        obj7 = GameObject.Find("3SelectInFrame");
        script5 = obj7.GetComponent<ButtonChangeP3Text>();
        if (script5 == null)
        {
            script5 = obj7.GetComponent<ButtonChangeP3Text>();
        }

        obj8 = GameObject.Find("BackKey");
        script6 = obj8.GetComponent<ButtonChangeBackText>();
        if (script6 == null)
        {
            script6 = obj8.GetComponent<ButtonChangeBackText>();
        }

        TriggerButton = false;
        //Timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (TriggerButton)
        {
            OnClick();
            //TriggerButton = false;
        }

        //if (TriggerButton && !script6.TriggerButton)
        //{
        //    if (Input.GetKeyDown(KeyCode.D))
        //    {
        //        script6.TriggerButton = true;
        //        TriggerButton = false;
        //    }
        //}

        //if (TriggerButton && !script5.TriggerButton)
        //{
        //    if (Input.GetKeyDown(KeyCode.A))
        //    {
        //        script5.TriggerButton = true;
        //        TriggerButton = false;
        //    }
        //}

        //if (script5.TriggerButton)
        //{
        //    Timer += Time.deltaTime;
        //    if (Timer < StopTimer) return;
        //    if (Input.GetKeyDown(KeyCode.D))
        //    {
        //        OnClick();
        //        Timer = 0;
        //    }
        //}

        //フレーム全消し
        if (script.mn)
        {
            P1text.enabled = false;
            P2text.enabled = false;
            P3text.enabled = false;
            P4text.enabled = false;
            P1out.enabled = false;
            P2out.enabled = false;
            P3out.enabled = false;
            P4out.enabled = false;
            P1in.enabled = false;
            P2in.enabled = false;
            P3in.enabled = false;
            P4in.enabled = false;
        }

        if (script2.mn)
        {
            P1text.enabled = false;
            P2text.enabled = false;
            P3text.enabled = false;
            P4text.enabled = false;
            P1out.enabled = false;
            P2out.enabled = false;
            P3out.enabled = false;
            P4out.enabled = false;
            P1in.enabled = false;
            P2in.enabled = false;
            P3in.enabled = false;
            P4in.enabled = false;
        }
    }

    // ボタンが押された場合、今回呼び出される関数
    public void OnClick()
    {
        text.text = "Player4";
        image1.enabled = false;
        image2.enabled = false;
        image3.enabled = false;
        image4.enabled = false;
        P1text.enabled = true;
        P2text.enabled = true;
        P3text.enabled = true;
        P4text.enabled = true;
        P1out.enabled = true;
        P2out.enabled = true;
        P3out.enabled = true;
        P4out.enabled = true;
        P1in.enabled = true;
        P2in.enabled = true;
        P3in.enabled = true;
        P4in.enabled = true;

        P1text.text = "イベント踏んだ数               回";
        P2text.text = "周回数                                回";
        P3text.text = "総資産                                ＄";
        P4text.text = "お土産数                             個";

        Top1.GetComponent<Image>().color = new Color32(101, 71, 21, 255);
        Top2.GetComponent<Image>().color = new Color32(195, 195, 55, 255);
        Back.GetComponent<Image>().color = new Color32(195, 195, 55, 255);

        script.mn = false;
        obj.GetComponent<Button>().interactable = true;
        obj2.GetComponent<Button>().interactable = true;

        script2.mn = false;
        obj3.GetComponent<Button>().interactable = true;
        obj4.GetComponent<Button>().interactable = true;

        //1が押された
        //TriggerButton = true;
        //script3.TriggerButton = false;
        //script4.TriggerButton = false;
        //script5.TriggerButton = false;
        //script6.TriggerButton = false;
    }
}
