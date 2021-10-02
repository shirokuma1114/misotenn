using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip _clip1;
    [SerializeField]
    private AudioClip _clip2;

    private bool _change;

    private AudioSource _audioSourceMain;
    private AudioSource _audioSourceWait;
    private int _playingSourceIndex;

    private float _fream = 0;

    // Start is called before the first frame update
    void Start()
    {
        var audioSources = GetComponents<AudioSource>();
        _audioSourceMain = audioSources[0];
        _audioSourceWait = audioSources[1];
        _playingSourceIndex = 0;

        _audioSourceMain.clip = _clip1;
        _audioSourceMain.Play();

        _audioSourceWait.clip = _clip2;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0) && !_change)
        {
            _change = true;

            _fream = 0;

            _audioSourceWait.volume = 0;
            _audioSourceWait.Play();
        }


        if(_change)
        {
            _audioSourceMain.volume = 1 - _fream;
            _audioSourceWait.volume = _fream;


            if (_fream > 1)
            {
                _change = false;
                
                var tmp = _audioSourceMain;
                _audioSourceMain = _audioSourceWait;
                _audioSourceWait = tmp;
            }

            _fream += Time.deltaTime;
        }
    }


    //=================================
    //public
    //=================================
    public void ChangeBGM(int clipIndex)
    {
        
    }
}
