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

    [SerializeField]
    private GameObject _cardPrefab;
    private List<GameObject> _cards = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        _selectedCardIndex = 0;
        _selectComplete = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(_selectComplete) Debug.Log(_selectedCardIndex);

        if(_cards.Count != 0)
        {
            if (!_selectComplete)
                SelectCards();
        }
    }


    //=================================
    //public
    //=================================
    public void SetCardList(List<int> cardNumberList)
    {
        _cardNumberLists = cardNumberList;

        if(_cards.Count != 0)
            foreach (var card in _cards)
                Destroy(card);

        CreateCards();
        SelectCardColorUpdate();

        _selectComplete = false;

    }

    public void SelectedCardIndex(int index)
    {
        _selectedCardIndex = index;

        _selectComplete = true;
    }

    public int GetSelectedCardIndex()
    {
        if (!_selectComplete)
        {
            Debug.Log("cardが選択されていない");
            return -1;
        }

        return _selectedCardIndex;
    }

    //=================================



    private void CreateCards()
    {
        for (int i = 0; i < _cardNumberLists.Count; i++)
        {
            GameObject card = Instantiate(_cardPrefab);
            var rt = card.GetComponent<RectTransform>();

            rt.position = new Vector3((rt.rect.width / 2.0f) + (rt.rect.width * i), rt.rect.height / 2.0f, 0.0f);
            card.transform.SetParent(transform);
            card.transform.Find("Text").GetComponent<Text>().text = _cardNumberLists[i].ToString();
            card.GetComponent<MoveCard>().SetIndex(i);

            _cards.Add(card);
        }
    }

    
    private void SelectCards()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            _selectedCardIndex--;
            if (_selectedCardIndex < 0)
                _selectedCardIndex = _cards.Count - 1;

            SelectCardColorUpdate();
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            _selectedCardIndex++;
            if (_selectedCardIndex >= _cards.Count)
                _selectedCardIndex = 0;

            SelectCardColorUpdate();
        }

        if(Input.GetKeyDown(KeyCode.Return))
        {
            _selectComplete = true;
        }
    }

    private void SelectCardColorUpdate()
    {
        for(int i = 0; i < _cards.Count; i++)
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
