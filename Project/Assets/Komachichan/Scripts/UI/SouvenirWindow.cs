using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SouvenirWindow : MonoBehaviour
{
    [SerializeField]
    Image _frame;

    [SerializeField]
    GameObject _prefab;

    [SerializeField]
    Sprite[] _textures;

    List<GameObject> _images = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetEnable(bool enable)
    {
        _frame.enabled = enable;

        if(!enable)
        {
            foreach (var x in _images)
                Destroy(x);

            _images.Clear();
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
            obj.GetComponent<RectTransform>().anchoredPosition = new Vector3(0.0f + typeId * 99.0f + offset, -147.0f - offset * 2.0f, 0.0f);
            obj.GetComponent<Image>().sprite = _textures[typeId];
            _images.Add(obj);
        }
    }
}
