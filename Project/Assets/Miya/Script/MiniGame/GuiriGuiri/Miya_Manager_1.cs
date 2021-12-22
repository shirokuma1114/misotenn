using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Miya_Manager_1 : MonoBehaviour
{
	public enum Miya_State_1
	{
		TUTORIAL,
		WAIT,
		PLAY,
		RESULT,
		END,
	};
	private Miya_State_1 _state;
	public Miya_State_1 State => _state;

	[SerializeField]
	private MiniGameConnection _miniGameConnection;

	[SerializeField]
	private List<Miya_Controller_1> _miniGameControllers;



	// Variable-------------------------------------------------------------------------------------------
	//public Slider Slider_Percentage;
	public Image Guage_Back;
	Vector3 Rect_Guage_Back_First;
	RectTransform Rect_Guage_Back;

	public float Second_Wait = 1.0f;
	float Timer_Wait = 0;
	public Vector2 SecondRange_Meter = new Vector2(1, 2);
	public Vector2 CorrectPercentageRange_Meter = new Vector2(0.4f, 0.9f);

	bool FinishGame = false;
	public void Set_FinishGame() { FinishGame = true; }


	// プレイヤー管理
	int PlayerCount = 0;
	public int Get_PlayerCount() { return PlayerCount; }
	int CurrentPlayerNumber = 1;
	public int Get_CurrentPlayerNumber() { return CurrentPlayerNumber; }

	bool Player_Finished = false;
	public void Inform_Finished()
	{
		Player_Finished = true;

		Miya_Controller_1.Reset_MeterPercentage();
	}

	// Animation
	public Image Card;
	public Image Get_Card() { return Card; }
	RectTransform Rect_Card;
	Sequence Sequence_Initialize;


	// GoalLine
	public Image Goal;
	float Goal_Length = -1;
	public float Get_Length_CardToGoal()
	{
		return Mathf.Abs(Goal.GetComponent<RectTransform>().localPosition.x - Card.GetComponent<RectTransform>().localPosition.x);
	}

	// Back
	public Image Board;


	public MiniGameResult Result;



	// Play Player Color
	public Image PlayerColor_1;
	public Image PlayerColor_2;
	public Image PlayerColor_3;
	public Image PlayerColor_4;

	public Image Character_1;
	public Image Character_2;
	public Image Character_3;
	public Image Character_4;

	public Sprite PlayerSprite_f;
	public Sprite PlayerSprite_z;
	public Sprite PlayerSprite_s;
	public Sprite PlayerSprite_a;
	public Sprite Get_PlayerSprite_f() { return PlayerSprite_f; }
	public Sprite Get_PlayerSprite_z() { return PlayerSprite_z; }
	public Sprite Get_PlayerSprite_s() { return PlayerSprite_s; }
	public Sprite Get_PlayerSprite_a() { return PlayerSprite_a; }


	void Start()
	{
		_miniGameConnection = MiniGameConnection.Instance;
		//Debug.Log(_miniGameConnection.gameObject);

		PlayerCount = 0;
		foreach (var c in _miniGameConnection.Characters)
		{
			_miniGameControllers[PlayerCount].Init(c, this);


			PlayerCount++;
		}


		//Slider_Percentage.gameObject.SetActive(false);

		Timer_Wait = 0;

		FinishGame = false;


		Rect_Guage_Back = Guage_Back.GetComponent<RectTransform>();
		Rect_Guage_Back_First = Rect_Guage_Back.anchoredPosition;



		// Animation
		Rect_Card = Card.GetComponent<RectTransform>();


		// GoalLine
		Goal_Length = Random.Range(100, 200);
		Goal.GetComponent<RectTransform>().localPosition = new Vector3( Goal_Length , 0, 0);



		PlayerColor_1.color = new Color32(255, 0, 0, 50);
	}

	void Update()
	{
		switch (_state)
		{
			case Miya_State_1.TUTORIAL:
				TutorialState();
				break;

			case Miya_State_1.WAIT:
				WaitState();
				break;

			case Miya_State_1.PLAY:
				PlayState();
				break;

			case Miya_State_1.RESULT:
				ResltState();
				break;

			case Miya_State_1.END:
				EndState();
				break;
		}
	}

	private void TutorialState()
	{
		//if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Start"))
		{
			_state = Miya_State_1.WAIT;
			//Slider_Percentage.gameObject.SetActive(true);

			Card.gameObject.SetActive(true);
			Goal.gameObject.SetActive(true);
			//Board.gameObject.SetActive(true);
		}
	}

	private void WaitState()
	{
		Timer_Wait += Time.deltaTime;
		if (Timer_Wait > Second_Wait)
		{
			_state = Miya_State_1.PLAY;
		}
	}


	float Second_Wait_NextPlayer = 2.0f;
	float Conter_Wait_NextPlayer = 0;

	private void PlayState()
	{
		// ゲージ
		//Slider_Percentage.value = Miya_Controller_1.Get_MeterPercentage(); // koko
		Rect_Guage_Back.anchoredPosition = new Vector3(Rect_Guage_Back_First.x + Miya_Controller_1.Get_MeterPercentage() * 356, Rect_Guage_Back_First.y, Rect_Guage_Back_First.z);

		// プレイヤー変更
		if ( Player_Finished )
		{
			Conter_Wait_NextPlayer += Time.deltaTime;
			if (Conter_Wait_NextPlayer > Second_Wait_NextPlayer)
			{
				Conter_Wait_NextPlayer = 0;

				switch (CurrentPlayerNumber)
				{
					case 1:
						PlayerColor_1.color = new Color32(0, 0, 0, 35);
						break;
					case 2:
						PlayerColor_2.color = new Color32(0, 0, 0, 35);
						break;
					case 3:
						PlayerColor_3.color = new Color32(0, 0, 0, 35);
						break;
				}

				CurrentPlayerNumber++;

				switch (CurrentPlayerNumber)
				{
					case 2:
						PlayerColor_2.color = new Color32(255, 0, 0, 50);
						break;
					case 3:
						PlayerColor_3.color = new Color32(255, 0, 0, 50);
						break;
					case 4:
						PlayerColor_4.color = new Color32(255, 0, 0, 50);
						break;
				}

				Set_Animation_Initialize();
			}
		}


		// 終了
		if ( FinishGame )
		{
			_state = Miya_State_1.RESULT;
		}
	}

	private void ResltState()
	{
		if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Start") || Input.GetButtonDown("A"))
			_state = Miya_State_1.END;
	}

	private void EndState()
	{
		_miniGameConnection.EndMiniGame();
	}


	private void Set_Animation_Initialize()
	{
		Sequence_Initialize = DOTween.Sequence();
		Sequence_Initialize.Append(Rect_Card.DOLocalMove(new Vector3(-300, 0, 0), 1));
		Sequence_Initialize.Join(Rect_Card.DORotate(new Vector3(0, 0, 0), 1)
			.OnComplete(Completed));
	}
	// OnComplete
	private void Completed()
	{
		Player_Finished = false;


		if (CurrentPlayerNumber > 4)
		{
			FinishGame = true;
			// リザルト
			Result.Display(Ranking());
		}
	}




	//順位付け
	private Dictionary<MiniGameCharacter, int> Ranking()
	{
		Dictionary<MiniGameCharacter, int> dispCharacters = new Dictionary<MiniGameCharacter, int>();
		List<Miya_Controller_1> workCharacters = new List<Miya_Controller_1>();
		workCharacters.AddRange(_miniGameControllers);

		List<Miya_Controller_1> sameRanks = new List<Miya_Controller_1>();
		for (int rank = 1; rank <= 4;)
		{
			int minDistance = 1000000000;
			foreach (var chara in workCharacters)
			{
				if (chara.Get_DistanceScore() < minDistance)
				{
					sameRanks.Clear();

					sameRanks.Add(chara);
					minDistance = chara.Get_DistanceScore();
					//Debug.Log(minDistance);
				}
				else if (chara.Get_DistanceScore() == minDistance)
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
