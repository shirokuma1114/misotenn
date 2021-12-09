using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PayUI : MonoBehaviour
{
    private bool _selectComplete = false;
    public bool IsSelectComplete => _selectComplete;

    private bool _selectYes;
    public bool IsSelectYes => _selectYes;

    private GameObject _yes;
    private GameObject _no;

    private bool _open;
    public bool IsOpen => _open;
    private CharacterBase _openerCharacter;


    [Header("操作キー")]
    [SerializeField]
    private KeyCode _moveUp = KeyCode.W;
    [SerializeField]
    private KeyCode _moveDown = KeyCode.S;
    [SerializeField]
    private KeyCode _enter = KeyCode.Return;

    private float beforeTrigger;

    /// <summary>
    /// AI選択用
    /// </summary>
    public void AISelectYes()
    {
        _selectComplete = true;
        _selectYes = true;

        ButtonColorUpdate();

        Invoke("Close", 0.5f);
    }

    /// <summary>
    /// AI選択用
    /// </summary>
    public void AISelectNo()
    {
        _selectComplete = true;
        _selectYes = false;

        ButtonColorUpdate();

        Close();
    }


    /// <summary>
    /// UIを開く
    /// </summary>
    /// <param name="opener">UIを開いたプレイヤー</param>
    public void Open(CharacterBase opener)
    {
        _openerCharacter = opener;

        _yes.GetComponent<Image>().enabled = true;
        _yes.GetComponentInChildren<Text>().enabled = true;
        _no.GetComponent<Image>().enabled = true;
        _no.GetComponentInChildren<Text>().enabled = true;

        _selectComplete = false;
        _selectYes = false;

        ButtonColorUpdate();

        _open = true;

        //if (_openerCharacter.IsAutomatic)
          //  AISelectYes();
    }

    //===========================================

    private void Awake()
    {
        _selectComplete = false;

        _yes = transform.Find("Yes").gameObject;
        _no = transform.Find("No").gameObject;

        _selectYes = false;
        ButtonColorUpdate();

        Close();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (_open)
        {
            SelectChoices();
        }
    }


    private void SelectChoices()
    {
        if (_openerCharacter.IsAutomatic)
            return;

        float viewButton = _openerCharacter.Input.GetAxis("Vertical");

        if (beforeTrigger == 0.0f)
        {
            if (viewButton != 0)
            {
                _selectYes = !_selectYes;
                ButtonColorUpdate();

                Control_SE.Get_Instance().Play_SE("UI_Select");
            }
        }

        beforeTrigger = viewButton;

        if (_selectYes)
        {
            if (Input.GetKeyDown(_moveUp) || Input.GetKeyDown(_moveDown))
            {
                _selectYes = false;
                ButtonColorUpdate();

                Control_SE.Get_Instance().Play_SE("UI_Select");
            }
        }
        else
        {
            if (Input.GetKeyDown(_moveUp) || Input.GetKeyDown(_moveDown))
            {
                _selectYes = true;
                ButtonColorUpdate();

                Control_SE.Get_Instance().Play_SE("UI_Select");
            }
        }

        if(Input.GetKeyDown(_enter) || _openerCharacter.Input.GetButtonDown("A"))
        {
            _selectComplete = true;
            Close();

            Control_SE.Get_Instance().Play_SE("UI_Correct");
        }
    }


    private void ButtonColorUpdate()
    {
        if (_selectYes)
        {
            _yes.GetComponent<Image>().color = new Color(1, 0, 0, 1);
            _no.GetComponent<Image>().color = new Color(0, 0, 0, 0.137254f);
        }
        else
        {
            _no.GetComponent<Image>().color = new Color(1, 0, 0, 1);
            _yes.GetComponent<Image>().color = new Color(0, 0, 0, 0.137254f);
        }
    }

    private void Close()
    {
        _open = false;

        _openerCharacter = null;

        _yes.GetComponent<Image>().enabled = false;
        _yes.GetComponentInChildren<Text>().enabled = false;
        _no.GetComponent<Image>().enabled = false;
        _no.GetComponentInChildren<Text>().enabled = false;
    }
}