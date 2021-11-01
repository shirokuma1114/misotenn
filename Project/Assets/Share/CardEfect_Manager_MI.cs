using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardEfect_Manager_MI : MonoBehaviour
{
	private List<int> _cardNumberLists;
	private int _selectedCardIndex = 0;
	private bool _selectComplete;
	public bool IsSelectComplete => _selectComplete;
	private bool _autoSelect;

	[Header("生成するカードのプレハブ")]
	[SerializeField]
	private GameObject _cardPrefab = null;
	private List<GameObject> _cards = new List<GameObject>();


	// miya
	bool Finish_FinishAnimation = false;
	public void Set_FinishAnimation() { Finish_FinishAnimation = true; }


	/// <summary>
	/// カード生成
	/// </summary>
	/// <param name="cardNumberList">生成するカードのリスト</param>
	/// <param name="autoSelect">AI判別用</param>
	public void SetCardList(List<int> cardNumberList, bool autoSelect = false)
	{
		_cardNumberLists = cardNumberList;

		DeleteCards();

		CreateCards();
		SelectCardColorUpdate();
		_autoSelect = autoSelect;

		_selectComplete = false;
	}

	/// <summary>
	/// AI選択用
	/// </summary>
	/// <param name="index">生成時のカードリストの選択したいインデックス</param>
	public void IndexSelect(int index)
	{
		if (_cardNumberLists.Count <= index || 0 > index)
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
			card.transform.SetParent(transform);
			card.transform.Find("Text").GetComponent<Text>().text = _cardNumberLists[i].ToString();
			var mc = card.GetComponent<MoveCard>();
			mc.SetIndex(i);

			mc.SetMoveTargetPos(new Vector3((rt.rect.width / 2.0f) + (rt.rect.width * i), rt.rect.height / 2.0f, 0.0f), i == _cardNumberLists.Count - 1);

			_cards.Add(card);
		}

		_selectedCardIndex = 0;
	}


	private void SelectCards()
	{
		if (_autoSelect)
			return;

		if (Input.GetKeyDown(KeyCode.A))
		{
			_selectedCardIndex--;
			if (_selectedCardIndex < 0)
				_selectedCardIndex = _cards.Count - 1;

			SelectCardColorUpdate();
		}
		if (Input.GetKeyDown(KeyCode.D))
		{
			_selectedCardIndex++;
			if (_selectedCardIndex >= _cards.Count)
				_selectedCardIndex = 0;

			SelectCardColorUpdate();
		}


		// changed by miya
		if (Input.GetKeyDown(KeyCode.Return))
		{
			_cards[_selectedCardIndex].GetComponent<CardEfect_MI>().Play_FinishAnimation();
		}
		if ( Finish_FinishAnimation )
		{
			_selectComplete = true;
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