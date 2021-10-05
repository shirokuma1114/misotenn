using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIArrow : MonoBehaviour
{
    [SerializeField]
    private GameObject _canvas;

    private List<GameObject> _arrows = new List<GameObject>();

    [SerializeField]
    private GameObject _prefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Create(SquareBase[] squares)
    {
        return;
        foreach(var x in squares)
        {
            if (!x) continue;
            var obj = Instantiate(_prefab, new Vector3(0, 0, 0), Quaternion.identity);
            obj.transform.SetParent(_canvas.transform);
            obj.GetComponent<RectTransform>().anchoredPosition = new Vector3();
            _arrows.Add(obj);
        }
    }

    public void Delete()
    {
        for (int i = 0; i < _arrows.Count; i++)
        {
            Destroy(_arrows[i]);
        }

    }
}
