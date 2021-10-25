using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonChangeP1Text : MonoBehaviour
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

    // ボタンが押された場合、今回呼び出される関数
    public void OnClick()
    {
        text.text = "Player1";
        image1.enabled = false;
        image2.enabled = false;
        image3.enabled = false;
        image4.enabled = false;
        P1text.text = "総資産                                ＄";
        P2text.text = "周回数                                回";
        P3text.text = "イベント踏んだ数               回";
        P4text.text = "お土産数                             個";
        Top1.GetComponent<Image>().color = new Color32(101, 21, 21, 255);
        Top2.GetComponent<Image>().color = new Color32(185, 66, 66, 255);
        Back.GetComponent<Image>().color = new Color32(185, 66, 66, 255);
    }
}
