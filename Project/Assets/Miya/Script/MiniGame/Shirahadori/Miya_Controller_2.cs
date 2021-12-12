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
	public float AI_Second_Wait = 2;
	public float AI_Second_Hand = 2;
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


	// CPU
	int AI_State = 0; // 0 = 待機, 1 = プレイ, 2 = 終了
	float AI_Counter = 0;


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
				}
				break;

			case 1: // プレイ
				if (AI_Counter > AI_Second_Hand)
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
		Sequence_PushButton.Append(Rect_Button.DOScaleY(0.5f, 0.2f));
		Sequence_PushButton.Join(Rect_Hand_L.DOLocalMove(new Vector3(-20, 0, 0), 0.2f));
		Sequence_PushButton.Join(Rect_Hand_R.DOLocalMove(new Vector3( 20, 0, 0), 0.2f));
		Sequence_PushButton.AppendInterval(1)
			.OnComplete(Completed);
	}
	// OnComplete
	private void Completed()
	{
		_manager.Set_PlayerFinished();
		_playerUI.Set_Score(Score);
	}
	// OnDisable
	private void OnDisable()
	{
		if (Sequence_PushButton != null) Sequence_PushButton.Kill();
	}
}
