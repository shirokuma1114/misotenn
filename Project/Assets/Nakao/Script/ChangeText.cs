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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))         //P1
        {
            text.text = "Player1";
            image1.enabled = false;
            image2.enabled = false;
            image3.enabled = false;
            image4.enabled = false;
            P1text.text = "ï\é¶á@";
            P2text.text = "ï\é¶áA";
            P3text.text = "ï\é¶áB";
            P4text.text = "ï\é¶áC";
            Top1.GetComponent<Image>().color = new Color32(101, 21, 21, 255);
            Top2.GetComponent<Image>().color = new Color32(185, 66, 66, 255);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))    //P2
        {
            text.text = "Player2";
            image1.enabled = false;
            image2.enabled = false;
            image3.enabled = false;
            image4.enabled = false;
            P1text.text = "ï\é¶á@";
            P2text.text = "ï\é¶áA";
            P3text.text = "ï\é¶áB";
            P4text.text = "ï\é¶áC";
            Top1.GetComponent<Image>().color = new Color32(21, 21, 101, 255);
            Top2.GetComponent<Image>().color = new Color32(62, 115, 185, 255);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))    //P3
        {
            text.text = "Player3";
            image1.enabled = false;
            image2.enabled = false;
            image3.enabled = false;
            image4.enabled = false;
            P1text.text = "ï\é¶á@";
            P2text.text = "ï\é¶áA";
            P3text.text = "ï\é¶áB";
            P4text.text = "ï\é¶áC";
            Top1.GetComponent<Image>().color = new Color32(11, 60, 11, 255);
            Top2.GetComponent<Image>().color = new Color32(52, 180, 105, 255);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))    //P4
        {
            text.text = "Player4";
            image1.enabled = false;
            image2.enabled = false;
            image3.enabled = false;
            image4.enabled = false;
            P1text.text = "ï\é¶á@";
            P2text.text = "ï\é¶áA";
            P3text.text = "ï\é¶áB";
            P4text.text = "ï\é¶áC";
            Top1.GetComponent<Image>().color = new Color32(101, 71, 21, 255);
            Top2.GetComponent<Image>().color = new Color32(195, 195, 55, 255);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))    //ñﬂÇÈÉLÅ[
        {
            text.text = "åãâ î≠ï\";
            image1.enabled = true;
            image2.enabled = true;
            image3.enabled = true;
            image4.enabled = true;
            P1text.text = "Player1                                èáà èåè";
            P2text.text = "Player2                                èáà èåè";
            P3text.text = "Player3                                èáà èåè";
            P4text.text = "Player4                                èáà èåè";
            Top1.GetComponent<Image>().color = new Color32(50, 55, 19, 255);
            Top2.GetComponent<Image>().color = new Color32(244, 255, 182, 255);
        }
    }
}