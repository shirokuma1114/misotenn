using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PayUI : MonoBehaviour
{
    private bool _selectComplete = false;
    private bool _selectYes;

    private GameObject _yes;
    private GameObject _no;

    private bool _open;
    public bool IsOpen => _open;
    private CharacterBase _openerCharacter;

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
        if(_open)
        {
            SelectChoices();
        }
    }


    //=================================
    //public
    //=================================
    public void AISelectYes()
    {
        _selectComplete = true;
        _selectYes = true;

        ButtonColorUpdate();

        Close();
    }

    public void AISelectNo()
    {
        _selectComplete = true;
        _selectYes = false;

        ButtonColorUpdate();

        Close();
    }


    public void SetEnable(bool enable)
    {
        _open = enable;

        _yes.GetComponent<Image>().enabled = enable;
        _yes.GetComponentInChildren<Text>().enabled = enable;

        _no.GetComponent<Image>().enabled = enable;
        _no.GetComponentInChildren<Text>().enabled = enable;
    }

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
    }

    public bool IsChoiseComplete()
    {
        return _selectComplete;
    }

    public bool IsSelectYes()
    {
        return _selectYes;
    }
    //=================================


    private void SelectChoices()
    {
        if (_openerCharacter.IsAutomatic)
            return;

        if (_selectYes)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
            {
                _selectYes = false;
                ButtonColorUpdate();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
            {
                _selectYes = true;
                ButtonColorUpdate();
            }
        }

        if(Input.GetKeyDown(KeyCode.Return))
        {
            _selectComplete = true;
            Close();
        }
    }


    private void ButtonColorUpdate()
    {
        if (_selectYes)
        {
            _yes.GetComponent<Image>().color = new Color(1, 0, 0, 1);
            _no.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
        else
        {
            _no.GetComponent<Image>().color = new Color(1, 0, 0, 1);
            _yes.GetComponent<Image>().color = new Color(1, 1, 1, 1);
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