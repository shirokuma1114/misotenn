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
        // カード情報リスト
        List<CardData> cardDataList = new List<CardData>();

        // forを回す回数を取得する
        int loopCnt = imgList.Count;

        for (int i = 0; i < loopCnt; i++)
        {
            // カード情報を生成する
            CardData cardata = new CardData(i, imgList[i]);
            cardDataList.Add(cardata);
        }

        // プレイヤーはカードを３枚揃えるのが目的ーーーーーーー下記関数化したらバグったのでこのまま
        //１列目ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
        List<CardData> SumCardDataList = new List<CardData>();
        SumCardDataList.AddRange(cardDataList);
        // リストの中身をランダムに再配置する
        List<CardData> randomCardDataList = SumCardDataList.OrderBy(a => System.Guid.NewGuid()).ToList();
        // カードオブジェクトを生成する
        foreach (var _cardData in randomCardDataList)
        {
            // Instantiate で Cardオブジェクトを生成
            Card card = Instantiate<Card>(this.cardPrefab, this.cardCreateParent);
            // データを設定する
            card.Set(_cardData);
        }
        //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー

        //２列目ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
        List<CardData> SumCardDataList2= new List<CardData>();
        SumCardDataList2.AddRange(cardDataList);
        // リストの中身をランダムに再配置する
        List<CardData> randomCardDataList2 = SumCardDataList2.OrderBy(a => System.Guid.NewGuid()).ToList();
        // カードオブジェクトを生成する
        foreach (var _cardData in randomCardDataList2)
        {
            // Instantiate で Cardオブジェクトを生成
            Card card = Instantiate<Card>(this.cardPrefab, this.cardCreateParent);
            // データを設定する
            card.Set(_cardData);
        }
        //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー

        //３列目ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
        List<CardData> SumCardDataList3 = new List<CardData>();
        SumCardDataList3.AddRange(cardDataList);
        // リストの中身をランダムに再配置する
        List<CardData> randomCardDataList3 = SumCardDataList3.OrderBy(a => System.Guid.NewGuid()).ToList();
        // カードオブジェクトを生成する
        foreach (var _cardData in randomCardDataList3)
        {
            // Instantiate で Cardオブジェクトを生成
            Card card = Instantiate<Card>(this.cardPrefab, this.cardCreateParent);
            // データを設定する
            card.Set(_cardData);
        }
        //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    
        //カードを生成し終わったらカードマネージャーに報告
    }
}
