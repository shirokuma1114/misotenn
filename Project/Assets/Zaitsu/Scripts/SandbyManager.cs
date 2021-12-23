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
    [SerializeField]
    private Image[] _characterIcon;
    [SerializeField]
    private KeyCode[] _correctKey;

    private MiniGameConnection _miniGameConnection;

    // Start is called before the first frame update
    void Start()
    {
        _miniGameConnection = MiniGameConnection.Instance;

        // ステートをスタンバイように変更する

        var characters = _miniGameConnection.Characters;
        for (int i = 0; i < characters.Count; i++)
        {
            _characterIcon[i].sprite = characters[i].Icon;

            if(characters[i].IsAutomatic)
            {
                _charaImage[i].color = new Color(255.0f, 0f, 0f);
                _isUse[i] = true;
                _isOK[i].SetActive(true);
            }
            else
            {
                _charaImage[i].color = new Color(0f, 0f, 0f);
                _isUse[i] = false;
                _isOK[i].SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // インプットでカラーとフラグを立てる // カラーとフラグをおろす
        var characters = _miniGameConnection.Characters;
        for(int i = 0; i < characters.Count;i++)
        {
            var character = characters[i];

            if (character.IsAutomatic) continue;

            if (!_isUse[i])
            {
                if (character.Input.GetButtonDown("A") || Input.GetKeyDown(_correctKey[i]))
                {
                    _charaImage[i].color = new Color(255.0f, 0f, 0f);
                    _isUse[i] = true;
                    _isOK[i].SetActive(true);

                    if(Control_SE.Get_Instance()) Control_SE.Get_Instance().Play_SE("UI_Correct");
                }
            }
            else
            {
                if (character.Input.GetButtonDown("B") || Input.GetKeyDown(_correctKey[i]))
                {
                    _charaImage[i].color = new Color(0f, 0f, 0f);
                    _isUse[i] = false;
                    _isOK[i].SetActive(false);

                    if (Control_SE.Get_Instance()) Control_SE.Get_Instance().Play_SE("UI_Close");
                }
            }
        }
        

        // フラグがすべて立った時
        if (_isUse[0] == true&&_isUse[1] == true&&_isUse[2] == true&&_isUse[3] == true)
        {
            StartCoroutine(_fade.FadeStart());
        }
    }

    
}
