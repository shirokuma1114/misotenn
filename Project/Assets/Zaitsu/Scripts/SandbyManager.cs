using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SandbyManager : MonoBehaviour
{
    [SerializeField]
    private Image[] _charaImage;
    private bool[] _isUse=new bool[4];
    [SerializeField]
    private GameObject[] _isOK;
    [SerializeField]
    private StandbyFade _fade;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            _isUse[i] = false;
            _isOK[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // インプットでカラーとフラグを立てる // カラーとフラグをおろす
        if (!_isUse[0])
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _charaImage[0].color = new Color(255.0f, 0f, 0f);
                _isUse[0] = true;
                _isOK[0].SetActive(true);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _charaImage[0].color = new Color(0f, 0f, 0f);
                _isUse[0] = false;
                _isOK[0].SetActive(false);
            }
        }
        if (!_isUse[1])
        {
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _charaImage[1].color = new Color(255.0f, 0f, 0f);
                _isUse[1] = true;
                _isOK[1].SetActive(true);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _charaImage[1].color = new Color(0f, 0f, 0f);
                _isUse[1] = false;
                _isOK[1].SetActive(false);
            }
        }

        if (!_isUse[2])
        {
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                _charaImage[2].color = new Color(255.0f, 0f, 0f);
                _isUse[2] = true;
                _isOK[2].SetActive(true);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                _charaImage[2].color = new Color(0f, 0f, 0f);
                _isUse[2] = false;
                _isOK[2].SetActive(false);
            }
        }

        if (!_isUse[3])
        {
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                _charaImage[3].color = new Color(255.0f, 0f, 0f);
                _isUse[3] = true;
                _isOK[3].SetActive(true);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                _charaImage[3].color = new Color(0f, 0f, 0f);
                _isUse[3] = false;
                _isOK[3].SetActive(false);
            }
        }
       
        // フラグがすべて立った時
        if (_isUse[0] == true&&_isUse[1] == true&&_isUse[2] == true&&_isUse[3] == true)
        {
            StartCoroutine(_fade.FadeStart());
        }
    }

    
}
