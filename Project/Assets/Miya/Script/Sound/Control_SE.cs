using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control_SE : MonoBehaviour
{
	// singleton
	static Control_SE Myself = null;
	static public Control_SE Get_Instance() { return Myself; }


	// äOïîÇ©ÇÁì«Ç›çûÇﬁä÷êî
	public void Play_SE(string _name)
	{
		Sound_List content = Sound_Contents.Find(l => l.Name == _name);
		Sound.clip = content.Audio;
		Sound.volume = Initial_SoundVolume * Setting_SoundUI.Magnification_SE * content.Volume;
		Sound.loop = content.Loop;
		Sound.Play();
	}
	public void Stop_SE()
	{
		Sound.Stop();
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
		// Initialize
		Sound = this.GetComponent<AudioSource>();
		Initial_SoundVolume = Sound.volume;
		Sound.volume = Initial_SoundVolume * Setting_SoundUI.Magnification_SE;

		foreach(var i in Sound_Contents)
		{
			if (i.Volume == 0) i.Volume = 1.0f;
		}

		// Event
		Setting_SoundUI.Event_Sound += time =>
		{
			Sound.volume = Initial_SoundVolume * Setting_SoundUI.Magnification_SE;
		};
	}

    private void OnEnable()
    {
        Myself = this;
        Debug.Log("Sound_SEêÿÇËë÷Ç¶");
    }
}
