using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CasinoCard : MonoBehaviour
{
    private RectTransform _transform;
    private Image _cardSprite;

    private Tween _tween;

    private bool _animEnd;
    public bool IsAnimEnd => _animEnd;


    public void InitDisplay(int cardNumber,bool backCard = false)
    {
        _cardSprite.enabled = true;

        if (backCard)
            _transform.localRotation = new Quaternion(0, 180, 0, 0);

        _animEnd = false;
    }

    public void InitDisplay(Sprite numberSprite, bool backCard = false)
    {
        _cardSprite.enabled = true;

        _cardSprite.sprite = numberSprite;

        if (backCard)
            _transform.localRotation = new Quaternion(0, 180, 0, 0);

        _animEnd = false;
    }

    public void UnDisplay()
    {
        _cardSprite.enabled = false;
    }

    public void TrunUp()
    {
        _tween = _transform.DOLocalRotate(new Vector3(0, 0.0f, 0), 1.0f);
        _tween.OnComplete( () => { _animEnd = true; } );
    }

    //========================

    // Start is called before the first frame update
    void Start()
    {
        _transform = GetComponent<RectTransform>();
        _cardSprite = GetComponent<Image>();
        _cardSprite.enabled = false;

        UnDisplay();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnEnable()
    {
    }
}
