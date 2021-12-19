using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CasinoGame : MonoBehaviour
{
    private const int CARD_NUMBER_MAX = 13;
    private const int CARD_NUMBER_MIN = 1;

    public enum CasinoGameState
    {
        IDLE,
        INIT,

        GAME,
        AI_GAME,

        CORRECT_CHECK,
        CONTINUE_CHECK,
        END,
    }
    private CasinoGameState _state;
    public CasinoGameState State => _state;

    private List<CasinoCard> _selectCards = new List<CasinoCard>();
    private int _selectIndex = 0;
    private int _answerIndex;
    private int _answerNumber;

    private bool _correct = false;
    public bool IsCorrectAnswer => _correct;

    private Image _selectFrameSprite;
    private RectTransform _selectFrameTransform;

    private bool _complate = false;
    public bool IsComplate => _complate;

    private bool _autoPlay;
    private bool _aiThinkingAnimEnd;

    private int _betMoney;
    private int _reward;
    public int Reward => _reward;

    private PayUI _payUI;
    private MessageWindow _messageWindow;

    private CharacterBase _character;

    private float beforeTrigger;


    [SerializeField]
    private List<Sprite> _cardNumberSprites;

    [SerializeField]
    private KeyCode _right = KeyCode.D;
    [SerializeField]
    private KeyCode _left = KeyCode.A;
    [SerializeField]
    private KeyCode _enter = KeyCode.Return;


    public void Play(CharacterBase player,int betMoney)
    {
        _character = player;
        _betMoney = betMoney;
        _reward = betMoney;

        _state = CasinoGameState.INIT;
    }

    public void AIRandomSelect()
    {
        foreach (var card in _selectCards)
        {
            card.TrunUp();
        }

        _selectFrameSprite.enabled = false;

        if (_selectIndex == _answerIndex)
            _correct = true;
        else
            _correct = false;
    }

    //========================

    private void Awake()
    {
        _selectIndex = 0;
        _state = CasinoGameState.IDLE;
    }

    void Start()
    {
        _selectCards.Add(transform.Find("Card1").GetComponent<CasinoCard>());
        _selectCards.Add(transform.Find("Card2").GetComponent<CasinoCard>());

        var sf = transform.Find("SelectFrame").gameObject;
        _selectFrameSprite = sf.GetComponent<Image>();
        _selectFrameSprite.enabled = false;
        _selectFrameTransform = sf.GetComponent<RectTransform>();

        GetComponent<Canvas>().worldCamera = Camera.main;

        _payUI = FindObjectOfType<PayUI>();
        _messageWindow = FindObjectOfType<MessageWindow>();
    }

    void Update()
    {
        switch (_state)
        {
            case CasinoGameState.IDLE:
                break;
            

            case CasinoGameState.INIT:
                InitState();
                break;

            case CasinoGameState.GAME:
                GameState();
                break;

            case CasinoGameState.AI_GAME:
                AIGameState();
                break;

            case CasinoGameState.CORRECT_CHECK:
                CorrectCheckState();
                break;

            case CasinoGameState.CONTINUE_CHECK:
                ContinueCheckState();
                break;

            case CasinoGameState.END:
                EndState();
                break;
        }
    }

    private void InitState()
    {
        _answerIndex = Random.Range(0, 2);
        _answerNumber = Random.Range(2, 12);

        for (int i = 0; i < _selectCards.Count; i++)
        {
            if (i == _answerIndex)
            {
                _selectCards[i].InitDisplay(_cardNumberSprites[_answerNumber - 1], true);
            }
            else
            {
                _selectCards[i].InitDisplay(_cardNumberSprites[Random.Range(CARD_NUMBER_MIN, _answerNumber - 1) - 1], true);
            }
        }

        _selectIndex = 0;
        Vector3 selectCardPos = _selectCards[_selectIndex].GetComponent<RectTransform>().localPosition;
        _selectFrameTransform.localPosition = new Vector3(selectCardPos.x,selectCardPos.y,2.0f);
        _selectFrameSprite.enabled = true;

        _complate = false;

        _autoPlay = _character.IsAutomatic;
        if (_autoPlay)
        {
            _selectIndex = Random.Range(0, 2);

            Sequence sequence = DOTween.Sequence();
            sequence.Append(_selectFrameTransform.DOLocalMoveX(_selectCards[0].GetComponent<RectTransform>().localPosition.x, 0.5f).SetEase(Ease.INTERNAL_Zero));
            sequence.Append(_selectFrameTransform.DOLocalMoveX(_selectCards[1].GetComponent<RectTransform>().localPosition.x, 0.5f).SetEase(Ease.INTERNAL_Zero));
            sequence.Append(_selectFrameTransform.DOLocalMoveX(_selectCards[0].GetComponent<RectTransform>().localPosition.x, 0.5f).SetEase(Ease.INTERNAL_Zero));
            sequence.Append(_selectFrameTransform.DOLocalMoveX(_selectCards[_selectIndex].GetComponent<RectTransform>().localPosition.x, 0.5f).SetEase(Ease.INTERNAL_Zero));
            _aiThinkingAnimEnd = false;

            sequence.OnComplete(() => { _aiThinkingAnimEnd = true; });

            _state = CasinoGameState.AI_GAME;
        }
        else
            _state = CasinoGameState.GAME;
    }

    private void GameState()
    {
        Operation();
    }

    private void AIGameState()
    {
        if (!_aiThinkingAnimEnd)
            return;

        foreach (var card in _selectCards)
        {
            card.TrunUp();
        }

        _selectFrameSprite.enabled = false;

        _state = CasinoGameState.CORRECT_CHECK;
    }

    private void CorrectCheckState()
    {
        if (!_selectCards[0].IsAnimEnd)
            return;

        _correct = _selectIndex == _answerIndex;

        if (_correct)
        {
            _reward *= 2; 

            _messageWindow.SetMessage("正解!!\nダブルアップチャンスに挑戦しますか？\n(降りた場合" + _reward.ToString() + "円獲得できます)", _character);
            _payUI.Open(_character);

            if (_autoPlay)
                AIDoubleUpChallenge();

            _state = CasinoGameState.CONTINUE_CHECK;
        }
        else
        {
            _reward = 0;

            _messageWindow.SetMessage("不正解", _character);

            _state = CasinoGameState.END;
        }            
    }

    private void ContinueCheckState()
    {
        if (!_payUI.IsSelectComplete)
            return;

        if(_payUI.IsSelectYes)
        {
            _state = CasinoGameState.INIT;
        }
        else
        {
            _state = CasinoGameState.END;
        }
    }

    private void EndState()
    {
        foreach (var card in _selectCards)
            card.UnDisplay();

        _complate = true;
    }

    private void Operation()
    {
        if (_autoPlay)
            return;

        float viewButton = _character.Input.GetAxis("Horizontal");

        if (beforeTrigger == 0.0f)
        {
            if (viewButton < 0)
            {
                _selectIndex++;
                _selectIndex = Mathf.Min(_selectIndex, _selectCards.Count - 1);

                Vector3 selectCardPos = _selectCards[_selectIndex].GetComponent<RectTransform>().localPosition;
                _selectFrameTransform.localPosition = new Vector3(selectCardPos.x, selectCardPos.y, 2.0f);
            }

            if (viewButton > 0)
            {
                _selectIndex--;
                _selectIndex = Mathf.Max(_selectIndex, 0);

                Vector3 selectCardPos = _selectCards[_selectIndex].GetComponent<RectTransform>().localPosition;
                _selectFrameTransform.localPosition = new Vector3(selectCardPos.x, selectCardPos.y, 2.0f);
            }
        }

        beforeTrigger = viewButton;

        if (Input.GetKeyDown(_right))
        {
            _selectIndex++;
            _selectIndex = Mathf.Min(_selectIndex, _selectCards.Count - 1);

            Vector3 selectCardPos = _selectCards[_selectIndex].GetComponent<RectTransform>().localPosition;
            _selectFrameTransform.localPosition = new Vector3(selectCardPos.x, selectCardPos.y, 2.0f);
        }
        if(Input.GetKeyDown(_left))
        {
            _selectIndex--;
            _selectIndex = Mathf.Max(_selectIndex,0);

            Vector3 selectCardPos = _selectCards[_selectIndex].GetComponent<RectTransform>().localPosition;
            _selectFrameTransform.localPosition = new Vector3(selectCardPos.x, selectCardPos.y, 2.0f);
        }

        if(Input.GetKeyDown(_enter) || _character.Input.GetButtonDown("A"))
        {
            foreach(var card in _selectCards)
            {
                card.TrunUp();               
            }

            _selectFrameSprite.enabled = false;

            _state = CasinoGameState.CORRECT_CHECK;
        }
    }

    private void AIDoubleUpChallenge()
    {
        //AIがダブルアップに挑戦する確率(%)
        const int DOUBLEUP_CHALLENGE_PERCENTAGE = 50;

        if(Random.Range(1,100) <= DOUBLEUP_CHALLENGE_PERCENTAGE)
        {
            _payUI.AISelectYes();
        }
        else
        {
            _payUI.AISelectNo();
        }
    }
}
