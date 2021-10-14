using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _selectionPrefab;
    private Rect DEFAULT_RECT = new Rect(244.0f,-75.0f,160,30);
    private Color SELECT_COLOR = new Color(1, 0, 0, 1);
    private Color NOT_SELECT_COLOR = new Color(1, 1, 1, 1);

    private List<string> _elements = new List<string>();
    private List<GameObject> _selections = new List<GameObject>();
    private int _selectIndex;
    public int SelectIndex => _selectIndex;
    private bool _selectComplete;
    public bool IsComplete => _selectComplete;


    // Start is called before the first frame update
    void Start()
    {
        _selectIndex = 0;
        _selectComplete = false;
    }

    // Update is called once per frame
    void Update()
    {
        Select();
    }

    
    public void Open(List<string> elements)
    {
        _elements = elements;

        _selectComplete = false;
        _selectIndex = 0;


        for (int i = 0; i < _elements.Count; i++)
        {
            var obj = Instantiate(_selectionPrefab);
            var rt = obj.GetComponent<RectTransform>();

            rt.SetParent(transform);
            rt.localPosition = new Vector3(DEFAULT_RECT.x, DEFAULT_RECT.y + DEFAULT_RECT.height * i ,0.0f);
            obj.GetComponentInChildren<Text>().text = _elements[i];

            _selections.Add(obj);
        }

        UpdateSelectionColor();
    }

    public void Close()
    {
        _elements.Clear();

        for (int i = 0; i < _selections.Count; i++)
            Destroy(_selections[i]);
        _selections.Clear();
    }

    public void IndexSelect(int index)
    {
        _selectIndex = index;
        _selectComplete = true;
    }



    private void Select()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            _selectIndex++;
            if (_selectIndex >= _selections.Count)
                _selectIndex = 0;

            UpdateSelectionColor();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            _selectIndex--;
            if (_selectIndex < 0)
                _selectIndex = _selections.Count - 1;

            UpdateSelectionColor();
        }

        if(Input.GetKeyDown(KeyCode.Return))
        {
            _selectComplete = true;
            Close();
        }
    }
    private void UpdateSelectionColor()
    {
        for(int i = 0; i < _selections.Count;i++)
        {
            var image = _selections[i].GetComponent<Image>();
            if(i == _selectIndex)
            {
                image.color = SELECT_COLOR;
            }
            else
            {
                image.color = NOT_SELECT_COLOR;
            }
        }
    }
}
