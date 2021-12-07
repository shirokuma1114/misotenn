//ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
//　カードの生成
//ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
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
        // カード情報リスト
        List<CardData> cardDataList = new List<CardData>();

        // forを回す回数を取得する
        int loopCnt = _imgList.Count;

        for (int i = 0; i < loopCnt; i++)
        {
            // カード情報を生成する
            CardData cardata = new CardData(i, _imgList[i]);
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
            Card card = Instantiate<Card>(this._cardPrefab, this.transform);
            // データを設定する
            card.Set(_cardData);
        }
        //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー

        //２列目ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
        List<CardData> SumCardDataList2 = new List<CardData>();
        SumCardDataList2.AddRange(cardDataList);
        // リストの中身をランダムに再配置する
        List<CardData> randomCardDataList2 = SumCardDataList2.OrderBy(a => System.Guid.NewGuid()).ToList();
        // カードオブジェクトを生成する
        foreach (var _cardData in randomCardDataList2)
        {
            // Instantiate で Cardオブジェクトを生成
            Card card = Instantiate<Card>(this._cardPrefab, this.transform);
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
            Card card = Instantiate<Card>(this._cardPrefab, this.transform);
            // データを設定する
            card.Set(_cardData);
        }
        //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー

        //ターンマネージャーに通知してゲームを開始させる
        _turnController.Init();
    }

    //指定した１枚のカードだけ取得する
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

    //カードを全て取得する
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

    //カーソル移動
    public void SetCursolCurd(int num, Color _col)
    {
        var _cards = GetCards();
        for (int i = 0; i < _cards.Length; i++)
        {
            if (i == num) _cards[i].CursolSet(true, _col);
            else _cards[i].CursolSet(false, new Color(1.0f, 1.0f, 1.0f));
        }
    }

    //カードをめくる
    public bool SetClickCurd(int num, int _charaId,int _nowStep)
    {
        Debug.Log("カードめくる：ID：" + _charaId);
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
                    Debug.Log(_enemyBrains[i].GetId() + "番が" + _nowStep +"ターン目の" + (GetCard(num).id) + "のカードをおぼえました");
                }
            }
            
            return false;
        }
    }

    //カードを裏返す(今までめくったカードの番号を配列に保持しておいてそれら全てを裏返す)
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

    //めくれないカードにする（敵がゴールした時呼び出す）
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