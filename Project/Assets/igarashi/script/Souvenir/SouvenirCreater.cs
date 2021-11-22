using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SouvenirCreater : MonoBehaviour
{
    private static SouvenirCreater _instance;
    public static SouvenirCreater Instance => _instance;


    [SerializeField]
    private List<Souvenir> _souvenirs;


    public Souvenir CreateSouvenir(SouvenirType type)
    {
        return new Souvenir(_souvenirs.Find(t => t.Type == type));
    }


    private void Awake()
    {
        _instance = this;
    }
}
