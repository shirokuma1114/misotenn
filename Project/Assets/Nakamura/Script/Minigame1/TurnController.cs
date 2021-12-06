using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnController : MonoBehaviour
{
    [SerializeField] private Text _turnText;
    [SerializeField] private PlayerBrain _player;
    [SerializeField] private EnemyBrain[] _enemy; 
    [SerializeField] private Image _nowTurnArrow;

    public int[] gameOrder = new int[4];//0�`�R�̃L�����N�^�[ID������
    private int[] _winner = new int[4];//�������l��ID�����Ă���
    
    private int _nowTurnCharaId = 0;
    private bool _isGameEnd = false;
    
   public void Init()
    {
        //�Q�[���̏��Ԃ����߂�
        for(int i = 0;i < gameOrder.Length;i++)
        {
            gameOrder[i] = i;
            //��Ń����_��������
        }

        for (int i = 0; i < _winner.Length; i++)
        {
            _winner[i] = -1;
        }

        // �e�L�X�g�̕\�������ւ���
        _turnText.text = "���܂��В��@�̔Ԃł�";
        _player.StartTurn();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isGameEnd)
        {
            //���U���g�L�����o�X���o��
        }
    }

    //�^�[����ς���֐�(�L�����N�^�[���Ăяo��)
    public void TurnChange()
    {
        //���̃^�[���Ɣ�ׂĈʒu��c�����Ă���ɂ���Ĉړ����ς���
        var _allowPosY = 0.0f;
        var rect = _nowTurnArrow.gameObject.GetComponent<RectTransform>();

        if (_nowTurnCharaId < 3) _nowTurnCharaId += 1;
        else _nowTurnCharaId = 0;

        //���������Ă�l�Ȃ�X�L�b�v����
        for(int i = 0;i < _winner.Length;i++)
        {
            if(_winner[i] == _nowTurnCharaId)
            {
                Debug.Log("�X�L�b�v" + _nowTurnCharaId);
                TurnChange();//�ċA...
                return;
            }
        }

        switch (gameOrder[_nowTurnCharaId])
        {
            //�v���C���[
            case 0:
                _turnText.text = "���܂��В��@�̔Ԃł�";
     
                rect.localPosition += new Vector3(0, 120, 0);

                _player.StartTurn();
                break;

            //�G�l�~�[�P
            case 1:
                _turnText.text = "�G�P���@�̔Ԃł�";
                rect.localPosition += new Vector3(0, -40, 0);
                _enemy[0].StartTurn();
                _enemy[0].TurnCard();
                break;

            //�G�l�~�[�Q
            case 2:
                _turnText.text = "�G�Q���@�̔Ԃł�";
                rect.localPosition += new Vector3(0, -40, 0);

                _enemy[1].StartTurn();
                _enemy[1].TurnCard();
                break;

            //�G�l�~�[�R
            case 3:
                _turnText.text = "�G�R���@�̔Ԃł�";
                rect.localPosition += new Vector3(0, -40, 0);

                _enemy[2].StartTurn();
                _enemy[2].TurnCard();
                break;
        }
    }

    //�������l��ۑ�
    public void SetWinner(int _id)
    {
        for(int i = 0;i < _winner.Length;i++)
        {
            if (_winner[i] == -1)
            {
                _winner[i] = _id;
                break;
            }
        }
    }
    
    //���[���b�g�p�A�j���[�V�����֐�
}
