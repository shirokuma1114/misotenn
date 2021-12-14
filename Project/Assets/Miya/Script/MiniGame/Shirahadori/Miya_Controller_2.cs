using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Miya_Controller_2 : MonoBehaviour
{
	// Template
	private MiniGameCharacter _controller;
	public MiniGameCharacter Character => _controller;
	private Miya_Manager_2 _manager;
	[SerializeField] private Miya_ControllerUI_2 _playerUI;


	// public
	public int Get_Score() { return Score; }


	// Player Manager
	static int PlayerCount = 0;
	int PlayerNumber = -1;


	// Variable
	int Score = 0;
	bool Finish = false;

	// Animation
	Image Button;
	RectTransform Rect_Button;
	Image Hand_L;
	RectTransform Rect_Hand_L;
	Image Hand_R;
	RectTransform Rect_Hand_R;
	Sequence Sequence_PushButton;

	float Speed_Animation = 0.2f;


	// CPU
	int AI_State = 0; // 0 = 待機, 1 = プレイ, 2 = 終了
	float AI_Counter = 0;

	float AI_Second_Wait = 0;
	float AI_Second_Wait_PushButton = -1;

	Vector2 AI_Range_Second_Error = new Vector2(-0.3f, 0.3f);

	// Play
	float Counter_Waiting = 0;
	
	bool Scored = false;

	float a = 0.52f;


	public void Init(MiniGameCharacter character, Miya_Manager_2 manager)
	{
		_controller = character;
		_manager = manager;

		_playerUI.SetPlayerName(character.Name);


		// Player Manager
		PlayerCount++;
		PlayerNumber = PlayerCount;
		
		// Variable
		Score = 0;
		Finish = false;

		// Animation
		Button = _manager.Get_Button();
		Rect_Button = Button.GetComponent<RectTransform>();
		Hand_L = _manager.Get_Hand_L();
		Rect_Hand_L = Hand_L.GetComponent<RectTransform>();
		Hand_R = _manager.Get_Hand_R();
		Rect_Hand_R = Hand_R.GetComponent<RectTransform>();
		
		// CPU
		AI_State = 0;
		AI_Counter = 0;
	}

	// Initialize
	void Start()
	{

	}

	// Update
	void Update()
	{
		if (_manager.State == Miya_Manager_2.Miya_State_2.PLAY &&
			PlayerNumber == _manager.Get_CurrentPlayerNumber())
		{
			if ( !Finish )
			{
				if (_controller.IsAutomatic) AutomaticPlay();
				else HumanPlay();

				Counter_Waiting += Time.deltaTime;
			}
			else // ボタンが押された瞬間から
			{
				if
					(
					Counter_Waiting + Speed_Animation > _manager.Get_Second_FallingToMiddle() - _manager.Get_Tolerance() - a &&
					Counter_Waiting + Speed_Animation < _manager.Get_Second_FallingToMiddle() + _manager.Get_Tolerance() - a
					)
				{
					if (!Scored)
					{
						Scored = true;

						Score = 1;
						_playerUI.Set_Score(Score);
						_manager.Stop_Animation_Fall();
					}
				}
			}
		}
	}

	private void AutomaticPlay()
	{
		AI_Counter += Time.deltaTime;

		switch ( AI_State )
		{
			case 0: // 待機
				if (AI_Counter > AI_Second_Wait)
				{
					AI_State = 1;
					AI_Counter = 0;

					AI_Second_Wait_PushButton = Random.Range
						(
						_manager.Get_Second_FallingToMiddle() - Speed_Animation + AI_Range_Second_Error.x - a,
						_manager.Get_Second_FallingToMiddle() - Speed_Animation + AI_Range_Second_Error.y - a
						);
				}
				break;

			case 1: // プレイ
				if (AI_Counter > AI_Second_Wait_PushButton)
				{
					AI_State = 2;
					AI_Counter = 0;

					// Finish
					Animation_Push_Button();
					Finish = true;
				}
				break;

			case 2: // 終了
				break;
		}
	}

	private void HumanPlay()
	{
		if (Input.GetKeyUp(KeyCode.Space))
		{
			Animation_Push_Button();
			Finish = true;
		}
	}

	// Animation
	private void Animation_Push_Button()
	{
		Sequence_PushButton = DOTween.Sequence();
		Sequence_PushButton.Append(Rect_Button.DOScaleY(0.5f, Speed_Animation));
		Sequence_PushButton.Join(Rect_Hand_L.DOLocalMove(new Vector3(-20, 0, 0), Speed_Animation));
		Sequence_PushButton.Join(Rect_Hand_R.DOLocalMove(new Vector3( 20, 0, 0), Speed_Animation));
		Sequence_PushButton.AppendInterval(1)
			.OnComplete(Completed);
	}
	// OnComplete
	private void Completed()
	{
		_manager.Set_PlayerFinished();
	}

	// OnDisable
	private void OnDisable()
	{
		if (Sequence_PushButton != null) Sequence_PushButton.Kill(); 
	}
}
