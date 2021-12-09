using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveCardManager : MonoBehaviour
{
    private List<int> _cardNumberLists;
    private int _selectedCardIndex = 0;
    private bool _selectComplete;
    public bool IsSelectComplete => _selectComplete;
    private bool _autoSelect;

    // miya
    bool _finAnimEndFlag = false;
    bool _finAnimStartFlag = false;
    public void FinAnimEnd() { _finAnimEndFlag = true; }

    [Header("生成するカードのプレハブ")]
    [SerializeField]
    private GameObject _cardPrefab = null;
    private List<GameObject> _cards = new List<GameObject>();

    [Header("操作")]
    [SerializeField]
    private KeyCode _selectRightKey = KeyCode.D;
    [SerializeField]
    private KeyCode _selectLeftKey = KeyCode.A;
    [SerializeField]
    private KeyCode _enterKey = KeyCode.Return;

    CharacterBase _character;

    private float beforeTrigger;

    /// <summary>
    /// カード生成
    /// </summary>
    /// <param name="cardNumberList">生成するカードのリスト</param>
    /// <param name="autoSelect">AI判別用</param>
    public void SetCardList(List<int> cardNumberList, CharacterBase character)
    {
        _cardNumberLists = cardNumberList;
        _character = character;
        DeleteCards();

        CreateCards();
        SelectCardColorUpdate();
        _autoSelect = character.IsAutomatic;

        _selectComplete = false;
    }

    /// <summary>
    /// AI選択用
    /// </summary>
    /// <param name="index">生成時のカードリストの選択したいインデックス</param>
    public void IndexSelect(int index)
    {
        if(_cardNumberLists.Count <= index　|| 0 > index)
        {
            _selectedCardIndex = -1;
            return;
        }

        _selectedCardIndex = index;
        SelectCardColorUpdate();
        _autoSelect = true;

        _selectComplete = true;
    }

    /// <summary>
    /// 選択されたインデックスを返す
    /// </summary>
    /// <returns>
    /// カードリスト中の選択されたカードのインデックス
    /// まだ選択途中の場合 -1
    /// </returns>
    public int GetSelectedCardIndex()
    {
        if (!_selectComplete)
        {
            return -1; //カードが選択されていない
        }

        return _selectedCardIndex;
    }

    /// <summary>
    /// 生成されているカードの破棄
    /// </summary>
    public void DeleteCards()
    {
        if (_cards.Count != 0)
        {
            for (int i = 0; i < _cards.Count; i++)
            {
                Destroy(_cards[i]);
            }

            _cards.Clear();
        }
    }

    //=================================

    void Start()
    {
        _selectedCardIndex = 0;
        _selectComplete = false;
    }

    void Update()
    {
        if (_cards.Count != 0)
        {
            if (_finAnimEndFlag)
            {
                _selectComplete = true;
            }

            if (!_selectComplete)
                SelectCards();
        }
    }

    private void CreateCards()
    {
        for (int i = 0; i < _cardNumberLists.Count; i++)
        {
            GameObject card = Instantiate(_cardPrefab);
            var rt = card.GetComponent<RectTransform>();

            rt.position = new Vector3(0.0f, -50.0f, 0.0f);
            rt.SetParent(transform);
            rt.localScale = _cardPrefab.transform.localScale;

            rt.Find("Text").GetComponent<Text>().text = _cardNumberLists[i].ToString();

            var mc = card.GetComponent<MoveCard>();
            mc.SetIndex(i);

            Vector3 cardPos = new Vector3();
            cardPos.x = rt.rect.width * rt.lossyScale.x / 2 + rt.rect.width * rt.lossyScale.x * i;
            cardPos.y = rt.rect.height * rt.lossyScale.y / 2;
            cardPos.z = 0.0f;
            mc.SetMoveTargetPos(cardPos,i == _cardNumberLists.Count - 1);
            //mc.SetMoveTargetPos(new Vector3((rt.rect.width * rt.localScale.x) + (rt.rect.width * rt.localScale.x * i), rt.rect.height * rt.localScale.y, 0.0f),i == _cardNumberLists.Count - 1);
            //mc.SetMoveTargetPos(new Vector3(0, 0, 0.0f),i == _cardNumberLists.Count - 1);
           
            _finAnimEndFlag = false;
            _finAnimStartFlag = false;

            _cards.Add(card);
        }

        _selectedCardIndex = 0;
    }


    private void SelectCards()
    {
        if (_autoSelect)
            return;
        if (_finAnimStartFlag)
            return;
        foreach(var card in _cards)
        {
            if (!card.GetComponent<MoveCard>().IsStartAnimComplete)
                return;
        }

        float viewButton = _character.Input.GetAxis("Horizontal");

        if (beforeTrigger == 0.0f)
        {
            if (viewButton < 0)
            {
                _selectedCardIndex--;
                if (_selectedCardIndex < 0)
                    _selectedCardIndex = _cards.Count - 1;

                SelectCardColorUpdate();

                Control_SE.Get_Instance().Play_SE("UI_Select");
            }
            if(viewButton > 0)
            {
                _selectedCardIndex++;
                if (_selectedCardIndex >= _cards.Count)
                    _selectedCardIndex = 0;

                SelectCardColorUpdate();

                Control_SE.Get_Instance().Play_SE("UI_Select");
            }
        }

        beforeTrigger = viewButton;

        if (Input.GetKeyDown(_selectLeftKey))
        {
            _selectedCardIndex--;
            if (_selectedCardIndex < 0)
                _selectedCardIndex = _cards.Count - 1;

            SelectCardColorUpdate();

            if(Control_SE.Get_Instance())Control_SE.Get_Instance().Play_SE("UI_Select");
        }
        if (Input.GetKeyDown(_selectRightKey))
        {
            _selectedCardIndex++;
            if (_selectedCardIndex >= _cards.Count)
                _selectedCardIndex = 0;

            SelectCardColorUpdate();

            if(Control_SE.Get_Instance())Control_SE.Get_Instance().Play_SE("UI_Select");
        }

        // changed by miya
        if (Input.GetKeyDown(_enterKey) || _character.Input.GetButtonDown("A"))
        {
            _cards[_selectedCardIndex].GetComponent<MoveCard>().PlayFinishAnimation();
            _finAnimStartFlag = true;

            if(Control_SE.Get_Instance())Control_SE.Get_Instance().Play_SE("UI_Correct");
        }        
    }

    private void SelectCardColorUpdate()
    {
        for (int i = 0; i < _cards.Count; i++)
        {
            if (i == _selectedCardIndex)
            {
                _cards[i].GetComponent<Image>().color = new Color(1, 0, 0, 0.5f);

                continue;
            }

            _cards[i].GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
        }
    }
}