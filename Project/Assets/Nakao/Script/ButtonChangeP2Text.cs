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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // �{�^���������ꂽ�ꍇ�A����Ăяo�����֐�
    public void OnClick()
    {
        text.text = "Player2";
        image1.enabled = false;
        image2.enabled = false;
        image3.enabled = false;
        image4.enabled = false;
        P1text.text = "�����Y                                ��";
        P2text.text = "����                                ��";
        P3text.text = "�C�x���g���񂾐�               ��";
        P4text.text = "���y�Y��                             ��";
        Top1.GetComponent<Image>().color = new Color32(21, 21, 101, 255);
        Top2.GetComponent<Image>().color = new Color32(62, 115, 185, 255);
        Back.GetComponent<Image>().color = new Color32(62, 115, 185, 255);
    }
}
