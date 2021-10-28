using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
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
        {SceneName.Game, "Game"},
        {SceneName.Result, "Result"},
    };

    // ������Ă�ŃV�[���J�ڂ��s��
    public void LoadSceneEX(SceneName sceneName)
    {
        SceneManager.LoadScene(m_sceneNameDictionary[sceneName]);
    }

    [SerializeField] SceneName sceneName;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LoadSceneEX(sceneName);
        }
    }
}
