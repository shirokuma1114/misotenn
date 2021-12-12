using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Miya_ControllerUI_1 : MonoBehaviour
{
	private Text _playerName;
	private Text _rotateCounter;

	public void SetPlayerName(string playerName)
	{
		_playerName.text = playerName;
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
	}

	void Update()
	{

	}
}
