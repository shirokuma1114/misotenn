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
        // �e�L�X�g�̕\�������ւ���
        _turnText.text = "���Ȃ��@�̔Ԃł�";
    }

    // Update is called once per frame
    void Update()
    {
        if (_isGameEnd)
        {
            //���U���g�L�����o�X���o��
        }
    }
}
