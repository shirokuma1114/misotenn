using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllIvent : MonoBehaviour
{
    public Image Ex1out;
    public Image Ex2out;
    public Image Ex3out;
    public Image Ex4out;
    public Image Ex5out;
    public Image Ex6out;
    public Image Ex7out;
    public Image Ex8out;
    public Image Ex1in;
    public Image Ex2in;
    public Image Ex3in;
    public Image Ex4in;
    public Image Ex5in;
    public Image Ex6in;
    public Image Ex7in;
    public Image Ex8in;

    public Text Ex1text;
    public Text Ex2text;
    public Text Ex3text;
    public Text Ex4text;
    public Text Ex5text;
    public Text Ex6text;
    public Text Ex7text;
    public Text Ex8text;

    public bool mn;

    // Start is called before the first frame update
    void Start()
    {
        mn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!mn)
        {
            Ex1out.enabled = false;
            Ex2out.enabled = false;
            Ex3out.enabled = false;
            Ex4out.enabled = false;
            Ex5out.enabled = false;
            Ex6out.enabled = false;
            Ex7out.enabled = false;
            Ex8out.enabled = false;
            Ex1in.enabled = false;
            Ex2in.enabled = false;
            Ex3in.enabled = false;
            Ex4in.enabled = false;
            Ex5in.enabled = false;
            Ex6in.enabled = false;
            Ex7in.enabled = false;
            Ex8in.enabled = false;
            Ex1text.enabled = false;
            Ex2text.enabled = false;
            Ex3text.enabled = false;
            Ex4text.enabled = false;
            Ex5text.enabled = false;
            Ex6text.enabled = false;
            Ex7text.enabled = false;
            Ex8text.enabled = false;
        }
        else
        {
            Ex1out.enabled = true;
            Ex2out.enabled = true;
            Ex3out.enabled = true;
            Ex4out.enabled = true;
            Ex5out.enabled = true;
            Ex6out.enabled = true;
            Ex7out.enabled = true;
            Ex8out.enabled = true;
            Ex1in.enabled = true;
            Ex2in.enabled = true;
            Ex3in.enabled = true;
            Ex4in.enabled = true;
            Ex5in.enabled = true;
            Ex6in.enabled = true;
            Ex7in.enabled = true;
            Ex8in.enabled = true;
            Ex1text.enabled = true;
            Ex2text.enabled = true;
            Ex3text.enabled = true;
            Ex4text.enabled = true;
            Ex5text.enabled = true;
            Ex6text.enabled = true;
            Ex7text.enabled = true;
            Ex8text.enabled = true;
        }
    }

    public void OnClick()
    {
        mn = true;
    }
}
