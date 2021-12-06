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
	static public void Reset_MeterPercentage() { Percentage_Meter = 0; }
	float Percentage = 0;

	bool Animation = false;

	static int PlayerCount = 0;
	int PlayerNumber = -1;


	// Animation
	Image Card;
	RectTransform Rect_Card;
	Sequence Sequence_Initialize;
	Sequence Sequence_Throw;

	float TotalLength = 150 + 55;


	// CPU
	int AI_State = 0; // 0 = 待機, 1 = ホールド, 2 = アニメーション
	float AI_Second_Wait = 2;
	float AI_Second_Throw;
	float Counter_AI = 0;


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


		// プレイヤー管理
		PlayerCount++;
		PlayerNumber = PlayerCount;
		//Debug.Log("PlayerNumber" + PlayerNumber);


		// Animation
		Card = _manager.Get_Card();
		Rect_Card = Card.GetComponent<RectTransform>();

		// CPU
		AI_State = 0;
		AI_Second_Throw = Random.Range(Second_Meter - 1, Second_Meter - 0.2f);
		Counter_AI = 0;
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
						Set_Animation();
					}
					//Debug.Log("Percentage_Meter : " + Percentage_Meter);
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
		Counter_AI += Time.deltaTime;

		switch( AI_State )
		{
			case 0: // 待機
				if (Counter_AI > AI_Second_Wait)
				{
					AI_State = 1;
					Counter_AI = 0;
					
					Hold = true;
				}
				break;

			case 1: // ホールド
				if (Counter_AI > AI_Second_Throw)
				{
					AI_State = 2;
					Counter_AI = 0;

					Hold = false;
					Throw = true;

					// Animation
					Animation = true;
					Set_Animation();
				}
				break;

			case 2: // アニメーション
				break;
		}
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
			Set_Animation();
		}
	}

	private void Update_Animation()
	{
		//Debug.Log("Test");
	}

	private void Set_Animation()
	{
		Sequence_Throw = DOTween.Sequence();
		Sequence_Throw.Append(Rect_Card.DOLocalMove(new Vector3(0, -55 + TotalLength * Percentage_Meter, 0), 3));
		Sequence_Throw.Join(Rect_Card.DORotate(new Vector3(0, 0, 360 + 360 * Percentage_Meter), 3)
			.OnComplete(Completed));

		//Debug.Log("Test");
	}

	public void Set_Animation_Initialize()
	{
		Sequence_Initialize = DOTween.Sequence();
		Sequence_Initialize.Append(Rect_Card.DOLocalMove(new Vector3(0, -55, 0), 1));
		Sequence_Initialize.Join(Rect_Card.DORotate(new Vector3(0, 0, 0), 1));
	}

	// OnComplete
	private void Completed()
	{
		_manager.Inform_Finished();
		_playerUI.Set_Score((int)_manager.Get_Length_CardToGoal());
	}
	// OnDisable
	private void OnDisable()
	{
		if (Sequence_Initialize != null) Sequence_Initialize.Kill();
		if (Sequence_Throw != null) Sequence_Throw.Kill();
	}
}
