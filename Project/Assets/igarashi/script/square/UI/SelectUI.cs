using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectUI : MonoBehaviour
{    
    private Rect DEFAULT_RECT = new Rect(244.0f,-75.0f,160,30);

    private List<string> _elements = new List<string>();
    public List<string> Elements => _elements;

    private List<GameObject> _selections = new List<GameObject>();
    private int _selectIndex;
    public int SelectIndex => _selectIndex;
    private bool _selectComplete;
    public bool IsComplete => _selectComplete;
    private bool _open;
    public bool IsOpen => _open;

    private CharacterBase _openerCharacter;   

    [Header("プレハブ")]
    [SerializeField]
    private GameObject _selectionPrefab = null;

    [Header("ボタンカラー")]
    [SerializeField]
    private Color SELECT_COLOR = new Color(1, 0, 0, 1);
    [SerializeField]
    private Color NOT_SELECT_COLOR = new Color(1, 1, 1, 1);


    /// <summary>
    /// UIを開く
    /// </summary>
    /// <param name="elements">選択する要素</param>
    /// <param name="character">開いたプレイヤー</param>
    public void Open(List<string> elements,CharacterBase character)
    {
        _elements = new List<string>(elements);

        _selectComplete = false;
        _selectIndex = _elements.Count - 1;

        int elementNumber = 0;
        for (int i = 0; i < _elements.Count ; i++)
        {
            var obj = Instantiate(_selectionPrefab);
            var rt = obj.GetComponent<RectTransform>();

            rt.SetParent(transform);
            rt.localPosition = new Vector3(DEFAULT_RECT.x, DEFAULT_RECT.y + DEFAULT_RECT.height * (_elements.Count - 1 - i) ,0.0f);
            obj.GetComponentInChildren<Text>().text = _elements[i];

            _selections.Add(obj);

            elementNumber++;
        }

        UpdateSelectionColor();

        if (character.IsAutomatic)
            IndexSelect(0);

        _openerCharacter = character;
        _open = true;
    }


    /// <summary>
    /// UIを閉じる
    /// </summary>
    public void Close()
    {
        _elements.Clear();

        for (int i = 0; i < _selections.Count; i++)
            Destroy(_selections[i]);
        _selections.Clear();

        _openerCharacter = null;
        _open = false;
    }


    /// <summary>
    /// AI選択用
    /// </summary>
    /// <param name="index">選択したいインデックス</param>
    public void IndexSelect(int index)
    {
        _selectIndex = index;
        _selectComplete = true;
        Close();
    }


    //=============================================

    void Start()
    {
        _selectIndex = 0;
        _selectComplete = false;
    }

    void Update()
    {
        if (_open)
            Select();
    }

 
    private void Select()
    {
        if (_openerCharacter.IsAutomatic)
            return;

        if(Input.GetKeyDown(KeyCode.W))
        {

            _selectIndex--;
            if (_selectIndex < 0)
                _selectIndex = _selections.Count - 1;

            UpdateSelectionColor();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            _selectIndex++;
            if (_selectIndex >= _selections.Count)
                _selectIndex = 0;

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
