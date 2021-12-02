using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardGenerator : MonoBehaviour
{
    [SerializeField] Card cardPrefab;
    [SerializeField] RectTransform cardCreateParent;
    [SerializeField] List<Sprite> imgList;

    private CardsManager _cardsManager;

    void Start()
    {
        // �J�[�h��񃊃X�g
        List<CardData> cardDataList = new List<CardData>();

        // for���񂷉񐔂��擾����
        int loopCnt = imgList.Count;

        for (int i = 0; i < loopCnt; i++)
        {
            // �J�[�h���𐶐�����
            CardData cardata = new CardData(i, imgList[i]);
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
            Card card = Instantiate<Card>(this.cardPrefab, this.cardCreateParent);
            // �f�[�^��ݒ肷��
            card.Set(_cardData);
        }
        //�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[

        //�Q��ځ[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[
        List<CardData> SumCardDataList2= new List<CardData>();
        SumCardDataList2.AddRange(cardDataList);
        // ���X�g�̒��g�������_���ɍĔz�u����
        List<CardData> randomCardDataList2 = SumCardDataList2.OrderBy(a => System.Guid.NewGuid()).ToList();
        // �J�[�h�I�u�W�F�N�g�𐶐�����
        foreach (var _cardData in randomCardDataList2)
        {
            // Instantiate �� Card�I�u�W�F�N�g�𐶐�
            Card card = Instantiate<Card>(this.cardPrefab, this.cardCreateParent);
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
            Card card = Instantiate<Card>(this.cardPrefab, this.cardCreateParent);
            // �f�[�^��ݒ肷��
            card.Set(_cardData);
        }
        //�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[
    
        //�J�[�h�𐶐����I�������J�[�h�}�l�[�W���[�ɕ�
    }
}
