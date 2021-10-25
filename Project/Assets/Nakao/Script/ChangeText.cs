using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeText : MonoBehaviour
{
    //text
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
    public Image P1in;
    public Image P2out;
    public Image P2in;
    public Image P3out;
    public Image P3in;
    public Image P4out;
    public Image P4in;
    public Image ReturnImg;
    public Image Sideout;
    public Image Sidein;

    //����Key�p
    int Choice = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))         //P1
        {
            //text.text = "Player1";
            //image1.enabled = false;
            //image2.enabled = false;
            //image3.enabled = false;
            //image4.enabled = false;
            //P1text.text = "�����Y                                ��";
            //P2text.text = "����                                ��";
            //P3text.text = "�C�x���g���񂾐�               ��";
            //P4text.text = "���y�Y��                             ��";
            //Top1.GetComponent<Image>().color = new Color32(101, 21, 21, 255);
            //Top2.GetComponent<Image>().color = new Color32(185, 66, 66, 255);
            //Back.GetComponent<Image>().color = new Color32(185, 66, 66, 255);

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
            Choice = 1;

        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))    //P2
        {
            //text.text = "Player2";
            //image1.enabled = false;
            //image2.enabled = false;
            //image3.enabled = false;
            //image4.enabled = false;
            //P1text.text = "�����Y                                ��";
            //P2text.text = "����                                ��";
            //P3text.text = "�C�x���g���񂾐�               ��";
            //P4text.text = "���y�Y��                             ��";
            //Top1.GetComponent<Image>().color = new Color32(21, 21, 101, 255);
            //Top2.GetComponent<Image>().color = new Color32(62, 115, 185, 255);
            //Back.GetComponent<Image>().color = new Color32(62, 115, 185, 255);

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
            Choice = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))    //P3
        {
            //text.text = "Player3";
            //image1.enabled = false;
            //image2.enabled = false;
            //image3.enabled = false;
            //image4.enabled = false;
            //P1text.text = "�����Y                                ��";
            //P2text.text = "����                                ��";
            //P3text.text = "�C�x���g���񂾐�               ��";
            //P4text.text = "���y�Y��                             ��";
            //Top1.GetComponent<Image>().color = new Color32(11, 60, 11, 255);
            //Top2.GetComponent<Image>().color = new Color32(52, 180, 105, 255);
            //Back.GetComponent<Image>().color = new Color32(52, 180, 105, 255);

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
            Choice = 3;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))    //P4
        {
            //text.text = "Player4";
            //image1.enabled = false;
            //image2.enabled = false;
            //image3.enabled = false;
            //image4.enabled = false;
            //P1text.text = "�����Y                                ��";
            //P2text.text = "����                                ��";
            //P3text.text = "�C�x���g���񂾐�               ��";
            //P4text.text = "���y�Y��                             ��";
            //Top1.GetComponent<Image>().color = new Color32(101, 71, 21, 255);
            //Top2.GetComponent<Image>().color = new Color32(195, 195, 55, 255);
            //Back.GetComponent<Image>().color = new Color32(195, 195, 55, 255);

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
            Choice = 4;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))    //�߂�L�[
        {
            //text.text = "���ʔ��\";
            //image1.enabled = true;
            //image2.enabled = true;
            //image3.enabled = true;
            //image4.enabled = true;
            //P1text.text = "Player1                                ���ʏ���";
            //P2text.text = "Player2                                ���ʏ���";
            //P3text.text = "Player3                                ���ʏ���";
            //P4text.text = "Player4                                ���ʏ���";
            //Top1.GetComponent<Image>().color = new Color32(50, 55, 19, 255);
            //Top2.GetComponent<Image>().color = new Color32(244, 255, 182, 255);
            //Back.GetComponent<Image>().color = new Color32(244, 255, 182, 255);

            P1out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            P1in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            P2out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            P2in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            P3out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            P3in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            P4out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            P4in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            ReturnImg.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            Sideout.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            Sidein.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            Choice = 5;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))    //�^�C�g���L�[
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
            Sideout.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            Sidein.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            Choice = 6;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))    //����L�[
        {
            if (Choice == 1)
            {
                text.text = "Player1";
                image1.enabled = false;
                image2.enabled = false;
                image3.enabled = false;
                image4.enabled = false;
                P1text.text = "�����Y                                ��";
                P2text.text = "����                                ��";
                P3text.text = "�C�x���g���񂾐�               ��";
                P4text.text = "���y�Y��                             ��";
                Top1.GetComponent<Image>().color = new Color32(101, 21, 21, 255);
                Top2.GetComponent<Image>().color = new Color32(185, 66, 66, 255);
                Back.GetComponent<Image>().color = new Color32(185, 66, 66, 255);
                P1out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                P1in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                //Choice = 0;
            }
            else if(Choice == 2)
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
                P2out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                P2in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                //Choice = 0;
            }
            else if (Choice == 3)
            {
                text.text = "Player3";
                image1.enabled = false;
                image2.enabled = false;
                image3.enabled = false;
                image4.enabled = false;
                P1text.text = "�����Y                                ��";
                P2text.text = "����                                ��";
                P3text.text = "�C�x���g���񂾐�               ��";
                P4text.text = "���y�Y��                             ��";
                Top1.GetComponent<Image>().color = new Color32(11, 60, 11, 255);
                Top2.GetComponent<Image>().color = new Color32(52, 180, 105, 255);
                Back.GetComponent<Image>().color = new Color32(52, 180, 105, 255);
                P3out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                P3in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                //Choice = 0;
            }
            else if (Choice == 4)
            {
                text.text = "Player4";
                image1.enabled = false;
                image2.enabled = false;
                image3.enabled = false;
                image4.enabled = false;
                P1text.text = "�����Y                                ��";
                P2text.text = "����                                ��";
                P3text.text = "�C�x���g���񂾐�               ��";
                P4text.text = "���y�Y��                             ��";
                Top1.GetComponent<Image>().color = new Color32(101, 71, 21, 255);
                Top2.GetComponent<Image>().color = new Color32(195, 195, 55, 255);
                Back.GetComponent<Image>().color = new Color32(195, 195, 55, 255);
                P4out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                P4in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                //Choice = 0;
            }
            else if (Choice == 5)
            {
                text.text = "���ʔ��\";
                image1.enabled = true;
                image2.enabled = true;
                image3.enabled = true;
                image4.enabled = true;
                P1text.text = "Player1                                ���ʏ���";
                P2text.text = "Player2                                ���ʏ���";
                P3text.text = "Player3                                ���ʏ���";
                P4text.text = "Player4                                ���ʏ���";
                Top1.GetComponent<Image>().color = new Color32(50, 55, 19, 255);
                Top2.GetComponent<Image>().color = new Color32(244, 255, 182, 255);
                Back.GetComponent<Image>().color = new Color32(244, 255, 182, 255);
                ReturnImg.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                //Choice = 0;
            }
            else if (Choice == 6)
            {
                //�^�C�g���ɖ߂�
                //ChangeScene
                //Choice = 0;
            }
        }
    }
}