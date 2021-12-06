using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectManager : MonoBehaviour
{
    [SerializeField]
    Animator _fadeAnimation;

    bool _isFade;
    // Start is called before the first frame update
    void Start()
    {
        _fadeAnimation.Play("FadeIn");
    }

    // Update is called once per frame
    void Update()
    {
        if (_isFade && _fadeAnimation.GetCurrentAnimatorClipInfo(0)[0].clip.name == "FadeOut" && _fadeAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            SceneManager.LoadScene("NewTincleScene");
        }

        if (_isFade) return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            _fadeAnimation.Play("FadeOut");
            _isFade = true;
        }

    }
}
