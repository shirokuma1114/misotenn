using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

	int PlayerCount = 0;
	public int Get_PlayerCount() { return PlayerCount; }
	int CurrentPlayerNumber = 1;
	public int Get_CurrentPlayerNumber() { return CurrentPlayerNumber; }


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
		if (Input.GetKeyDown(KeyCode.Return))
		{
			_state = Miya_State_1.WAIT;
			_tenukiText.text = "ギリギリチキンレース\nスペースキーを押してチャージ\nスペースキーを離して投げる　";
			Slider_Percentage.gameObject.SetActive(true);
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

	private void PlayState()
	{
		Slider_Percentage.value = Miya_Controller_1.Get_MeterPercentage();

		if ( FinishGame )
		{
			_state = Miya_State_1.RESULT;
			_tenukiText.text = "リザルト\nEnterでゲームシーンへ戻る";
		}
	}

	private void ResltState()
	{
		if (Input.GetKeyDown(KeyCode.Return))
			_state = Miya_State_1.END;
	}

	private void EndState()
	{
		_miniGameConnection.EndMiniGame();
	}
}
