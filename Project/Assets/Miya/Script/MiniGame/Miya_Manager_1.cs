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
		PLAY_RENDA,
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

	[SerializeField]
	private float _rendaTime;
	private float _rendaTimeCounter;

	[SerializeField]
	private float _playTime;
	public float PlayTime => _playTime;
	private float _playTimeCounter;
	public float PlayTimeCounter => _playTimeCounter;


	void Start()
	{
		int i = 0;
		foreach (var c in _miniGameConnection.Characters)
		{
			_miniGameControllers[i].Init(c, this);
			i++;
		}

		_rendaTimeCounter = 0;
	}

	void Update()
	{
		switch (_state)
		{
			case Miya_State_1.TUTORIAL:
				TutorialState();
				break;

			case Miya_State_1.PLAY_RENDA:
				PlayRendaState();
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
		_tenukiText.text = "ギリギリチキンレース\nEnterでプレイ";

		if (Input.GetKeyDown(KeyCode.Return))
			_state = Miya_State_1.PLAY_RENDA;
	}

	private void PlayRendaState()
	{
		_tenukiText.text = "ギリギリチキンレース\nタイミングでスペース";

		if (_rendaTimeCounter > _rendaTime)
		{
			//_playersを順位で並び替え
			_miniGameControllers.Sort
				(
					(a, b) => { return b.RendaCount - a.RendaCount; }
				);

			foreach (var p in _miniGameControllers)
			{
				p.Go();
			}

			_state = Miya_State_1.PLAY;
		}

		_rendaTimeCounter += Time.deltaTime;
	}

	private void PlayState()
	{
		_tenukiText.text = "シュルシュルシュル";

		if (_playTimeCounter > _playTime)
			_state = Miya_State_1.RESULT;

		_playTimeCounter += Time.deltaTime;
	}

	private void ResltState()
	{
		_tenukiText.text = "リザルト\nEnterでゲームシーンへ戻る";

		if (Input.GetKeyDown(KeyCode.Return))
			_state = Miya_State_1.END;
	}

	private void EndState()
	{
		_miniGameConnection.EndMiniGame();
	}
}
