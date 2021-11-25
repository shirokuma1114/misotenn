using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;


public class ChangeText : MonoBehaviour
{
    //text
    public Text text;
    public Image image1;
    public Image image2;
    public Image image3;
    public Image image4;

    [SerializeField]
    private Text _p1ItemText;
    [SerializeField]
    private Text _p2ItemText;
    [SerializeField]
    private Text _p3ItemText;
    [SerializeField]
    private Text _p4ItemText;

    [SerializeField]
    private Text _p1NumText;
    [SerializeField]
    private Text _p2NumText;
    [SerializeField]
    private Text _p3NumText;
    [SerializeField]
    private Text _p4NumText;

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

    [SerializeField]
    private List<Text> _selectTexts;


    //決定Key用
    int Choice = 0;

    int _selectIndex;

    CharacterData[] _characters;

    // Start is called before the first frame update
    void Start()
    {
        InitCharacterInfo();
    
        ShowRank();
        {
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
        }
    }

    private void InitCharacterInfo()
    {
        

        var manager = FindObjectOfType<DontDestroyManager>();
        _characters = new CharacterData[4];


        if (manager)
        {
            var charaOriginData = manager.GetCharacterData();
            for (int i = 0; i < 4; i++)
            {
                _characters[i] = new CharacterData();
                _characters[i]._characterName = charaOriginData[i]._characterName;
                _characters[i]._lapCount = charaOriginData[i]._lapCount;
                _characters[i]._money = charaOriginData[i]._money;
                _characters[i]._useEventNumByType = new int[(int)SquareEventType.EVENT_TYPE_MAX];
                Array.Copy(charaOriginData[i]._character.Log.GetUseEventNum(), _characters[i]._useEventNumByType, _characters[i]._useEventNumByType.Length);
                _characters[i]._moneyByTurn = new List<int>();
                foreach (var x in charaOriginData[i]._character.Log.GetMoneyByTurn())
                {
                    _characters[i]._moneyByTurn.Add(x);
                }
                _characters[i]._rank = charaOriginData[i]._rank;
                _characters[i]._souvenirNum = charaOriginData[i]._souvenirNum;
                _selectTexts[i].text = _characters[i]._characterName;
            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                _characters[i] = new CharacterData();
                _characters[i]._characterName = "ああ" + i;
                _characters[i]._lapCount = 0;
                _characters[i]._money = 999;
                _characters[i]._useEventNumByType = new int[(int)SquareEventType.EVENT_TYPE_MAX];
                _characters[i]._moneyByTurn = new List<int>();
                _characters[i]._moneyByTurn.Add(999);
                _characters[i]._rank = i;
                _characters[i]._souvenirNum = 0;
                _selectTexts[i].text = _characters[i]._characterName;
            }
        }
    }

