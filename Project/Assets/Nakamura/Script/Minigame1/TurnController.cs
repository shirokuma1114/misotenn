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
    [SerializeField] private MiniGameConnection _miniGameCorrection;
    [SerializeField] private MiniGameResult _miniGameResult;

    public int[] gameOrder = new int[4];//0�`�R�̃L�����N�^�[ID������
    private int[] _winner = new int[4];//�������l��ID�����Ă���
    
    private int _nowTurnOrder = 0;
    private bool _isGameEnd = false;
    
   public void Init()
   {
        //�Q�[���̏��Ԃ����߂�
        turnRoulette(gameOrder);
       
        for (int i = 0; i < _winner.Length; i++)
        {
            _winner[i] = -1;
        }
        _turnText.text = "�̔Ԃł�";
        TurnChange();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isGameEnd)
        {
            //���U���g���o��
            _miniGameCorrection.EndMiniGame();
            //_miniGameResult.
            //_miniGameResult.Display(_winner);
        }
    }

    //�^�[����ς���֐�(�L�����N�^�[���Ăяo��)
    public void TurnChange()
    {
        if (_isGameEnd) return;

        //���̃^�[���Ɣ�ׂĈʒu��c�����Ă���ɂ���Ĉړ����ς���
        var _allowPosY = 0.0f;
        var rect = _nowTurnArrow.gameObject.GetComponent<RectTransform>();

        //���������Ă�l�Ȃ�X�L�b�v����
        var _isSkip = false;
        for(int i = 0;i < _winner.Length;i++)
        {
            if(_winner[i] == gameOrder[_nowTurnOrder])
            {
                _isSkip = true;
                break;
            }
        }
        if(_isSkip)
        {
            Debug.Log("�X�L�b�v" + gameOrder[_nowTurnOrder]);
            if (_nowTurnOrder < 3) _nowTurnOrder += 1;
            else _nowTurnOrder = 0;
            TurnChange();//�ċA...
            return;
        }

        Debug.Log(gameOrder[_nowTurnOrder] + "�̃^�[��");

        switch (gameOrder[_nowTurnOrder])
        {
            //�v���C���[
            case 0:
                _turnText.text = "���܂��В��@�̔Ԃł�";
                rect.localPosition = new Vector3(-270, 180, 0);

                _player.StartTurn();
                break;

            //�G�l�~�[�P
            case 1:
                _turnText.text = "�G�P���@�̔Ԃł�";
                rect.localPosition = new Vector3(-270, 140, 0);
                _enemy[0].StartTurn();
                _enemy[0].TurnCard();
                break;

            //�G�l�~�[�Q
            case 2:
                _turnText.text = "�G�Q���@�̔Ԃł�";
                rect.localPosition = new Vector3(-270, 100, 0);

                _enemy[1].StartTurn();
                _enemy[1].TurnCard();
                break;

            //�G�l�~�[�R
            case 3:
                _turnText.text = "�G�R���@�̔Ԃł�";
                rect.localPosition = new Vector3(-270, 60, 0);

                _enemy[2].StartTurn();
                _enemy[2].TurnCard();
                break;
        }


        if (_nowTurnOrder < 3) _nowTurnOrder += 1;
        else _nowTurnOrder = 0;
    }

    //�������l��ۑ�
    public void SetWinner(int _id)
    {
        for(int i = 0;i < _winner.Length;i++)
        {
            
            if (_winner[i] == -1)
            {
                _winner[i] = _id;
                if (i == _winner.Length - 1) _isGameEnd = true;
                Debug.Log((i + 1) + "�Ԗڂ�" + _id);
                break;
            }
        }
    }
    
    //���[���b�g�p�A�j���[�V�����֐�
    private int[] turnRoulette(int[] _order)
    {
        int ran = Random.Range(0, 4);

        for(int i = 0;i < _order.Length;i++)
        {
            if(ran + i > 3) _order[i] = (ran + i) - 4;
            else _order[i] = ran + i;
        }

        return _order;
    }
}