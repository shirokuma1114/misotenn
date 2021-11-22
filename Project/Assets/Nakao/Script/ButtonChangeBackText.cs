using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonChangeBackText : MonoBehaviour
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

    public int Number = 5;

    GameObject obj;
    AllIvent script;
    GameObject obj2;

    GameObject obj3;
    AllMoney script2;
    GameObject obj4;

    GameObject obj5;
    ButtonChangeP1Text script3;

    GameObject obj6;
    ButtonChangeP2Text script4;

    GameObject obj7;
    ButtonChangeP3Text script5;

    GameObject obj8;
    ButtonChangeP4Text script6;


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

        obj8 = GameObject.Find("4SelectInFrame");
        script6 = obj8.GetComponent<ButtonChangeP4Text>();
        if (script6 == null)
        {
            script6 = obj8.GetComponent<ButtonChangeP4Text>();
        }

        TriggerButton = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (TriggerButton)
        {
            OnClick();
            TriggerButton = false;
        }

        //if (TriggerButton && !script6.TriggerButton)
        //{
        //    if (Input.GetKeyDown(KeyCode.D))
        //    {
        //        script6.TriggerButton = true;
        //        TriggerButton = false;
        //    }
        //}

        //if (!TriggerButton && !script3.TriggerButton)
        //{
        //    if (Input.GetKeyDown(KeyCode.A))
        //    {
        //        script3.TriggerButton = true;
        //        TriggerButton = false;
        //    }
        //}
        

        //if(script6.TriggerButton)
        //{
        //    if (Input.GetKeyDown(KeyCode.D))
        //    {
        //        OnClick();
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
        text.text = "結果発表";
        image1.enabled = true;
        image2.enabled = true;
        image3.enabled = true;
        image4.enabled = true;
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
        P1text.text = "Player1                                順位条件";
        P2text.text = "Player2                                順位条件";
        P3text.text = "Player3                                順位条件";
        P4text.text = "Player4                                順位条件";
        Top1.GetComponent<Image>().color = new Color32(50, 55, 19, 255);
        Top2.GetComponent<Image>().color = new Color32(244, 255, 182, 255);
        Back.GetComponent<Image>().color = new Color32(244, 255, 182, 255);

        script.mn = false;
        obj.GetComponent<Button>().interactable = false;
        obj2.GetComponent<Button>().interactable = false;

        script2.mn = false;
        obj3.GetComponent<Button>().interactable = false;
        obj4.GetComponent<Button>().interactable = false;

        //TriggerButton = true;
        //script3.TriggerButton = false;
        //script4.TriggerButton = false;
        //script5.TriggerButton = false;
        //script6.TriggerButton = false;
    }
}
