using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Card : MonoBehaviour
{
    public int id { get; private set; }
    [SerializeField] private Sprite _backCard;//�������͒萔
    [SerializeField] private Image _cardImage;//�������͕ϐ�

  
    private bool _isFront = false;// �I������Ă��邩����
    private bool _isCursoled = false;// �J�[�\�������Ă��邩����
    private CardData mData;
    private RectTransform mRt;

    //public bool isTurnEnd { get; private set; }
    public bool isCanTurn { get; set; } //�߂��邱�Ƃ̂ł���J�[�h���H�i�S�[�������l���߂��������[�ǂ͂߂���Ȃ��j

    // �J�[�h�̐ݒ�
    public void Set(CardData data)
    {
        this.mData = data;

        // ID(�摜�ԍ�)��ݒ肷��
        this.id = data.id;

        // �\������摜��ݒ肷��
        this._cardImage.sprite = _backCard;

        // �I�𔻒�t���O������������
        this._isFront = false;

        // ���W�����擾���Ă���
        this.mRt = this.GetComponent<RectTransform>();

        isCanTurn = true;
    }

    public void BackCard()
    {

        // Dotween�ŉ�]�������s��
        this.mRt.DORotate(new Vector3(0f, 90f, 0f), 0.2f)
            // ��]������������
            .OnComplete(() =>
            {
                // �I�𔻒�t���O��L���ɂ���
                this._isFront = false;

                // �J�[�h��\�ʂɂ���
                this._cardImage.sprite = _backCard;

                // Y���W�����ɖ߂�
                this.onReturnRotate();
            });
    }

    //�N���b�N���ꂽ��߂���
    public void ClickCurd()
    {
        // �J�[�h���\�ʂɂȂ��Ă����ꍇ�͖���
        if (this._isFront)
        {
            return;
        }

        // Dotween�ŉ�]�������s��
        this.mRt.DORotate(new Vector3(0f, 90f, 0f), 0.2f)
            // ��]������������
            .OnComplete(() =>
            {
                // �I�𔻒�t���O��L���ɂ���
                this._isFront = true;

                // �J�[�h��\�ʂɂ���
                this._cardImage.sprite = this.mData.imgSprite;

                // Y���W�����ɖ߂�
                this.onReturnRotate();
            });
        
      //  isTurnEnd = true;
    }

    private void onReturnRotate()
    {
        this.mRt.DORotate(new Vector3(0f, 0f, 0f), 0.2f)
            // ��]���I�������
            .OnComplete(() => {
            // �I������CardId��ۑ����悤�I
            // GameStateController.Instance.SelectedCardIdList.Add(this.mData.Id);
            });
    }

    public void CursolSet(bool _isCursol,Color _col)
    {
        if(_isCursol == true)
        {
            _isCursoled = true;
            _cardImage.color = _col;
        }
        else
        {
            _isCursoled = false;
            _cardImage.color = _col;
        }
    }
}

public class CardData
{
    public int id { get; private set; }

    public Sprite imgSprite { get; private set; }

    public CardData(int _id, Sprite _sprite)
    {
        this.id = _id;
        this.imgSprite = _sprite;
    }
}