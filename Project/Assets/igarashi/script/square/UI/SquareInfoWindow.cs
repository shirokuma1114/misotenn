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

    [SerializeField]
    WindowBase _backToWindow;

    public override void SetEnable(bool enable)
    {
        _text.enabled = enable;
        _frame.enabled = enable;
        _target.enabled = enable;
        _enable = enable;

        var earth = FindObjectOfType<EarthFreeRotation>().gameObject;
        _rayDistance = Vector3.Distance(Camera.main.transform.position, earth.transform.position);

        _myGameManager.EnableFreeRotation(enable);
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
    }

    void Update()
    {
        if(_enable)
        {
            GameObject camera = Camera.main.gameObject;
            Ray cameraRay = new Ray(camera.transform.position, camera.transform.forward);
            RaycastHit hitInfo = new RaycastHit();

            if (Physics.Raycast(cameraRay, out hitInfo,_rayDistance))
            {
                SquareBase square = hitInfo.collider.GetComponent<SquareBase>();
                SetSquareInfo("");
                if (square)
                {
                    SetSquareInfo(square.SquareInfo);
                }
            }

            // フリーカメラモードOFF
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SetEnable(false);
                _backToWindow.SetEnable(true);
            }
        }        
    }
}
