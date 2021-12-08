using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrumpCreater : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _TrumpPrefabs;

    public GameObject Create(int number)
    {
        if (number > 13)
            return null;
        if (number < 1)
            return null;


        GameObject trump = Instantiate(_TrumpPrefabs[number - 1]);
        return trump;
    }

    public GameObject Create(int number,Transform parent)
    {
        if (number > 13)
            return null;
        if (number < 1)
            return null;


        GameObject trump = Instantiate(_TrumpPrefabs[number - 1],parent);
        return trump;
    }
}
