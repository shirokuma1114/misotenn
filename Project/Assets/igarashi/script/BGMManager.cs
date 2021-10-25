using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    private bool _change = false;
    private float _leapCount = 0;
    [SerializeField]
    private float _leapSpeed;

    private AudioSource _audioSourcePlaying;
    private AudioSource _audioSourceNext;


    // Start is called before the first frame update
    void Start()
    {
        _change = false;
        _leapCount = 0;


        var audioSources = GetComponents<AudioSource>();
        _audioSourcePlaying = audioSources[0];
        _audioSourceNext = audioSources[1];        
    }

    // Update is called once per frame
    void Update()
    {
        if(_change)
        {
            _audioSourcePlaying.volume = 1 - _leapCount;
            _audioSourceNext.volume = _leapCount;


            if (_leapCount > 1)
            {
                _change = false;

                var tmp = _audioSourcePlaying;
                _audioSourcePlaying = _audioSourceNext;
                _audioSourceNext = tmp;
                _audioSourceNext.Stop();
            }

            _leapCount += Time.deltaTime * _leapSpeed;
        }
    }


    //=================================
    //public
    //=================================
    public void SetNextBGMClip(AudioClip clip)
    {
        _change = true;
        _leapCount = 0;

        _audioSourceNext.clip = clip;
        _audioSourceNext.volume = 0.0f;
        _audioSourceNext.Play();
    }
}
