using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SimpleFade : MonoBehaviour
{
    bool _isFade = false;  // フェード中判定
    public bool IsFade
    {
        get { return _isFade; }
    }
    bool _isFadeOut = false; // フェードアウトか？
    public bool IsFadeOut
    {
        get { return _isFadeOut; }
    }

    int _frameCnt = 0;  // 現在のフレーム
    int _fadeFrame = 0; // フェードフレーム
    string _nextScene; //  次のシーン名
    string _additionScene;//　次の加算シーン名

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

        //Debug.Log("フェードスタート");
        _isFade = true;
        _isFadeOut = fadeOut;
        _fadeFrame = fadeFrame;
        _nextScene = nextScene;
        _additionScene = additionScene;
        _frameCnt = 0;
    }
}
