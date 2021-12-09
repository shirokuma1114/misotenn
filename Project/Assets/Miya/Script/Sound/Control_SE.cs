using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control_SE : MonoBehaviour
{
	// singleton
	static Control_SE Myself = null;
	static public Control_SE Get_Instance() { return Myself; }


	// �O������ǂݍ��ފ֐�
	public void Play_SE(string _name)
	{
		Sound_List content = Sound_Contents.Find(l => l.Name == _name);
		Sound.clip = content.Audio;
		Sound.Play();
	}

	

	// Setting
	Setting_SoundUI SoundSetting;

	// Sound
	AudioSource Sound;
	float Initial_SoundVolume;

	// List
	public List<Sound_List> Sound_Contents = new List<Sound_List>();

	// Start
	void Start()
	{
		// singlegon
		if ( !Myself )
		{
			Myself = this.GetComponent<Control_SE>();
			DontDestroyOnLoad(this.gameObject);
		}
        else
        {
            Destroy(gameObject);
        }

		Sound = this.GetComponent<AudioSource>();
		Initial_SoundVolume = Sound.volume;
		Sound.volume = Initial_SoundVolume * Setting_SoundUI.Magnification_SE;

		// Event
		Setting_SoundUI.Event_Sound += time =>
		{
			Sound.volume = Initial_SoundVolume * Setting_SoundUI.Magnification_SE;
		};
	}
}
