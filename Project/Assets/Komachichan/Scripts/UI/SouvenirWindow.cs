using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SouvenirWindow : WindowBase
{
    [SerializeField]
    Image _frame;

    [SerializeField]
    RectTransform _frameRt;

    [SerializeField]
    GameObject _prefab;

    List<GameObject> _images = new List<GameObject>();

    float _defaultPosY;

    float _displayPosY;

    // Start is called before the first frame update
    void Start()
    {
        _displayPosY = _defaultPosY = _frameRt.anchoredPosition.y;
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

        if(!enable)
        {
            foreach (var x in _images)
                Destroy(x);

            _images.Clear();
            SetDisplayPositionY(_defaultPosY);
        }
    }

    public void SetSouvenirs(List<Souvenir> souvenirs)
    {

        var map = new Dictionary<int, int>();

        foreach (var x in souvenirs)
        {
            var typeId = (int)x.Type;
            var obj = Instantiate(_prefab, transform);

            if (map.ContainsKey(typeId))
            {
                map[typeId] = map[typeId] + 1;
            }
            else
            {
                map.Add(typeId, 1);
            }

            float offset = map[typeId] * 2.0f;

            //float offset = _images.Where(y => y.GetComponent<Souvenir>().Type == x.Type).Count();
            //obj.GetComponent<RectTransform>().anchoredPosition = new Vector3(-47.0f + typeId * 75.0f + offset, -147.0f + offset * 2.0f, 0.0f);
            obj.GetComponent<RectTransform>().anchoredPosition = new Vector3(-47.0f + typeId * 75.0f + offset, _displayPosY + 49.0f + offset * 2.0f, 0.0f);
            obj.GetComponent<Image>().sprite = x.Sprite;
            _images.Add(obj);
        }
    }
}
