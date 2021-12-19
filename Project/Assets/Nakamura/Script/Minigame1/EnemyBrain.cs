using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyBrain : Brain
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

    float _beforeTrigger;


    void Start()
    {
        isControl = false;
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

    void Update()
    {
        if (isControl)
        {
            //���삪�ł���
            if (_miniGameChara.Input == null) return;
            float viewButton = _miniGameChara.Input.GetAxis("Horizontal");
            if (_beforeTrigger == 0.0f)
            {
                if (viewButton < 0)
                {
                    if (_nowCursol > ((_nowStep - 1) * 4)) _nowCursol -= 1;
                    _cardMgr.SetCursolCurd(_nowCursol, _myColor);

                    if (Control_SE.Get_Instance()) Control_SE.Get_Instance().Play_SE("UI_Select");
                }
                if (viewButton > 0)
                {
                    if (_nowCursol < _nowStep * 4 - 1) _nowCursol += 1;
                    _cardMgr.SetCursolCurd(_nowCursol, _myColor);

                    if (Control_SE.Get_Instance()) Control_SE.Get_Instance().Play_SE("UI_Select");
                }
            }
            _beforeTrigger = viewButton;
            
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (_nowCursol > ((_nowStep - 1) * 4)) _nowCursol -= 1;
                _cardMgr.SetCursolCurd(_nowCursol, _myColor);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (_nowCursol < _nowStep * 4 - 1) _nowCursol += 1;
                _cardMgr.SetCursolCurd(_nowCursol, _myColor);
            }

            //�X�y�[�X�L�[��������Card�̊֐��Ă�
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || _miniGameChara.Input.GetButtonDown("A"))
            {
                if (_cardMgr.GetCard(_nowCursol).isCanTurn == false) return;

                isControl = false;

                //�J�[�\���ʒu�ƏƂ炵���킹�Ă߂������J�[�h�̔ԍ��������Ēu��
                _turnCard[_nowStep - 1] = _nowCursol;

                //���딻��
                var _isCorrect = _cardMgr.SetClickCurd(_nowCursol, _myId, _nowStep);

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

                        Debug.Log("�������G�]��");
                        return;
                    }
                    else
                    {
                        //�J�[�\���̈ʒu�����̒i�̍��[�ɂ���
                        _nowCursol = (_nowStep - 1) * 4;
                        _cardMgr.SetCursolCurd(_nowCursol, _myColor);

                        DOVirtual.DelayedCall(0.5f, () => isControl = true);
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

        if(_miniGameChara.IsAutomatic == false) isControl = true;
    }

    //�J�[�h���߂��鏈�� �ċA������
    public void TurnCard()
    {
        //�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[
        //�@�I�ԃJ�[�h�����߂�
        //�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[
        var _isRandom = true;
        var _selectCard = -1;//�I�ԃJ�[�h

        //��x�ł������̃J�[�h�����������Ƃ����邩�H
        //���������Ƃ�����
        if(_correctMemory[_nowStep - 1] != -1)
        {
           var ran = Random.Range(0.0f, 1.0f);
            //�����̃J�[�h�̈ʒu���o���Ă�
            if (ran < _correctMemoryRate) _selectCard = _correctMemory[_nowStep - 1];
            //�����̃J�[�h�̈ʒu���o���ĂȂ������S�����_����
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
        //�]���ɐ������Ȃ��ꍇ
        else
        {
            //�����̃J�[�h���߂���ꂽ�̂��������Ƃ�����
            if (lookMemory[_nowStep - 1] != -1)
            {
                var ranLook = Random.Range(0.0f, 1.0f);
                //�����̃J�[�h�̈ʒu���v���o����
                if (ranLook < _lookMemoryRate)
                {
                    _selectCard = lookMemory[_nowStep - 1];
                }
                //�v���o���Ȃ����������S�����_����
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
            //�����̃J�[�h�̂��肩���݂����Ƃ��Ȃ������S�����_����
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
        //�@�J�[�\���ړ�
        //�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[
        var _dis = _selectCard -_nowCursol;
     
        if(_dis != 0)
        {
            StartCoroutine(CursolChange(_dis));
        }

        //�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[
        //�@�Q�b�l���āA�߂���
        //�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[
        DOVirtual.DelayedCall(2, () =>
        {
            _nowCursol = _selectCard;
            _cardMgr.SetCursolCurd(_nowCursol, _myColor);

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

    //�J�[�\���ړ��R���[�`��
    private IEnumerator CursolChange(int _dis)
    {
        while (true) // ����GameObject���L���ȊԎ��s��������
        {
            yield return new WaitForSeconds(0.8f);

            // ��Ŏw�肵���b���Ɏ��s����
            if (_dis < 0)
            {
                _nowCursol -= 1;
                _dis += 1;
            }
            else
            {
                _nowCursol += 1;
                _dis -= 1;
            }
            _cardMgr.SetCursolCurd(_nowCursol, _myColor);

            if (_dis == 0) yield break;
        }
    }

    public int GetId() { return _myId; } 

    //AI�̐��x��������A�C�f�A�i�������ĂȂ���j
    //�E�����̃J�[�h���������Ƃ��Ȃ��Ƃ��O���m��
    //�E���̐l���J�[�h���߂��������A���̂Ƃ��̈ʒu���������ɓ���Ă���
    //�E���̎����̃J�[�h�ȊO�̈ʒu�Ɗo���Ă���m�����킯��
}