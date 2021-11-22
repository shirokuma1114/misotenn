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

    GameObject obj;
    AllIvent script;
    GameObject obj2;

    GameObject obj3;
    AllMoney script2;
    GameObject obj4;

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
    }

    // Update is called once per frame
    void Update()
    {
        //�t���[���S����
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

    // �{�^���������ꂽ�ꍇ�A����Ăяo�����֐�
    public void OnClick()
    {
        text.text = "���ʔ��\";
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
        P1text.text = "Player1                                ���ʏ���";
        P2text.text = "Player2                                ���ʏ���";
        P3text.text = "Player3                                ���ʏ���";
        P4text.text = "Player4                                ���ʏ���";
        Top1.GetComponent<Image>().color = new Color32(50, 55, 19, 255);
        Top2.GetComponent<Image>().color = new Color32(244, 255, 182, 255);
        Back.GetComponent<Image>().color = new Color32(244, 255, 182, 255);

        script.mn = false;
        obj.GetComponent<Button>().interactable = false;
        obj2.GetComponent<Button>().interactable = false;

        script2.mn = false;
        obj3.GetComponent<Button>().interactable = false;
        obj4.GetComponent<Button>().interactable = false;
    
    }
}