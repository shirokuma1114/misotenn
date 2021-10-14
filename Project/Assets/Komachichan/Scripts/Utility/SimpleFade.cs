using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SimpleFade : MonoBehaviour
{
    bool _isFade = false;  // �t�F�[�h������
    public bool IsFade
    {
        get { return _isFade; }
    }
    bool _isFadeOut = false; // �t�F�[�h�A�E�g���H
    public bool IsFadeOut
    {
        get { return _isFadeOut; }
    }

    int _frameCnt = 0;  // ���݂̃t���[��
    int _fadeFrame = 0; // �t�F�[�h�t���[��
    string _nextScene; //  ���̃V�[����
    string _additionScene;//�@���̉��Z�V�[����

    void Awake()
    {
        _isFade = false;
        _isFadeOut = false;
        _frameCnt = 0;
        _fadeFrame = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_isFade) return;
        _frameCnt++;

        var color = this.gameObject.GetComponent<Image>().color;
        //var elapsed_frame = m_frame_cnt - m_fade_frame;
        var elapsedFrame = _frameCnt;

        var e = elapsedFrame / (float)_fadeFrame;

        float alpha = 0.0f;

        alpha = _isFadeOut ? e : 1.0f - e;

        if (elapsedFrame >= _fadeFrame)
        {
            _isFade = false;
            e = 1.0f;
            if (_isFadeOut)
            {
                /*
                SceneManager.LoadScene(_nextScene);
                if (_additionScene != "NONE")
                    SceneManager.LoadScene(_additionScene, LoadSceneMode.Additive);
                    */
            }
        }
        if (_isFadeOut)
        {
            alpha = e;
        }
        else
        {
            alpha = 1.0f - e;
        }
        color.a = alpha;
        this.gameObject.GetComponent<Image>().color = color;
    }

    public void FadeStart(int fadeFrame = 20, bool fadeOut = false, string nextScene = "NONE", string additionScene = "NONE")
    {
        if (_isFade) return;

        //Debug.Log("�t�F�[�h�X�^�[�g");
        _isFade = true;
        _isFadeOut = fadeOut;
        _fadeFrame = fadeFrame;
        _nextScene = nextScene;
        _additionScene = additionScene;
        _frameCnt = 0;
    }
}
