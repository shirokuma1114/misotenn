using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [SerializeField] private Animator fadeAnimater;
    [SerializeField] private bool isFadeIn;

    private bool isFadeOutStart;

    public enum SceneName
    {
        Title,
        Select,
        Game,
        Result
    }

    // enumからシーン名を取得するために必要
    private Dictionary<SceneName, string> m_sceneNameDictionary = new Dictionary<SceneName, string> {
        {SceneName.Title, "Title"},
        {SceneName.Select, "Select"},
        {SceneName.Game, "Game"},
        {SceneName.Result, "Result"},
    };

    [SerializeField] SceneName sceneName;

    // Start is called before the first frame update
    void Start()
    {
        if (isFadeIn == true) fadeAnimater.Play("FadeIn");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            fadeAnimater.Play("FadeOut");
            isFadeOutStart = true;
        }

        //if (isFadeOutStart == true)
        //{
        //    SceneManager.LoadScene(m_sceneNameDictionary[sceneName]);
        //}
    }
}
