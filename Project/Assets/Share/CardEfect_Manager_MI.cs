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

	[Header("��������J�[�h�̃v���n�u")]
	[SerializeField]
	private GameObject _cardPrefab = null;
	private List<GameObject> _cards = new List<GameObject>();


	// miya
	bool Finish_FinishAnimation = false;
	public void Set_FinishAnimation() { Finish_FinishAnimation = true; }


	/// <summary>
	/// �J�[�h����
	/// </summary>
	/// <param name="cardNumberList">��������J�[�h�̃��X�g</param>
	/// <param name="autoSelect">AI���ʗp</param>
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
	/// AI�I��p
	/// </summary>
	/// <param name="index">�������̃J�[�h���X�g�̑I���������C���f�b�N�X</param>
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
	/// �I�����ꂽ�C���f�b�N�X��Ԃ�
	/// </summary>
	/// <returns>
	/// �J�[�h���X�g���̑I�����ꂽ�J�[�h�̃C���f�b�N�X
	/// �܂��I��r���̏ꍇ -1
	/// </returns>
	public int GetSelectedCardIndex()
	{
		if (!_selectComplete)
		{
			return -1; //�J�[�h���I������Ă��Ȃ�
		}

		return _selectedCardIndex;
	}

	/// <summary>
	/// ��������Ă���J�[�h�̔j��
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