using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartCounterGura : MonoBehaviour
{
    public bool CountFlg;
    private Text _num;

    [SerializeField]
    Image _frame;

    // Start is called before the first frame update
    void Awake()
    {
        CountFlg = false;
        _num = GetComponent<Text>();

        StartCoroutine(NumCount(Destroy));
    }
    

    public IEnumerator NumCount(UnityEngine.Events.UnityAction callback)
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
        callback();
        //_num.text = "Start";
    }

    private void Destroy()
    {
        
        _frame.enabled = false;
        _num.enabled = false;
    }
}
