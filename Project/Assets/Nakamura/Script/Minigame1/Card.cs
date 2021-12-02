using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private Image cardImage;

    private bool mIsSelected = false;// �I������Ă��邩����
    private bool mIsCursoled = false;// �J�[�\�������Ă��邩����
    private CardData mData;
    
    public bool isTurnEnd { get; private set; }         

    // �J�[�h�̐ݒ�
    public void Set(CardData data)
    {
        this.mData = data;

        // ID��ݒ肷��
        this.id = data.id;

        // �\������摜��ݒ肷��
        this.cardImage.sprite = cardImage.sprite;

        // �I�𔻒�t���O������������
        this.mIsSelected = false;
    }

    //�N���b�N���ꂽ��߂���
    public void OnClick()
    {
        // �J�[�h���\�ʂɂȂ��Ă����ꍇ�͖���
        if (this.mIsSelected)
        {
            return;
        }

        // �I�𔻒�t���O��L���ɂ���
        this.mIsSelected = true;

        // �J�[�h��\�ʂɂ���
        this.cardImage.sprite = this.mData.imgSprite;

        isTurnEnd = true;
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