using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyBrain : MonoBehaviour
{
    //�v���C���[����
    [SerializeField] private TurnController _turnController;
    [SerializeField] private CardManager _cardMgr;
    [SerializeField] private Color _myColor;
    [SerializeField] private int _myId;//�����̃J�[�h�摜
    private int _nowCursol;
    private int _nowStep;
    private int[] _turnCard = new int[3];//�߂������J�[�h
    public int correctAnswer { get; set; }//����
    public bool isControl { get; set; }

    //�G�Ȃ񂿂����AI�֘A
    [SerializeField] private float _correctMemoryRate;//��x�����������̂��L�����Ă�����m���i0.0 ~ 1.0�j
    [SerializeField] private float _lookMemoryRate;//���̐l���߂������̂��L�����Ă�����m���i0.0 ~ 1.0�j 
    private int[] _correctMemory = new int[3];//���������ꏊ���L������
    private int[] _incorrectMemory = new int[3];//�ԈႦ���ꏊ���L������
    public int[] lookMemory = new int[3]; //���̐l���߂������̂��ڂɓ������i�ꏊ������j

    void Start()
    {
        _nowStep = 1;
        correctAnswer = 0;
        
        //�]���������
        for(int i = 0;i < 3;i++)
        {
            _correctMemory[i] = -1;
            _incorrectMemory[i] = -1;
            lookMemory[i] = -1;
        }
    }
        
    //�^�[���̍ŏ��̏���������
    public void StartTurn()
    {
        //�J�[�\���ݒ�
        _nowStep = 1;
        _nowCursol = 0;
        _cardMgr.SetCursolCurd(_nowCursol, _myColor);

        //�߂���ꂽ�J�[�h�̏�����
        for (int i = 0; i < 3; i++)
        {
            _turnCard[i] = -1;
        }

        isControl = true;
    }

    //�J�[�h���߂��鏈�� �ċA������
    public void TurnCard()
    {
        //�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[
        //�@�I�ԃJ�[�h�����߂�
        //�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[
        var _isRandom = true;
        var _selectCard = -1;//�I�ԃJ�[�h
        //�]���������ɐ��������邩�T��
        if(_correctMemory[_nowStep - 1] != -1)
        {
           var ran = Random.Range(0.0f, 1.0f);
            if (ran < _correctMemoryRate) _selectCard = _correctMemory[_nowStep - 1];
            else
            {
                if (lookMemory[_nowStep - 1] != -1)
                {
                    var ranLook = Random.Range(0.0f, 1.0f);
                    if (ranLook < _lookMemoryRate)
                    {
                        _selectCard = lookMemory[_nowStep - 1];
                    }
                    else
                    {
                        var ran2 = Random.Range(0, 4);
                        _selectCard = ran2 + ((_nowStep - 1) * 4);

                        //�O�ɓ����ԈႢ�����Ă������������
                        for (int i = 0; i < _incorrectMemory.Length; i++)
                        {
                            if (_incorrectMemory[i] == _selectCard)
                            {
                                TurnCard();
                                return;
                            }
                        }

                        //���ɏ������G���߂����Ă���J�[�h�Ȃ��������
                        if (_cardMgr.GetCard(_selectCard).isCanTurn == false)
                        {
                            TurnCard();
                            return;
                        }
                    }
                }
                else
                {
                    if (lookMemory[_nowStep - 1] != -1)
                    {
                        var ranLook = Random.Range(0.0f, 1.0f);
                        if (ranLook < _lookMemoryRate)
                        {
                            _selectCard = lookMemory[_nowStep - 1];
                        }
                        else
                        {
                            var ran2 = Random.Range(0, 4);
                            _selectCard = ran2 + ((_nowStep - 1) * 4);

                            //�O�ɓ����ԈႢ�����Ă������������
                            for (int i = 0; i < _incorrectMemory.Length; i++)
                            {
                                if (_incorrectMemory[i] == _selectCard)
                                {
                                    TurnCard();
                                    return;
                                }
                            }

                            //���ɏ������G���߂����Ă���J�[�h�Ȃ��������
                            if (_cardMgr.GetCard(_selectCard).isCanTurn == false)
                            {
                                TurnCard();
                                return;
                            }
                        }
                    }
                    else
                    {
                        var ran2 = Random.Range(0, 4);
                        _selectCard = ran2 + ((_nowStep - 1) * 4);

                        //�O�ɓ����ԈႢ�����Ă������������
                        for (int i = 0; i < _incorrectMemory.Length; i++)
                        {
                            if (_incorrectMemory[i] == _selectCard)
                            {
                                TurnCard();
                                return;
                            }
                        }

                        //���ɏ������G���߂����Ă���J�[�h�Ȃ��������
                        if (_cardMgr.GetCard(_selectCard).isCanTurn == false)
                        {
                            TurnCard();
                            return;
                        }
                    }
                }
            }
        }
        else
        {
            if (lookMemory[_nowStep - 1] != -1)
            {
                var ranLook = Random.Range(0.0f, 1.0f);
                if (ranLook < _lookMemoryRate)
                {
                    _selectCard = lookMemory[_nowStep - 1];
                }
                else
                {
                    var ran2 = Random.Range(0, 4);
                    _selectCard = ran2 + ((_nowStep - 1) * 4);

                    //�O�ɓ����ԈႢ�����Ă������������
                    for (int i = 0; i < _incorrectMemory.Length; i++)
                    {
                        if (_incorrectMemory[i] == _selectCard)
                        {
                            TurnCard();
                            return;
                        }
                    }

                    //���ɏ������G���߂����Ă���J�[�h�Ȃ��������
                    if (_cardMgr.GetCard(_selectCard).isCanTurn == false)
                    {
                        TurnCard();
                        return;
                    }
                }
            }
            else
            {
                var ran2 = Random.Range(0, 4);
                _selectCard = ran2 + ((_nowStep - 1) * 4);

                //�O�ɓ����ԈႢ�����Ă������������
                for (int i = 0; i < _incorrectMemory.Length; i++)
                {
                    if (_incorrectMemory[i] == _selectCard)
                    {
                        TurnCard();
                        return;
                    }
                }

                //���ɏ������G���߂����Ă���J�[�h�Ȃ��������
                if (_cardMgr.GetCard(_selectCard).isCanTurn == false)
                {
                    TurnCard();
                    return;
                }
            }
        }

        //�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[
        //�@�ړ�
        //�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[
        DOVirtual.DelayedCall(2, () =>
        {
            _nowCursol = _selectCard;
            _cardMgr.SetCursolCurd(_nowCursol, _myColor);

            //�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[
            //�@�߂���
            //�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[
            isControl = false;
            //�J�[�\���ʒu�ƏƂ炵���킹�Ă߂������J�[�h�̔ԍ��������Ēu��
            _turnCard[_nowStep - 1] = _nowCursol;
            //���딻��
            var _isCorrect = _cardMgr.SetClickCurd(_nowCursol, _myId, _nowStep);

            if (_isCorrect)
            {
                //���𐔍X�V
                _correctMemory[_nowStep - 1] = _nowCursol;//�]�̃������ɓ����
                if (correctAnswer < _nowStep)//�O�ɐ������Ă��Ȃ��Ƃ���𐳉�������
                {
                    correctAnswer = _nowStep;
                    
                    //�ԈႢ�������[������
                    for (int i = 0; i < 3; i++)
                    {
                        _incorrectMemory[i] = -1;
                    }
                }
                _nowStep += 1;

                //������
                if (correctAnswer == 3)
                {
                    _turnController.SetWinner(_myId);
                    _cardMgr.SetCardCantTurn(_correctMemory);//�J�[�h���Ԃ��Ȃ�����
                    _turnController.TurnChange();

                    Debug.Log("�������G�]��");
                    return;
                }
                else
                {
                    //�J�[�\���̈ʒu�����̒i�̍��[�ɂ���
                    _nowCursol = (_nowStep - 1) * 4;
                    _cardMgr.SetCursolCurd(_nowCursol, _myColor);

                    DOVirtual.DelayedCall(0.5f, () =>
                    {
                        isControl = true;
                        TurnCard();
                    });
                }
            }
            else
            {
                //�ԈႢ�������[�ɓ����
                for (int i = 0; i < 3; i++)
                {
                    if (_incorrectMemory[i] == -1) _incorrectMemory[i] = _nowCursol;
                }

                //�^�[���I������
                DOVirtual.DelayedCall(2, () =>
                {
                    _cardMgr.ResetCards(_turnCard);
                    _turnController.TurnChange();
                });
            }
        });
    }

    public int GetId() { return _myId; } 

    //���̐l�������̃J�[�h���߂����Ď����̈ʒu�ɂ���鏈���~������
}