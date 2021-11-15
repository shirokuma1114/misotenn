using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CasinoGame : MonoBehaviour
{
    private const int CARD_NUMBER_MAX = 13;
    private const int CARD_NUMBER_MIN = 1;

    private CasinoCard _originCard;
    private int _originCardNumber;

    private List<CasinoCard> _selectCards = new List<CasinoCard>();
    private int _selectIndex = 0;
    private int _answerIndex;
    private bool _correctAnswer = false;
    public bool IsCorrectAnswer => _correctAnswer;

    private Image _selectFrameSprite;
    private RectTransform _selectFrameTransform;

    private bool _playing;
    private bool _selectEnd;
    private bool _complate = false;
    public bool IsComplate => _complate;

    private bool _autoPlay;

    [SerializeField]
    private KeyCode _right = KeyCode.D;
    [SerializeField]
    private KeyCode _left = KeyCode.A;
    [SerializeField]
    private KeyCode _enter = KeyCode.Return;


    public void Play(bool autoPlay = false)
    {
        _answerIndex = Random.Range(0, 3);

        _originCardNumber = Random.Range(2, 12);
        _originCard.InitDisplay(_originCardNumber);
        
        for(int i = 0; i < _selectCards.Count;i++)
        {
            if(i == _answerIndex)
            {
                _selectCards[i].InitDisplay(Random.Range(_originCardNumber + 1, CARD_NUMBER_MAX),true);
            }
            else
            {
                _selectCards[i].InitDisplay(Random.Range(CARD_NUMBER_MIN, _originCardNumber - 1),true);
            }
        }

        _selectIndex = 0;
        _selectFrameTransform.position = _selectCards[_selectIndex].GetComponent<RectTransform>().position;

        _selectFrameSprite.enabled = true;

        _playing = true;
        _complate = false;
        _selectEnd = false;

        _autoPlay = autoPlay;
        if (autoPlay)
        {
            _selectIndex = Random.Range(0, 3);
            _selectFrameTransform.position = _selectCards[_selectIndex].GetComponent<RectTransform>().position;

            Invoke("AIRandomSelect", 1.5f);
        }
    }

    public void AIRandomSelect()
    {
        foreach (var card in _selectCards)
        {
            card.TrunUp();
        }

        _selectFrameSprite.enabled = false;

        if (_selectIndex == _answerIndex)
            _correctAnswer = true;
        else
            _correctAnswer = false;

        _selectEnd = true;
    }

    //========================

    private void Awake()
    {
        _selectIndex = 0;
    }

    void Start()
    {
        _originCard = transform.Find("OriginCard").GetComponent<CasinoCard>();

        _selectCards.Add(transform.Find("Card1").GetComponent<CasinoCard>());
        _selectCards.Add(transform.Find("Card2").GetComponent<CasinoCard>());
        _selectCards.Add(transform.Find("Card3").GetComponent<CasinoCard>());

        var sf = transform.Find("SelectFrame").gameObject;
        _selectFrameSprite = sf.GetComponent<Image>();
        _selectFrameSprite.enabled = false;
        _selectFrameTransform = sf.GetComponent<RectTransform>();

        GetComponent<Canvas>().worldCamera = Camera.main;
    }

    void Update()
    {
        if (!_playing)
            return;

        AnimEndCheck();

        Operation();
    }

    private void Operation()
    {
        if (_selectEnd)
            return;
        if (_autoPlay)
            return;

        if(Input.GetKeyDown(_right))
        {
            _selectIndex++;
            _selectIndex = Mathf.Min(_selectIndex, _selectCards.Count - 1);

            _selectFrameTransform.position = _selectCards[_selectIndex].GetComponent<RectTransform>().position;
        }
        if(Input.GetKeyDown(_left))
        {
            _selectIndex--;
            _selectIndex = Mathf.Max(_selectIndex,0);

            _selectFrameTransform.position = _selectCards[_selectIndex].GetComponent<RectTransform>().position;
        }

        if(Input.GetKeyDown(_enter))
        {
            foreach(var card in _selectCards)
            {
                card.TrunUp();               
            }

            _selectFrameSprite.enabled = false;

            if (_selectIndex == _answerIndex)
                _correctAnswer = true;
            else
                _correctAnswer = false;

            _selectEnd = true;
        }
    }

    private void AnimEndCheck()
    {
        if (!_selectEnd)
            return;

        if (_selectCards[0].IsAnimEnd)
        {
            _originCard.UnDisplay();
            foreach (var card in _selectCards)
            {
                card.UnDisplay();
            }
            _selectFrameSprite.enabled = false;

            _complate = true;
        }
    }
}
