using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameRandomManager : MonoBehaviour
{
    [SerializeField] private MiniGameConnection _miniGameConnection;
    [SerializeField] private List<GameObject> _miniGameObj;
    [SerializeField] private float _speed = 10f; // 再生時間
    [SerializeField] private int _randMin = 10; // 再生時間
    [SerializeField] private int _randMax = 13; // 再生時間

    // Start is called before the first frame update
    void Start()
    {
        StartMiniGamneRand();
    }

    public void StartMiniGamneRand()
    {
        List<string> _miniGameList = new List<string>();

        // リスト格納
        foreach (var scene in _miniGameConnection.MiniGameSceneNames)
        {
            _miniGameList.Add(scene);
        }

        // ランダムで三つ残す
        while (_miniGameList.Count > 3)
        {
            _miniGameList.RemoveAt(Random.Range(0, _miniGameList.Count));
        }

        // テキスト表示
        for (int i = 0; i < _miniGameObj.Count; i++)
        {
            _miniGameObj[i].GetComponentInChildren<Text>().text = _miniGameList[i];
        }

        // ランダム演出スタート
        StartCoroutine(RandomStart());
    }

    public IEnumerator RandomStart()
    {
        int rotateCount = 0;
        int endCount = Random.Range(_randMin, _randMax);
        float delta = 0;

        _miniGameObj[rotateCount].GetComponent<Image>().color = new Color(1, 0, 0, 1.0f);

        while (rotateCount<= endCount)
        {
            delta += Time.deltaTime * _speed;
            
            if (delta >= 1f)
            {
                delta = 0f;
                rotateCount++;
                ClearObject();
                _miniGameObj[rotateCount%3].GetComponent<Image>().color = new Color(1f, 223/255f, 162/255f, 1.0f);
                if (rotateCount % 3 == 0) _speed *= 0.75f;
            }

            yield return null;
        }

        int decisionNum = rotateCount%3;

        rotateCount = 0;
        while (rotateCount <= 4)
        {
            delta += Time.deltaTime * 3.5f;

            if (delta >= 1f)
            {
                delta = 0f;
                rotateCount++;

                if (rotateCount % 2 == 0)
                    _miniGameObj[decisionNum].GetComponent<Image>().color = new Color(1, 1, 1, 1.0f);
                else
                    _miniGameObj[decisionNum].GetComponent<Image>().color = new Color(1f, 223 / 255f, 162 / 255f, 1.0f);

            }
            yield return null;
        }

    }

    void ClearObject()
    {
        for (int i = 0; i < _miniGameObj.Count; i++)
        {
            _miniGameObj[i].GetComponent<Image>().color = new Color(1, 1, 1, 1.0f);
        }
    }
}
