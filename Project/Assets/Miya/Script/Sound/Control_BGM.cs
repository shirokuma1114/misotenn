using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control_BGM : MonoBehaviour
{
	// Setting
	public Setting_SoundUI SoundSetting;

	// Sound
	AudioSource Sound;
	float Initial_SoundVolume;

	// Start
	void Start()
    {
		Sound = this.GetComponent<AudioSource>();
		Initial_SoundVolume = Sound.volume;
		Sound.volume = Initial_SoundVolume * Setting_SoundUI.Magnification_BGM;

		// Event
		SoundSetting.Event_Sound += time =>
		{
			Sound.volume = Initial_SoundVolume * Setting_SoundUI.Magnification_BGM;
		};
	}
}
