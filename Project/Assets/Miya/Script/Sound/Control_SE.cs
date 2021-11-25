using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control_SE : MonoBehaviour
{
	// ŠO•”‚©‚ç“Ç‚Ýž‚ÞŠÖ”
	public void Play_SE(string _name)
	{
		Sound_List content = Sound_Contents.Find(l => l.Name == _name);
		if (content != 0)
		{
			Sound.clip = content.Audio;
			Sound.Play();
		}
	}

	// Setting
	public Setting_SoundUI SoundSetting;

	// Sound
	AudioSource Sound;
	float Initial_SoundVolume;

	// List
	public List<Sound_List> Sound_Contents = new List<Sound_List>();

	// Start
	void Start()
	{
		Sound = this.GetComponent<AudioSource>();
		Initial_SoundVolume = Sound.volume;
		Sound.volume = Initial_SoundVolume * Setting_SoundUI.Magnification_SE;

		// Event
		SoundSetting.Event_Sound += time =>
		{
			Sound.volume = Initial_SoundVolume * Setting_SoundUI.Magnification_SE;
		};
	}
}
