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

    // enumからシーン名を取得するために必要
    private Dictionary<SceneName, string> m_sceneNameDictionary = new Dictionary<SceneName, string> {
        {SceneName.Title, "Title"},
        {SceneName.Select, "Select"},
        {SceneName.Game, "NewTincleScene"},
        {SceneName.Result, "Result"},
    };

    [SerializeField] SceneName sceneName;

    // Start is called before the first frame update
    void Start()
    {
        if (isFadeIn) fadeAnimater.Play("FadeIn");
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Start") || Input.GetButtonDown("A")) && !isFadeOutStart)
        {
            FindObjectOfType<Control_BGM>().FadeOut();
            fadeAnimater.Play("FadeOut");
            isFadeOutStart = true;
            if (Control_SE.Get_Instance()) Control_SE.Get_Instance().Play_SE("UI_Correct");
        }

        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("B"))
        {
            UnityEngine.Application.Quit();
        }

        if (isFadeOutStart && fadeAnimater.GetCurrentAnimatorClipInfo(0)[0].clip.name == "FadeOut" 
            && fadeAnimater.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            //ここで終了を呼び出し
            SceneManager.LoadScene("Select");
        }

    }
}
