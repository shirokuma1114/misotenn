using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control_BGM : MonoBehaviour
{
	// �O������Ăяo��
	public void FadeOut() { if (FadeState == FADE_STATE.NONE) FadeState = FADE_STATE.OUT; Count_Fade = Second_Fade; }
	public void FadeIn() { if (FadeState == FADE_STATE.NONE) FadeState = FADE_STATE.IN; }
	public float Get_FadeSecond() { return Second_Fade; }




	// Setting
	Setting_SoundUI SoundSetting;

	// Sound
	AudioSource Sound;
	float Initial_SoundVolume;

	// Fade
	enum FADE_STATE
	{
		IN,//0
		NONE,//1
		OUT//2
	}
	FADE_STATE FadeState = FADE_STATE.NONE;
	public float Second_Fade = 2.0f;
	float Count_Fade = 0;

	// Start
	void Start()
    {
		Sound = this.GetComponent<AudioSource>();
		Initial_SoundVolume = Sound.volume;
		Sound.volume = Initial_SoundVolume * Setting_SoundUI.Magnification_BGM;

		// Event
		Setting_SoundUI.Event_Sound += time =>
		{
			//Debug.Log("AudioSource : " + Sound);

			if (this) Sound = this.GetComponent<AudioSource>();
			if (this) Sound.volume = Initial_SoundVolume * Setting_SoundUI.Magnification_BGM;

			// Debug
			//Debug.Log("Initial : " + Initial_SoundVolume);
			//Debug.Log("Magnification : " + Setting_SoundUI.Magnification_BGM);
		};



		// Fade
		if (FadeState == FADE_STATE.NONE) FadeState = FADE_STATE.IN;
	}

	private void Update()
	{
		//if (Input.GetKeyUp(KeyCode.Space))
		//{
		//	FadeOut();
		//}
	}

	private void FixedUpdate()
	{
		float sound_base;
		switch (FadeState)
		{
			case FADE_STATE.IN:
				Count_Fade += Time.deltaTime; if (Count_Fade > Second_Fade) Count_Fade = Second_Fade;
				sound_base = Initial_SoundVolume * Setting_SoundUI.Magnification_BGM;
				Sound.volume = sound_base * (Count_Fade / Second_Fade);

				if (Count_Fade >= Second_Fade)
				{
					Count_Fade = 0;
					FadeState = FADE_STATE.NONE;
				}
				break;

			case FADE_STATE.NONE: break;

			case FADE_STATE.OUT:
				Count_Fade -= Time.deltaTime; if (Count_Fade < 0) Count_Fade = 0;
				sound_base = Initial_SoundVolume * Setting_SoundUI.Magnification_BGM;
				Sound.volume = sound_base * (Count_Fade / Second_Fade);// 1��0

				if (Count_Fade <= 0)
				{
					Count_Fade = 0;
					FadeState = FADE_STATE.NONE;
				}
				break;
		}
	}
}
