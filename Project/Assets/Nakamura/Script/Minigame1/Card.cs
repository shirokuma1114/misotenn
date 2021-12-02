using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private Image cardImage;

    private bool mIsSelected = false;// 選択されているか判定
    private bool mIsCursoled = false;// カーソルがついているか判定
    private CardData mData;
    
    public bool isTurnEnd { get; private set; }         

    // カードの設定
    public void Set(CardData data)
    {
        this.mData = data;

        // IDを設定する
        this.id = data.id;

        // 表示する画像を設定する
        this.cardImage.sprite = cardImage.sprite;

        // 選択判定フラグを初期化する
        this.mIsSelected = false;
    }

    //クリックされたらめくる
    public void OnClick()
    {
        // カードが表面になっていた場合は無効
        if (this.mIsSelected)
        {
            return;
        }

        // 選択判定フラグを有効にする
        this.mIsSelected = true;

        // カードを表面にする
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