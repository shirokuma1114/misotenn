using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartCounterGura : MonoBehaviour
{
    public bool CountFlg;
    private Text _num;

    // Start is called before the first frame update
    void Awake()
    {
        CountFlg = false;
        _num = GetComponent<Text>();

        StartCoroutine(NumCount());
    }
    

    public IEnumerator NumCount()
    {
        yield return new WaitForSeconds(1f);

        int num = 3;
        while(num>0)
        {
            num--;
            _num.text = num.ToString();
            yield return new WaitForSeconds(1f);

        }

        CountFlg = true;
        //_num.text = "Start";
    }

}
