using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberRender : MonoBehaviour
{
    [SerializeField]
    List<Sprite> _sprites;

    [SerializeField]
    private Image _image;

    public void SetNumber(int num)
    {
        num %= 10;
        _image.sprite = _sprites[num];
    }
}
