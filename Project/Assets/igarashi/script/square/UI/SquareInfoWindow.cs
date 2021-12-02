using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquareInfoWindow : WindowBase
{
    private Text _text;
    private Image _frame;
    private Image _target;

    private bool _enable;

    private float _rayDistance;
    
    MyGameManager _myGameManager;

    private EarthFreeRotation _earth;

    private CharacterBase _operator;

    private SquareBase _beforeSquare;

    [SerializeField]
    WindowBase _backToWindow;

    public override void SetEnable(bool enable)
    {
        _text.enabled = enable;
        _frame.enabled = enable;
        _target.enabled = enable;
        _enable = enable;

        _myGameManager.EnableFreeRotation(enable);


        if (enable)
        {
            _operator = _earth.Operator;
        }
    }

    public void SetSquareInfo(string info)
    {
        _text.text = info;
    }


    //====================================================
    void Start()
    {
        _myGameManager = FindObjectOfType<MyGameManager>();
        _text = transform.Find("Message").GetComponent<Text>();
        _frame = transform.Find("Frame").GetComponent<Image>();
        _target = transform.Find("Target").GetComponent<Image>();
        //_enable = false;

        _text.enabled = false;
        _frame.enabled = false;
        _target.enabled = false;
        _enable = false;

        _earth = FindObjectOfType<EarthFreeRotation>();
        _rayDistance = Vector3.Distance(Camera.main.transform.position, _earth.gameObject.transform.position);
    }

    void Update()
    {
        if(_enable)
        {
            UpdateInfoText();

            // フリーカメラモードOFF
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SetEnable(false);
                _backToWindow.SetEnable(true);
            }
        }        
    }

    private void UpdateInfoText()
    {
        GameObject camera = Camera.main.gameObject;
        Ray cameraRay = new Ray(camera.transform.position, camera.transform.forward);
        RaycastHit hitInfo = new RaycastHit();

        if (Physics.Raycast(cameraRay, out hitInfo, _rayDistance))
        {
            SquareBase square = hitInfo.collider.GetComponent<SquareBase>();

            if (square != _beforeSquare)
            {
                SetSquareInfo(square.GetSquareInfo(_operator));

                _beforeSquare = square;
            }
        }
        else
        {
            SetSquareInfo("");
            _beforeSquare = null;
        }
    }
}
