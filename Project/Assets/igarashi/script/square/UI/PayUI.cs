using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PayUI : MonoBehaviour
{
    private bool _selectComplete = false;

    private GameObject _description;
    private GameObject _yes;
    private GameObject _no;

    private bool _selectYes;


    private void Awake()
    {
        _selectComplete = false;

        _description = transform.Find("Description").gameObject;
        _yes = transform.Find("Yes").gameObject;
        _no = transform.Find("No").gameObject;

        _selectYes = false;
        ButtonColorUpdate();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SelectChoices();
    }


    //=================================
    //public
    //=================================
    public void Yes()
    {
        _selectComplete = true;

        _selectYes = true;

        ButtonColorUpdate();
    }

    public void No()
    {
        _selectComplete = true;

        _selectYes = false;

        ButtonColorUpdate();
    }

    public void SetDescription(string description)
    {
        _description.GetComponent<Text>().text = description;
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
        if (_selectYes)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            {
                _selectYes = false;
                ButtonColorUpdate();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            {
                _selectYes = true;
                ButtonColorUpdate();
            }
        }

        if(Input.GetKeyDown(KeyCode.Return))
        {
            _selectComplete = true;
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
}
