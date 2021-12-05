using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SelectSelectWindow : MonoBehaviour
{
    [SerializeField]
    List<RectTransform> _selectRts;

    int _selectedIndex;

    Vector2[] _initPositions;

    [SerializeField]
    List<Vector2> _movePositions;

    [SerializeField]
    Ease _inEase;

    [SerializeField]
    Ease _outEase;

    [SerializeField]
    SelectPlanet _selectPlanet;

    // Start is called before the first frame update
    void Start()
    {
        // -280 -120 

        _initPositions = new Vector2[_selectRts.Count];

        for(int i = 0; i < _selectRts.Count; i++)
        {
            _initPositions[i] = _selectRts[i].anchoredPosition;
        }

        _selectRts[_selectedIndex].DOAnchorPos(_initPositions[_selectedIndex] + _movePositions[_selectedIndex], 0.5f).SetEase(Ease.InBounce);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            var newIndex = _selectedIndex - 1;
            if (newIndex < 0) newIndex = _selectRts.Count - 1;
            ChangeSelect(newIndex);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            var newIndex = _selectedIndex + 1;
            if (newIndex == _selectRts.Count) newIndex = 0;
            ChangeSelect(newIndex);
        }
    }

    void InitSelect()
    {
        _selectRts[_selectedIndex].DOAnchorPos(_initPositions[_selectedIndex] + _movePositions[_selectedIndex], 0.5f).SetEase(Ease.InBounce);
    }

    void ChangeSelect(int newIndex)
    {
        // Œ³‚ÌˆÊ’u‚É–ß‚·
        _selectRts[_selectedIndex].DOAnchorPos(_initPositions[_selectedIndex], 0.2f).SetEase(_outEase);
        

        _selectRts[newIndex].DOAnchorPos(_initPositions[newIndex] + _movePositions[newIndex], 0.2f).SetEase(_inEase).SetDelay(0.1f);

        _selectedIndex = newIndex;

        _selectPlanet.SetIndex(_selectedIndex);
    }
}
