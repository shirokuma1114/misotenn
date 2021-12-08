using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Card : MonoBehaviour
{
    public int id { get; private set; }
    [SerializeField] private Sprite _backCard;//こっちは定数
    [SerializeField] private Image _cardImage;//こっちは変数

  
    private bool _isFront = false;// 選択されているか判定
    private bool _isCursoled = false;// カーソルがついているか判定
    private CardData mData;
    private RectTransform mRt;

    //public bool isTurnEnd { get; private set; }
    public bool isCanTurn { get; set; } //めくることのできるカードか？（ゴールした人がめくったかーどはめくれない）

    // カードの設定
    public void Set(CardData data)
    {
        this.mData = data;

        // ID(画像番号)を設定する
        this.id = data.id;

        // 表示する画像を設定する
        this._cardImage.sprite = _backCard;

        // 選択判定フラグを初期化する
        this._isFront = false;

        // 座標情報を取得しておく
        this.mRt = this.GetComponent<RectTransform>();

        isCanTurn = true;
    }

    public void BackCard()
    {

        // Dotweenで回転処理を行う
        this.mRt.DORotate(new Vector3(0f, 90f, 0f), 0.2f)
            // 回転が完了したら
            .OnComplete(() =>
            {
                // 選択判定フラグを有効にする
                this._isFront = false;

                // カードを表面にする
                this._cardImage.sprite = _backCard;

                // Y座標を元に戻す
                this.onReturnRotate();
            });
    }

    //クリックされたらめくる
    public void ClickCurd()
    {
        // カードが表面になっていた場合は無効
        if (this._isFront)
        {
            return;
        }

        // Dotweenで回転処理を行う
        this.mRt.DORotate(new Vector3(0f, 90f, 0f), 0.2f)
            // 回転が完了したら
            .OnComplete(() =>
            {
                // 選択判定フラグを有効にする
                this._isFront = true;

                // カードを表面にする
                this._cardImage.sprite = this.mData.imgSprite;

                // Y座標を元に戻す
                this.onReturnRotate();
            });
        
      //  isTurnEnd = true;
    }

    private void onReturnRotate()
    {
        this.mRt.DORotate(new Vector3(0f, 0f, 0f), 0.2f)
            // 回転が終わったら
            .OnComplete(() => {
            // 選択したCardIdを保存しよう！
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