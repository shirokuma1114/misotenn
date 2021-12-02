using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Miya_Controller_1 : MonoBehaviour
{
	private MiniGameCharacter _controller;
	private Miya_Manager_1 _manager;

	[SerializeField]
	private Miya_ControllerUI_1 _playerUI;



	// Variable
	float Second_Meter = -1;
	float Count_Meter = 0;
	bool Hold = false;
	bool Throw = false;
	float CorrectPercentage = -1;
	static float Percentage_Meter = 0;// アニメーション終了時初期化
	static public float Get_MeterPercentage() { return Percentage_Meter; }
	float Percentage = 0;

	bool Animation = false;

	static int PlayerCount = 0;
	int PlayerNumber = -1;



	public void Init(MiniGameCharacter character, Miya_Manager_1 manager)
	{
		_controller = character;
		_manager = manager;

		_playerUI.SetPlayerName(character.Name);


		Second_Meter = Random.Range(_manager.SecondRange_Meter.x, _manager.SecondRange_Meter.y);
		Count_Meter = 0;
		Hold = false;
		Throw = false;
		CorrectPercentage = Random.Range(_manager.CorrectPercentageRange_Meter.x, _manager.CorrectPercentageRange_Meter.y);
		Percentage = 0;

		Animation = false;

		PlayerCount++;
		PlayerNumber = PlayerCount;
		//Debug.Log("PlayerNumber" + PlayerNumber);
	}

	//==================

	void Start()
	{
		
	}

	void Update()
	{
		if (_manager.State == Miya_Manager_1.Miya_State_1.PLAY &&
			PlayerNumber == _manager.Get_CurrentPlayerNumber())
		{
			//Debug.Log("Test_1");
			if ( !Animation )
			{
				if ( _controller.IsAutomatic )
					AutomaticPlay();
				else
					HumanPlay();

				if ( Hold )
				{
					Count_Meter += Time.deltaTime;
					Percentage = Percentage_Meter = Count_Meter / Second_Meter;
					if (Percentage_Meter > 1)
					{
						Percentage = Percentage_Meter = 1.0f;
						Animation = true;
					}
					Debug.Log("Percentage_Meter : " + Percentage_Meter);
				}
			}
			else
			{
				Update_Animation();
			}
		}
	}

	private void AutomaticPlay()
	{
		
	}

	private void HumanPlay()
	{
		if (Input.GetKeyDown(KeyCode.Space) && !Throw)
		{
			Hold = true;
		}

		if (Input.GetKeyUp(KeyCode.Space) && Hold)
		{
			Hold = false;
			Throw = true;

			// Animation
			Animation = true;
		}
	}

	private void Update_Animation()
	{

	}
}
