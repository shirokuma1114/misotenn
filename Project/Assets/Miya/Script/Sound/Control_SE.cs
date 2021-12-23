using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control_SE : MonoBehaviour
{
	// singleton
	static Control_SE Myself = null;
	static public Control_SE Get_Instance() { return Myself; }


	// �O������ǂݍ��ފ֐�
	public AudioSource Play_SE(string _name)
	{
        AudioSource source = null;

        //�󂢂Ă�AudioSource��T��
        bool seNumOverflow = true;
        for (int i = 0; i < SE_MAX_NUM;i++)
        {
            if(!_audioSources[i].isPlaying)
            {
                source = _audioSources[i];
                seNumOverflow = false;
                break;
            }
        }
        if (seNumOverflow)
        {
            Debug.Log("AudioSource����Ȃ���SE�Ȃ��ĂȂ�����SE_MAX_NUM���₵��");
            return null;
        }

		Sound_List content = Sound_Contents.Find(l => l.Name == _name);
        source.clip = content.Audio;
        source.volume = Setting_SoundUI.Magnification_SE * content.Volume;
        source.loop = content.Loop;
        source.Play();

        // Event
        Setting_SoundUI.Event_Sound += time =>
        {
            source.volume = Initial_SoundVolume * Setting_SoundUI.Magnification_SE;
        };

        //_audioSources.Add(source);

        return source;
    }
	public void Stop_SE(AudioSource source)
	{
        source.Stop();
	}


	// Setting
	Setting_SoundUI SoundSetting;

    // Sound
    private const int SE_MAX_NUM = 256;
	private AudioSource[] _audioSources;
	float Initial_SoundVolume;

	// List
	public List<Sound_List> Sound_Contents = new List<Sound_List>();

	// Start
	void Start()
	{
        _audioSources = new AudioSource[SE_MAX_NUM];

        for (int i = 0; i < SE_MAX_NUM; i++)
        {
            _audioSources[i] = gameObject.AddComponent<AudioSource>();
        }

        //// Initialize
        //Sound = this.GetComponent<AudioSource>();
        //Initial_SoundVolume = Sound.volume;
        //Sound.volume = Initial_SoundVolume * Setting_SoundUI.Magnification_SE;

        foreach (var i in Sound_Contents)
        {
            if (i.Volume == 0) i.Volume = 1.0f;
        }
    }

    private void OnEnable()
    {
        Myself = this;
        Debug.Log("Sound_SE�؂�ւ�");
    }


}
