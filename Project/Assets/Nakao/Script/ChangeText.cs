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
            P1text.text = "•\¦‡@";
            P2text.text = "•\¦‡A";
            P3text.text = "•\¦‡B";
            P4text.text = "•\¦‡C";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))    //P2
        {
            text.text = "Player2";
            image1.enabled = false;
            image2.enabled = false;
            image3.enabled = false;
            image4.enabled = false;
            P1text.text = "•\¦‡@";
            P2text.text = "•\¦‡A";
            P3text.text = "•\¦‡B";
            P4text.text = "•\¦‡C";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))    //P3
        {
            text.text = "Player3";
            image1.enabled = false;
            image2.enabled = false;
            image3.enabled = false;
            image4.enabled = false;
            P1text.text = "•\¦‡@";
            P2text.text = "•\¦‡A";
            P3text.text = "•\¦‡B";
            P4text.text = "•\¦‡C";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))    //P4
        {
            text.text = "Player4";
            image1.enabled = false;
            image2.enabled = false;
            image3.enabled = false;
            image4.enabled = false;
            P1text.text = "•\¦‡@";
            P2text.text = "•\¦‡A";
            P3text.text = "•\¦‡B";
            P4text.text = "•\¦‡C";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))    //–ß‚éƒL[
        {
            text.text = "Œ‹‰Ê”­•\";
            image1.enabled = true;
            image2.enabled = true;
            image3.enabled = true;
            image4.enabled = true;
            P1text.text = "Player1                                ‡ˆÊğŒ";
            P2text.text = "Player2                                ‡ˆÊğŒ";
            P3text.text = "Player3                                ‡ˆÊğŒ";
            P4text.text = "Player4                                ‡ˆÊğŒ";
        }
    }
}