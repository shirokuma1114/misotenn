using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [SerializeField] private Animator fadeAnimater;
    [SerializeField] private bool isFadeIn;
    [SerializeField] private int changeCou;

    private bool isFadeOutStart;

    public enum SceneName
    {
        Title,
        Select,
        Game,
        Result
    }

    // enum����V�[�������擾���邽�߂ɕK�v
    private Dictionary<SceneName, string> m_sceneNameDictionary = new Dictionary<SceneName, string> {
        {SceneName.Title, "Title"},
        {SceneName.Select, "Select"},
        {SceneName.Game, "NewTincleScene"},
        {SceneName.Result, "Result"},
    };

    [SerializeField] SceneName sceneName;

    private int cou = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (isFadeIn) fadeAnimater.Play("FadeIn");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            fadeAnimater.Play("FadeOut");
            isFadeOutStart = true;
        }

        if (isFadeOutStart) cou++;

        if (cou > changeCou && isFadeOutStart)
        {
            //�����ŏI�����Ăяo��

        }

    }
}
