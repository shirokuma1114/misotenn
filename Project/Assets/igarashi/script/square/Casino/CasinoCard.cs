using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CasinoCard : MonoBehaviour
{
    private RectTransform _transform;
    private Image _cardSprite;
    private Text _numberText;

    private Tween _tween;

    private bool _animEnd;
    public bool IsAnimEnd => _animEnd;


    public void InitDisplay(int cardNumber,bool backCard = false)
    {
        _cardSprite.enabled = true;
        _numberText.enabled = true;

        _numberText.text = cardNumber.ToString();

        if (backCard)
            _transform.localRotation = new Quaternion(0, 180, 0, 0);

        _animEnd = false;
    }

    public void UnDisplay()
    {
        _cardSprite.enabled = false;
        _numberText.enabled = false;
    }

    public void TrunUp()
    {
        _tween = _transform.DOLocalRotate(new Vector3(0, 0.0f, 0), 1.0f);
        _tween.SetAutoKill(false);
    }

    //========================

    // Start is called before the first frame update
    void Start()
    {
        _transform = GetComponent<RectTransform>();
        _cardSprite = GetComponent<Image>();
        _numberText = transform.Find("Number").GetComponent<Text>();

        UnDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        if (_tween != null)
            if (!_tween.IsPlaying())
            {
                _animEnd = true;
                _tween.Kill();
                _tween = null;
            }
    }

    private void OnEnable()
    {
        if (_tween != null)
            _tween.Kill();
    }
}
