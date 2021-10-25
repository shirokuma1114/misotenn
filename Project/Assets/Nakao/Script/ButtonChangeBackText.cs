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
        text.text = "結果発表";
        image1.enabled = true;
        image2.enabled = true;
        image3.enabled = true;
        image4.enabled = true;
        P1text.text = "Player1                                順位条件";
        P2text.text = "Player2                                順位条件";
        P3text.text = "Player3                                順位条件";
        P4text.text = "Player4                                順位条件";
        Top1.GetComponent<Image>().color = new Color32(50, 55, 19, 255);
        Top2.GetComponent<Image>().color = new Color32(244, 255, 182, 255);
        Back.GetComponent<Image>().color = new Color32(244, 255, 182, 255);
    }
}
