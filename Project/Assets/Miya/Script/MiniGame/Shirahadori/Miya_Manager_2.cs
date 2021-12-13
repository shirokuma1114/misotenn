using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Miya_Manager_2 : MonoBehaviour
{
	// Template
	public enum Miya_State_2 { TUTORIAL, WAIT, PLAY, RESULT, END, };
	private		Miya_State_2 _state;
	public		Miya_State_2 State => _state;
	[SerializeField] private MiniGameConnection _miniGameConnection;
	[SerializeField] private Text _tenukiText;
	[SerializeField] private List<Miya_Controller_2> _miniGameControllers;
	public MiniGameResult Result;

	// Public
	public Image Card;
	public Image Button;
	public Image Hand_L;
	public Image Hand_R;
	public float Second_Wait_Start = 1.0f;
	public float Second_Wait_NextPlayer = 2.0f;
	public Image Get_Button	() { return Button; }
	public Image Get_Hand_L	() { return Hand_L; }
	public Image Get_Hand_R	() { return Hand_R; }
	public int Get_CurrentPlayerNumber	() { return CurrentPlayerNumber; }
	public void Set_PlayerFinished		() { Player_Finished = true; }

	// Variable
	float	Counter_Wait_Start		= 0;
	float	Conter_Wait_NextPlayer	= 0;

	int		CurrentPlayerNumber = 1;
	bool	Player_Finished		= false;

	RectTransform Rect_Card;
	Sequence Sequence_Initialize;

	bool FinishGame = false;

	// MiniGame
	RectTransform Rect_Button;
	RectTransform Rect_Hand_L;
	RectTransform Rect_Hand_R;

	// Play
	float Falling_Position_Start	=  500;
	float Falling_Position_End		= -500;
	float Second_FallingStandby		= -1;
	float Conter_FallingStandby		= 0;
	Vector2 Range_Second_FallingStandby = new Vector2(0, 6);
	float Second_Falling = -1;
	Vector2 Range_Second_Falling = new Vector2(1, 2);
	Sequence Sequence_Fall;

	float Tolerance = -1;
	public float Get_Tolerance() { return Tolerance; }

	public float Get_Second_FallingToMiddle()
	{
		return Second_FallingStandby + Second_Falling / 2;
	}

	// Initialize
	void Start()
	{
		// Template
		_miniGameConnection = MiniGameConnection.Instance;
		int i = 0; foreach (var c in _miniGameConnection.Characters) { _miniGameControllers[i].Init(c, this); i++; }
		_tenukiText.text = "全集中白羽取り！\nEnterでプレイ";


		// Variable
		Counter_Wait_Start = 0;
		Conter_Wait_NextPlayer = 0;

		CurrentPlayerNumber = 1;
		Player_Finished = false;

		Rect_Card = Card.GetComponent<RectTransform>();

		FinishGame = false;

		// MiniGame
		Rect_Button = Button.GetComponent<RectTransform>();
		Rect_Hand_L = Hand_L.GetComponent<RectTransform>();
		Rect_Hand_R = Hand_R.GetComponent<RectTransform>();

		// Play
		Second_FallingStandby	= Random.Range(Range_Second_FallingStandby.x, Range_Second_FallingStandby.y);
		Second_Falling			= Random.Range(Range_Second_Falling.x		, Range_Second_Falling.y);
		Tolerance = Second_Falling / 10; // 要調整
	}


	// Update
	void Update()
	{
		switch (_state)
		{
			case Miya_State_2.TUTORIAL: TutorialState	(); break;
			case Miya_State_2.WAIT:		WaitState		(); break;
			case Miya_State_2.PLAY:		PlayState		(); break;
			case Miya_State_2.RESULT:	ResultState		(); break;
			case Miya_State_2.END:		EndState		(); break;
		}
	}

	private void TutorialState()
	{
		if (Input.GetKeyDown(KeyCode.Return))
		{
			_state = Miya_State_2.WAIT;
			_tenukiText.text = "全集中白羽取り！";
			// Display
			Card.gameObject.SetActive(true);
		}
	}

	private void WaitState()
	{
		Counter_Wait_Start += Time.deltaTime;
		if (Counter_Wait_Start > Second_Wait_Start)
		{
			_state = Miya_State_2.PLAY;

			// 上に投げ上げ
			Sequence_Initialize = DOTween.Sequence();
			Sequence_Initialize.Append(Rect_Card.DORotateY(88, 1));
			Sequence_Initialize.Append(Rect_Card.DOLocalMove(new Vector3(0, Falling_Position_Start, 0), 1));
		}
	}
	
	private void PlayState()
	{
		// Play
		Conter_FallingStandby += Time.deltaTime;
		if (Counter_FallingStandby > Second_FallingStandby)
		{
			Counter_FallingStandby = 0;
			Set_Animation_Fall();
		}

		// Player Change
		if ( Player_Finished )
		{
			Conter_Wait_NextPlayer += Time.deltaTime;
			if (Conter_Wait_NextPlayer > Second_Wait_NextPlayer)
			{
				Conter_Wait_NextPlayer = 0;

				CurrentPlayerNumber++;

				Set_Animation_Initialize();
			}
		}
		
		// Finish
		if ( FinishGame )
		{
			_state = Miya_State_2.RESULT;
			_tenukiText.text = "リザルト\nEnterでゲームシーンへ戻る";
		}
	}

	private void ResultState()
	{
		if (Input.GetKeyDown(KeyCode.Return))
			_state = Miya_State_2.END;
	}

	private void EndState()
	{
		_miniGameConnection.EndMiniGame();
	}


	// Animation
	private void Set_Animation_Initialize()
	{
		// 初期位置
		Sequence_Initialize = DOTween.Sequence();
		Sequence_Initialize.Append(Rect_Card.DOLocalMove(new Vector3(0, -100, 0), 1));
		Sequence_Initialize.Join(Rect_Hand_L.DOLocalMove(new Vector3(-200, 0, 0), 1));
		Sequence_Initialize.Join(Rect_Hand_R.DOLocalMove(new Vector3( 200, 0, 0), 1));
		Sequence_Initialize.Join(Rect_Button.DORotateY(0, 1));
		Sequence_Initialize.Join(Rect_Button.DOScaleY(1, 1));
		// 上に投げ上げ
		Sequence_Initialize.Append(Rect_Card.DORotateY(88, 1));
		Sequence_Initialize.Append(Rect_Card.DOLocalMove(new Vector3(0, Falling_Position_Start, 0), 1)
			.OnComplete(Completed));
	}
	// OnComplete
	private void Completed()
	{
		Player_Finished = false;
		
		// Play
		Second_FallingStandby	= Random.Range(Range_Second_FallingStandby.x, Range_Second_FallingStandby.y);
		Second_Falling			= Random.Range(Range_Second_Falling.x, Range_Second_Falling.y);

		if (CurrentPlayerNumber > 4)
		{
			FinishGame = true;
			Result.Display(Ranking());
		}
	}

	private void Set_Animation_Fall()
	{
		Sequence_Fall = DOTween.Sequence();
		Sequence_Fall.Append(Rect_Card.DOLocalMove(new Vector3(0, Falling_Position_End, 0), Second_Falling));
	}
	public void Stop_Animation_Fall()
	{
		Sequence_Fall.Kill();
	}

	// OnDisable
	private void OnDisable()
	{
		if (Sequence_Initialize != null) Sequence_Initialize.Kill();
	}


	//順位付け
	private Dictionary<MiniGameCharacter, int> Ranking()
	{
		Dictionary<MiniGameCharacter, int> dispCharacters = new Dictionary<MiniGameCharacter, int>();
		List<Miya_Controller_2> workCharacters = new List<Miya_Controller_2>();
		workCharacters.AddRange(_miniGameControllers);

		List<Miya_Controller_2> sameRanks = new List<Miya_Controller_2>();
		for (int rank = 1; rank <= 4;)
		{
			int minDistance = 1000000000;
			foreach (var chara in workCharacters)
			{
				if (chara.Get_Score() < minDistance)
				{
					sameRanks.Clear();

					sameRanks.Add(chara);
					minDistance = chara.Get_Score();
					Debug.Log(minDistance);
				}
				else if (chara.Get_Score() == minDistance)
				{
					sameRanks.Add(chara);
				}
			}

			foreach (var ranker in sameRanks)
			{
				dispCharacters.Add(ranker.Character, rank);
				workCharacters.Remove(ranker);
			}
			rank += sameRanks.Count;
		}

		return dispCharacters;
	}
}