using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialWindow : WindowBase
{
    private int _page;

    private Tween _tweem;

    [SerializeField]
    private RectTransform _moveTransform;

    [SerializeField]
    private float _speed = 0.5f;

    [SerializeField]
    private Ease _ease = Ease.Linear;

    [SerializeField]
    private WindowBase _backToWindow;


    void Start()
    {
        _page = 0;

        SetEnable(false);
    }

    void Update()
    {
        if(_tweem != null)
            if (_tweem.IsPlaying())
                return;

        if (Input.GetButtonDown("A") || Input.GetKeyDown(KeyCode.Return))
        {
            if(_page >= 4)
            {
                BackToWindow();
                Control_SE.Get_Instance().Play_SE("UI_Close");
            }
            else
            {
                _tweem = _moveTransform.DOLocalMoveX(_moveTransform.localPosition.x - 800.0f, _speed)
                    .SetEase(_ease);
                _tweem.SetAutoKill(false);

                _page++;

                Control_SE.Get_Instance().Play_SE("UI_Select");
            }            
        }

        if(Input.GetButtonDown("B") || Input.GetKeyDown(KeyCode.Escape))
        {
            if (_page <= 0)
            {
                
            }
            else
            {
                _tweem = _moveTransform.DOLocalMoveX(_moveTransform.localPosition.x + 800.0f, _speed)
                    .SetEase(_ease);
                _tweem.SetAutoKill(false);

                _page--;

                Control_SE.Get_Instance().Play_SE("UI_Select");
            }            
        }
    }

    private void BackToWindow()
    {
        SetEnable(false);
        _backToWindow.SetEnable(true);
    }

    public override void SetEnable(bool enable)
    {
        gameObject.SetActive(enable);

        if(enable)
        {
            _moveTransform.localPosition = new Vector3(0, 0, 0);
            _page = 0;
        }
    }
}
