//�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[
//�@�J�[�h�̐���
//�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardManager : MonoBehaviour
{
    [SerializeField] private Card _cardPrefab;
    [SerializeField] private List<Sprite> _imgList;
    [SerializeField] private TurnController _turnController;
    [SerializeField] private EnemyBrain[] _enemyBrains;
    void Start()
    {
        // �J�[�h��񃊃X�g
        List<CardData> cardDataList = new List<CardData>();

        // for���񂷉񐔂��擾����
        int loopCnt = _imgList.Count;

        for (int i = 0; i < loopCnt; i++)
        {
            // �J�[�h���𐶐�����
            CardData cardata = new CardData(i, _imgList[i]);
            cardDataList.Add(cardata);
        }

        // �v���C���[�̓J�[�h���R��������̂��ړI�[�[�[�[�[�[�[���L�֐���������o�O�����̂ł��̂܂�
        //�P��ځ[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[
        List<CardData> SumCardDataList = new List<CardData>();
        SumCardDataList.AddRange(cardDataList);
        // ���X�g�̒��g�������_���ɍĔz�u����
        List<CardData> randomCardDataList = SumCardDataList.OrderBy(a => System.Guid.NewGuid()).ToList();
        // �J�[�h�I�u�W�F�N�g�𐶐�����
        foreach (var _cardData in randomCardDataList)
        {
            // Instantiate �� Card�I�u�W�F�N�g�𐶐�
            Card card = Instantiate<Card>(this._cardPrefab, this.transform);
            // �f�[�^��ݒ肷��
            card.Set(_cardData);
        }
        //�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[

        //�Q��ځ[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[
        List<CardData> SumCardDataList2 = new List<CardData>();
        SumCardDataList2.AddRange(cardDataList);
        // ���X�g�̒��g�������_���ɍĔz�u����
        List<CardData> randomCardDataList2 = SumCardDataList2.OrderBy(a => System.Guid.NewGuid()).ToList();
        // �J�[�h�I�u�W�F�N�g�𐶐�����
        foreach (var _cardData in randomCardDataList2)
        {
            // Instantiate �� Card�I�u�W�F�N�g�𐶐�
            Card card = Instantiate<Card>(this._cardPrefab, this.transform);
            // �f�[�^��ݒ肷��
            card.Set(_cardData);
        }
        //�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[

        //�R��ځ[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[
        List<CardData> SumCardDataList3 = new List<CardData>();
        SumCardDataList3.AddRange(cardDataList);
        // ���X�g�̒��g�������_���ɍĔz�u����
        List<CardData> randomCardDataList3 = SumCardDataList3.OrderBy(a => System.Guid.NewGuid()).ToList();
        // �J�[�h�I�u�W�F�N�g�𐶐�����
        foreach (var _cardData in randomCardDataList3)
        {
            // Instantiate �� Card�I�u�W�F�N�g�𐶐�
            Card card = Instantiate<Card>(this._cardPrefab, this.transform);
            // �f�[�^��ݒ肷��
            card.Set(_cardData);
        }
        //�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[

        //�^�[���}�l�[�W���[�ɒʒm���ăQ�[�����J�n������
        _turnController.Init();
    }

    //�w�肵���P���̃J�[�h�����擾����
    public Card GetCard(int num)
    {
        GameObject[] child = new GameObject[this.transform.childCount];
        var _card = new Card();

        for (int i = 0; i < this.transform.childCount; i++)
        {
            child[i] = this.transform.GetChild(i).gameObject;
            _card = child[i].GetComponent<Card>();
            if (i == num) break;
        }

        return _card;
    }

    //�J�[�h��S�Ď擾����
    public Card[] GetCards()
    {
        GameObject[] child = new GameObject[this.transform.childCount];
        var _cards = new Card[12];

        for (int i = 0; i < this.transform.childCount; i++)
        {
            child[i] = this.transform.GetChild(i).gameObject;
            _cards[i] = child[i].GetComponent<Card>();
        }

        return _cards;
    }

    //�J�[�\���ړ�
    public void SetCursolCurd(int num, Color _col)
    {
        var _cards = GetCards();
        for (int i = 0; i < _cards.Length; i++)
        {
            if (i == num) _cards[i].CursolSet(true, _col);
            else _cards[i].CursolSet(false, new Color(1.0f, 1.0f, 1.0f));
        }
    }

    //�J�[�h���߂���
    public bool SetClickCurd(int num, int _charaId,int _nowStep)
    {
        Debug.Log("�J�[�h�߂���FID�F" + _charaId);
        GetCard(num).ClickCurd();
        if (GetCard(num).id == _charaId)
        {
            return true;
        }
        else
        {
            for(int i = 0;i < _enemyBrains.Length;i++)
            {
                if ((GetCard(num).id) == _enemyBrains[i].GetId())
                {
                    _enemyBrains[i].lookMemory[_nowStep - 1] = num;
                    Debug.Log(_enemyBrains[i].GetId() + "�Ԃ�" + _nowStep +"�^�[���ڂ�" + (GetCard(num).id) + "�̃J�[�h�����ڂ��܂���");
                }
            }
            
            return false;
        }
    }

    //�J�[�h�𗠕Ԃ�(���܂ł߂������J�[�h�̔ԍ���z��ɕێ����Ă����Ă����S�Ă𗠕Ԃ�)
    public void ResetCards(int[] _cardsNum)
    {
        var _cards = GetCards();
        for (int i = 0; i < _cards.Length; i++)
        {
            for (int j = 0; j < _cardsNum.Length; j++)
            {
                if (_cardsNum[j] == i)
                {
                    _cards[i].BackCard();
                    break;
                }
            }
        }
    }

    //�߂���Ȃ��J�[�h�ɂ���i�G���S�[���������Ăяo���j
    public void SetCardCantTurn(int[] _cardsNum)
    {
        var _cards = GetCards();
        for (int i = 0; i < _cards.Length; i++)
        {
            for (int j = 0; j < _cardsNum.Length; j++)
            {
                if (_cardsNum[j] == i)
                {
                    _cards[i].isCanTurn = false;
                    break;
                }
            }
        }
    }
}