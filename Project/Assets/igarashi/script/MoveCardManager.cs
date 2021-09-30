using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveCardManager : MonoBehaviour
{
    private List<int> _cardNumberLists;
    private int _selectedCardIndex = -1;
    private bool _selectComplete;
    public bool IsSelectComplete => _selectComplete;

    [SerializeField]
    private GameObject _cardPrefab;
    private List<GameObject> _cards = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        _selectedCardIndex = -1;
        _selectComplete = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(_selectedCardIndex);
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
            Debug.Log("card‚ª‘I‘ð‚³‚ê‚Ä‚¢‚È‚¢");
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
}
