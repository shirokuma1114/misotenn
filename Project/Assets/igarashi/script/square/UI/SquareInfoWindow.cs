using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquareInfoWindow : MonoBehaviour
{
    private Text _text;
    private Image _frame;
    private Image _target;

    private bool _enable;


    public void SetEnable(bool enable)
    {
        _text.enabled = enable;
        _frame.enabled = enable;
        _target.enabled = enable;
        _enable = enable;
    }

    public void SetSquareInfo(string info)
    {
        _text.text = info;
    }


    //====================================================
    void Start()
    {
        _text = transform.Find("Message").GetComponent<Text>();
        _frame = transform.Find("Frame").GetComponent<Image>();
        _target = transform.Find("Target").GetComponent<Image>();
        _enable = false;
    }

    void Update()
    {
        if(_enable)
        {
            GameObject camera = Camera.main.gameObject;
            Ray cameraRay = new Ray(camera.transform.position, camera.transform.forward);
            RaycastHit hitInfo = new RaycastHit();

            if (Physics.Raycast(cameraRay, out hitInfo))
            {
                SquareBase square = hitInfo.collider.GetComponent<SquareBase>();
                SetSquareInfo("");
                if (square)
                {
                    SetSquareInfo(square.SquareInfo);
                }
            }
        }        
    }
}
