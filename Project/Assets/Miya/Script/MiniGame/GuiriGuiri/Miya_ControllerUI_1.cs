using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Miya_ControllerUI_1 : MonoBehaviour
{
	private Text _playerName;
	private Text _rotateCounter;

	private Image Character;

	public void SetPlayerName(string playerName)
	{
		_playerName.text = playerName;

		Miya_Manager_1 manager = FindObjectOfType<Miya_Manager_1>();

		switch (playerName)
		{
			case "フレジエ":
				Character.sprite = manager.Get_PlayerSprite_f();
				break;
			case "ザッハトルテ":
				Character.sprite = manager.Get_PlayerSprite_z();
				break;
			case "ショートケーキ":
				Character.sprite = manager.Get_PlayerSprite_s();
				break;
			case "アップルパイ":
				Character.sprite = manager.Get_PlayerSprite_a();
				break;
		}
	}
	public string GetPlayerName()
	{
		return _playerName.text;
	}

	public void Set_Score(int _score)
	{
		_rotateCounter.text = _score.ToString();
	}

	//================

	void Start()
	{
		_playerName = transform.Find("PlayerName").GetComponent<Text>();
		_rotateCounter = transform.Find("Score").GetComponent<Text>();
		Character = transform.Find("Character").GetComponent<Image>();
	}

	void Update()
	{

	}
}
