using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Miya_ControllerUI_2 : MonoBehaviour
{
	private Text _playerName;
	private Text _rotateCounter;

	public void SetPlayerName(string playerName)
	{
		_playerName.text = playerName;
	}

	public void Set_Score(int _score)
	{
		if 	(_score == 0) _rotateCounter.text = "×";
		else if (_score == 1) _rotateCounter.text = "〇";
	}

	//================

	void Start()
	{
		_playerName = transform.Find("PlayerName").GetComponent<Text>();
		_rotateCounter = transform.Find("Score").GetComponent<Text>();

		_rotateCounter.text = "ー";
	}

	void Update()
	{

	}
}
