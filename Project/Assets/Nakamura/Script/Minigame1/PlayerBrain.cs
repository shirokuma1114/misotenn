using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerBrain : Brain
{
    [SerializeField] private TurnController _turnController;
    [SerializeField] private CardManager _cardMgr;
    [SerializeField] private Color _myColor;
    private int _nowCursol;
    private int _nowStep;
    private int[] _turnCard = new int[3];//�߂������J�[�h
    private int[] _correctMemory = new int[3];//���������ꏊ���L������
    public int _myId { get; private set;}//�����̃J�[�h�摜
    public int correctAnswer { get; set; }//����
    public bool isControl { get; set; }
   

    void Start()
    {
        _myId = 0;
        correctAnswer = 0;
        for (int i = 0; i < 3; i++)
        {
            _correctMemory[i] = -1;
        }
    }
   
    void Update()
    {
        if (isControl)
        {
            Debug.Log("isControll = " + isControl);

            //���삪�ł���
            if (Input.GetKeyDown(KeyCode.A))
            {
                if(_nowCursol > ((_nowStep - 1) * 4))_nowCursol -= 1;
                _cardMgr.SetCursolCurd(_nowCursol, _myColor);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (_nowCursol < _nowStep * 4 - 1) _nowCursol += 1;
                _cardMgr.SetCursolCurd(_nowCursol, _myColor);
            }

            //�X�y�[�X�L�[��������Card�̊֐��Ă�
            if(Input.GetKeyDown(KeyCode.Space))
            {
                if (_cardMgr.GetCard(_nowCursol).isCanTurn == false) return;

                isControl = false;

                //�J�[�\���ʒu�ƏƂ炵���킹�Ă߂������J�[�h�̔ԍ��������Ēu��
                _turnCard[_nowStep - 1] = _nowCursol;

                //���딻��
                var _isCorrect = _cardMgr.SetClickCurd(_nowCursol,_myId, _nowStep);
                
                if (_isCorrect)
                {
                    //���𐔍X�V
                    _correctMemory[_nowStep - 1] = _nowCursol;//�]�̃������ɓ����
                    if (correctAnswer < _nowStep)
                    {
                        correctAnswer = _nowStep;
                    }
                    _nowStep += 1;

                    //������
                    if (correctAnswer == 3)
                    {
                        _turnController.SetWinner(_myId);
                        _cardMgr.SetCardCantTurn(_correctMemory);//�J�[�h���Ԃ��Ȃ�����
                        _turnController.TurnChange();

                        Debug.Log("�v���C���[������");
                        return;
                    }
                    else
                    {
                        //�J�[�\���̈ʒu�����̒i�̍��[�ɂ���
                        _nowCursol = (_nowStep - 1) * 4;
                        _cardMgr.SetCursolCurd(_nowCursol, _myColor);

                        isControl = true;
                    }
                }
                else
                {
                    //�^�[���I������
                    DOVirtual.DelayedCall(2, () =>
                    {
                        _cardMgr.ResetCards(_turnCard);
                        _turnController.TurnChange();
                    });
                }
            }
        }
    }

    //�v���C���[�̃^�[���̍ŏ��̏���������
    public void StartTurn()
    {
        _nowStep = 1;

        //�J�[�\���ݒ�
        _nowCursol = 0;
        _cardMgr.SetCursolCurd(_nowCursol, _myColor);

        //�߂���ꂽ�J�[�h�̏�����
        for(int i = 0;i < 3; i++)
        {
            _turnCard[i] = -1;
        }

        isControl = true;
    }
}