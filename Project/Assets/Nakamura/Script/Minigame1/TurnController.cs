using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnController : MonoBehaviour
{
    [SerializeField] private Text _turnText;
    //[SerializeField] private Text _turnText;
    private int _nowTurn = 0;
    private bool _isGameEnd = false;
    private 

    // Start is called before the first frame update
    void Start()
    {
        // テキストの表示を入れ替える
        _turnText.text = "あなた　の番です";
    }

    // Update is called once per frame
    void Update()
    {
        if (_isGameEnd)
        {
            //リザルトキャンバスを出す
        }
    }
}
