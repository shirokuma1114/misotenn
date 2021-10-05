using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class RouletteUI : MonoBehaviour
{
    public static readonly int ROULETEE_MAX = 2;

    [SerializeField]
    float _moveSpeed;

    private CharacterBase _character;

    [SerializeField]
    GameObject _itemPrefab;

    [SerializeField]
    Image _frameImage;

    [SerializeField]
    RectTransform _frameTr;

    [SerializeField]
    Image _backImage;

    List<RouletteItemBase> _rouletteItems = new List<RouletteItemBase>();

    List<GameObject> _rouletteObjects = new List<GameObject>();

    bool _isDisplayed = false;

    bool _isPushed = false;

    bool _isStopped = false;

    float _offsetY;

    RouletteItemBase _selectedItem;

    // Start is called before the first frame update
    void Start()
    {
        _offsetY = _frameTr.anchoredPosition.y;

        _frameImage.enabled = false;
        _backImage.enabled = false;
        _selectedItem = null; 
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isDisplayed) return;
        UpdatePush();
        UpdateMove();
    }

    void UpdateMove()
    {
        if (_isStopped) return;
        foreach(var i in _rouletteObjects)
        {
            var pos = i.GetComponent<RectTransform>().anchoredPosition;
            pos.y -= Time.deltaTime * _moveSpeed;
            if(pos.y - _offsetY <= -74.0f)
            {
                pos.y = -64.0f + _rouletteObjects.Count * 32.0f - 10.0f + _offsetY;
                i.GetComponent<Text>().enabled = false;
            }

            i.GetComponent<RectTransform>().anchoredPosition = pos;

            if(pos.y - _offsetY <= 74.0f)
            {
                i.GetComponent<Text>().enabled = true;
            }
        }

        if (_isPushed)
        {
            var pos = _rouletteObjects[0].GetComponent<RectTransform>().anchoredPosition;
            if (Mathf.Abs((pos.y - _offsetY) % 32.0f) <= 0.8f)
            {
                _isStopped = true;
                float min = float.MaxValue;
                foreach(var i in _rouletteObjects)
                {
                    var s = Mathf.Abs(_offsetY - i.GetComponent<RectTransform>().anchoredPosition.y);
                    if(min > s)
                    {
                        _selectedItem = i.GetComponent<RouletteItemObject>().RouletteItem;
                        min = s;
                    }
                }
            }
        }
        
    }



    public RouletteItemBase GetSelectedItem()
    {
        return _selectedItem;
    }

    void UpdatePush()
    {
        if (!_isPushed && Input.GetKeyDown(KeyCode.Return))
        {
            _isPushed = true;
        }
    }

    public RouletteUI Begin(CharacterBase character)
    {
        _isDisplayed = true;
        _isPushed = false;
        _isStopped = false;
        _character = character;
        _frameImage.enabled = true;
        _backImage.enabled = true;
        _selectedItem = null;
        CreateTexts();

        return this;
    }

    public void End()
    {
        _isDisplayed = false;
        _rouletteItems.Clear();

        for (int i = 0; i < _rouletteObjects.Count; i++)
        {
            Destroy(_rouletteObjects[i]);
        }

        _frameImage.enabled = false;
        _backImage.enabled = false;
    }

    public RouletteUI AddItem(RouletteItemBase rouletteItem)
    {
        _rouletteItems.Add(rouletteItem);
        return this;
    }

    private void CreateTexts()
    {
        float posY = -64.0f + _offsetY;

        foreach(var i in _rouletteItems)
        {
            var obj = Instantiate(_itemPrefab, transform);
            obj.GetComponent<Text>().text = i.GetDisplayName();
            var pos = obj.GetComponent<RectTransform>().anchoredPosition;
            pos.y = posY;
            obj.GetComponent<RectTransform>().anchoredPosition = pos;
            obj.GetComponent<RouletteItemObject>().RouletteItem = i;
            _rouletteObjects.Add(obj);
            if(pos.y > 64.0f)
            {
                obj.GetComponent<Text>().enabled = false;
            }
            posY += 32.0f;

        }
        _frameTr.SetAsLastSibling();
    }
}
