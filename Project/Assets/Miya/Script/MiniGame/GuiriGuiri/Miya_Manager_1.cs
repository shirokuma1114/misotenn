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
	private Text _tenukiText;

	[SerializeField]
	private List<Miya_Controller_1> _miniGameControllers;



	// Variable
	public Slider Slider_Percentage;

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


		_tenukiText.text = "ギリギリチキンレース\nEnterでプレイ";


		Slider_Percentage.gameObject.SetActive(false);

		Timer_Wait = 0;

		FinishGame = false;


		// Animation
		Rect_Card = Card.GetComponent<RectTransform>();


		// GoalLine
		Goal_Length = Random.Range(100, 200);
		Goal.GetComponent<RectTransform>().localPosition = new Vector3( Goal_Length , 0, 0);
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
		if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Start"))
		{
			_state = Miya_State_1.WAIT;
			_tenukiText.text = "ギリギリチキンレース";
			//_tenukiText.text = "ギリギリチキンレース\nスペースキーを押してチャージ\nスペースキーを離して投げる　";
			Slider_Percentage.gameObject.SetActive(true);

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
		Slider_Percentage.value = Miya_Controller_1.Get_MeterPercentage();

		// プレイヤー変更
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


		// 終了
		if ( FinishGame )
		{
			_state = Miya_State_1.RESULT;
			_tenukiText.text = "リザルト\nEnterでゲームシーンへ戻る";
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
					Debug.Log(minDistance);
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
