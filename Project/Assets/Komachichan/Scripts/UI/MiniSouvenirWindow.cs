using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniSouvenirWindow : WindowBase
{
    [SerializeField]
    Image _frame;

    [SerializeField]
    RectTransform _frameRt;

    [SerializeField]
    GameObject _prefab;

    [SerializeField]
    GameObject _countPrefab;

    List<GameObject> _images = new List<GameObject>();

    List<GameObject> _countTexts = new List<GameObject>();

    float _defaultPosY;

    float _displayPosY;
    // Start is called before the first frame update
    void Start()
    {
        _displayPosY = _defaultPosY = _frameRt.anchoredPosition.y;
        SetEnable(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDisplayPositionY(float posY)
    {
        _displayPosY = posY;
        var pos = _frameRt.anchoredPosition;
        pos.y = _displayPosY;
        _frameRt.anchoredPosition = pos;
    }

    public override void SetEnable(bool enable)
    {
        _frame.enabled = enable;

        if (!enable)
        {
            foreach (var x in _images)
                Destroy(x);

            foreach (var x in _countTexts)
                Destroy(x);

            _images.Clear();
            _countTexts.Clear();
            SetDisplayPositionY(_defaultPosY);
        }
    }

    public void SetSouvenirs(List<Souvenir> souvenirs)
    {
        var map = new Dictionary<int, int>();

        foreach (var x in souvenirs)
        {
            var typeId = (int)x.Type;
            var obj = Instantiate(_prefab, transform.parent);

            if (map.ContainsKey(typeId))
            {
                map[typeId] = map[typeId] + 1;
            }
            else
            {
                map.Add(typeId, 1);
            }

            float offset = map[typeId] * 2.0f;

            obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(_frameRt.anchoredPosition.x - 190.0f + typeId * 75.0f + offset, _displayPosY + 49.0f + offset * 2.0f);
            obj.GetComponent<Image>().sprite = x.Sprite;
            _images.Add(obj);
        }

        for (int i = 0; i < (int)SouvenirType.MAX_TYPE; i++)
        {
            if (!map.ContainsKey(i)) continue;
            var obj = Instantiate(_countPrefab, transform.parent);
            obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(_frameRt.anchoredPosition.x - 200.0f + i * 75.0f, _displayPosY + 20.0f);
            obj.GetComponent<Text>().text = map[i].ToString();
            _countTexts.Add(obj);
        }
    }
}
