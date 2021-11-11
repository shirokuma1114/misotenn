using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonChangeP2Text : MonoBehaviour
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
        text.text = "Player2";
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

        Top1.GetComponent<Image>().color = new Color32(21, 21, 101, 255);
        Top2.GetComponent<Image>().color = new Color32(62, 115, 185, 255);
        Back.GetComponent<Image>().color = new Color32(62, 115, 185, 255);

        script.mn = false;
        obj.GetComponent<Button>().interactable = true;
        obj2.GetComponent<Button>().interactable = true;

        script2.mn = false;
        obj3.GetComponent<Button>().interactable = true;
        obj4.GetComponent<Button>().interactable = true;
    }
}