    void ShowRank()
    {
        var rankSortedCharacters = _characters;
        rankSortedCharacters = _characters.OrderByDescending(x => x._souvenirNum).ThenByDescending(x => x._money).ToArray();
        
        for(int i = 0; i < rankSortedCharacters.Count(); i++)
        {
            if(i == 0)
            {
                _p1ItemText.text = rankSortedCharacters[i]._characterName;
                _p1NumText.text = "";
            }
            if(i == 1)
            {
                _p2ItemText.text = rankSortedCharacters[i]._characterName;
                _p2NumText.text = "";
            }
            if (i == 2)
            {
                _p3ItemText.text = rankSortedCharacters[i]._characterName;
                _p3NumText.text = "";
            }
            if (i == 3)
            {
                _p4ItemText.text = rankSortedCharacters[i]._characterName;
                _p4NumText.text = "";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        bool isMove = false;
        if (Input.GetKeyDown(KeyCode.A))
        {
            _selectIndex = Mathf.Max(--_selectIndex, 0);
            isMove = true;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            _selectIndex = Mathf.Min(++_selectIndex, _characters.Length);
            isMove = true;
        }

        if (isMove)
        {
            if(_selectIndex == 0)
            {
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
            }
            if(_selectIndex == 1)
            {
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
            }
            if(_selectIndex == 2)
            {
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
            }
            if(_selectIndex == 3)
            {
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
            }

        }

        if (Input.GetKeyDown(KeyCode.Escape))    //戻るキー
        {
            //text.text = "結果発表";
            //image1.enabled = true;
            //image2.enabled = true;
            //image3.enabled = true;
            //image4.enabled = true;
            //P1text.text = "Player1                                順位条件";
            //P2text.text = "Player2                                順位条件";
            //P3text.text = "Player3                                順位条件";
            //P4text.text = "Player4                                順位条件";
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
        if (Input.GetKeyDown(KeyCode.Alpha6))    //タイトルキー
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
        if (Input.GetKeyDown(KeyCode.Return))    //決定キー
        {
            if (_selectIndex == 0)
            {
                text.text = _characters[_selectIndex]._characterName;
                image1.enabled = false;
                image2.enabled = false;
                image3.enabled = false;
                image4.enabled = false;

                _p1ItemText.text = "総資産";
                _p2ItemText.text = "周回数";
                _p3ItemText.text = "イベントマス使用回数";
                _p4ItemText.text = "お土産数";

                _p1NumText.text = _characters[_selectIndex]._money + "＄";
                _p2NumText.text = _characters[_selectIndex]._lapCount + "回";
                _p3NumText.text = _characters[_selectIndex]._useEventNumByType.Sum() + "回";
                _p4NumText.text = _characters[_selectIndex]._souvenirNum + "個";
                
                Top1.GetComponent<Image>().color = new Color32(101, 21, 21, 255);
                Top2.GetComponent<Image>().color = new Color32(185, 66, 66, 255);
                Back.GetComponent<Image>().color = new Color32(185, 66, 66, 255);
                P1out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                P1in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                //Choice = 0;
            }
            if(_selectIndex == 1)
            {
                text.text = _characters[_selectIndex]._characterName;
                image1.enabled = false;
                image2.enabled = false;
                image3.enabled = false;
                image4.enabled = false;
                _p1ItemText.text = "総資産";
                _p2ItemText.text = "周回数";
                _p3ItemText.text = "イベントマス使用回数";
                _p4ItemText.text = "お土産数";

                _p1NumText.text = _characters[_selectIndex]._money + "＄";
                _p2NumText.text = _characters[_selectIndex]._lapCount + "回";
                _p3NumText.text = _characters[_selectIndex]._useEventNumByType.Sum() + "回";
                _p4NumText.text = _characters[_selectIndex]._souvenirNum + "個";

                Top1.GetComponent<Image>().color = new Color32(21, 21, 101, 255);
                Top2.GetComponent<Image>().color = new Color32(62, 115, 185, 255);
                Back.GetComponent<Image>().color = new Color32(62, 115, 185, 255);
                P2out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                P2in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                //Choice = 0;
            }
            if (_selectIndex == 2)
            {
                text.text = _characters[_selectIndex]._characterName;
                image1.enabled = false;
                image2.enabled = false;
                image3.enabled = false;
                image4.enabled = false;
                _p1ItemText.text = "総資産";
                _p2ItemText.text = "周回数";
                _p3ItemText.text = "イベントマス使用回数";
                _p4ItemText.text = "お土産数";

                _p1NumText.text = _characters[_selectIndex]._money + "＄";
                _p2NumText.text = _characters[_selectIndex]._lapCount + "回";
                _p3NumText.text = _characters[_selectIndex]._useEventNumByType.Sum() + "回";
                _p4NumText.text = _characters[_selectIndex]._souvenirNum + "個";

                Top1.GetComponent<Image>().color = new Color32(11, 60, 11, 255);
                Top2.GetComponent<Image>().color = new Color32(52, 180, 105, 255);
                Back.GetComponent<Image>().color = new Color32(52, 180, 105, 255);
                P3out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                P3in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                //Choice = 0;
            }
            if (_selectIndex == 3)
            {
                text.text = _characters[_selectIndex]._characterName;
                image1.enabled = false;
                image2.enabled = false;
                image3.enabled = false;
                image4.enabled = false;
                _p1ItemText.text = "総資産";
                _p2ItemText.text = "周回数";
                _p3ItemText.text = "イベントマス使用回数";
                _p4ItemText.text = "お土産数";

                _p1NumText.text = _characters[_selectIndex]._money + "＄";
                _p2NumText.text = _characters[_selectIndex]._lapCount + "回";
                _p3NumText.text = _characters[_selectIndex]._useEventNumByType.Sum() + "回";
                _p4NumText.text = _characters[_selectIndex]._souvenirNum + "個";

                Top1.GetComponent<Image>().color = new Color32(101, 71, 21, 255);
                Top2.GetComponent<Image>().color = new Color32(195, 195, 55, 255);
                Back.GetComponent<Image>().color = new Color32(195, 195, 55, 255);
                P4out.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                P4in.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                //Choice = 0;
            }
            if (Choice == 5)
            {
                text.text = "結果発表";
                image1.enabled = true;
                image2.enabled = true;
                image3.enabled = true;
                image4.enabled = true;
                //P1text.text = "Player1                                順位条件";
                //P2text.text = "Player2                                順位条件";
                //P3text.text = "Player3                                順位条件";
                //P4text.text = "Player4                                順位条件";
                Top1.GetComponent<Image>().color = new Color32(50, 55, 19, 255);
                Top2.GetComponent<Image>().color = new Color32(244, 255, 182, 255);
                Back.GetComponent<Image>().color = new Color32(244, 255, 182, 255);
                ReturnImg.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                //Choice = 0;
            }
            else if (Choice == 6)
            {
                //タイトルに戻す
                //ChangeScene
                //Choice = 0;
            }
        }
    }
}