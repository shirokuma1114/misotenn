using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveArrowUI : MonoBehaviour
{
    private RectTransform _rectTransform;


    private Vector2 _direction;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var pos = _rectTransform.anchoredPosition;

        pos += _direction * Mathf.Sin(Time.time * 5.0f) * 60.0f * Time.deltaTime;
        _rectTransform.anchoredPosition = pos;

    }

    public void SetDirection(Vector2 direction)
    {
        _rectTransform = GetComponent<RectTransform>();

        _direction = direction;

        _rectTransform.rotation = Quaternion.FromToRotation(new Vector2(1, 0), _direction);

        var pos = _rectTransform.anchoredPosition + _direction * 80.0f;

        _rectTransform.anchoredPosition = pos;

    }
}
