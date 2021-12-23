using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MovingCardWindow : WindowBase
{
    [SerializeField]
    GameObject _prefab;

    List<GameObject> _cards = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void SetEnable(bool enable)
    {
        foreach(var x in _cards)
        {
            x.GetComponent<Image>().enabled = enable;
            x.GetComponentsInChildren<Image>().Last().enabled = enable;
            x.GetComponentInChildren<Text>().enabled = enable;
        }

        if(enable)
        {
            return;
        }

    }

    public void GenerateMovingCards(List<int> movingCards)
    {
        DestroyCards();
        for(int i = 0; i < movingCards.Count; i++)
        {
            var obj = Instantiate(_prefab, transform);
            obj.GetComponentInChildren<Text>().text = movingCards[i].ToString();
            obj.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-295 + i * 31.0f, -70f);
            obj.GetComponent<RectTransform>().localScale = new Vector3(0.3f, 0.3f, 0.3f);
            obj.GetComponent<RectTransform>().Rotate(0,0,-90.0f);
            _cards.Add(obj);
        }
    }

    private void DestroyCards()
    {
        foreach (var x in _cards) Destroy(x.gameObject);
        _cards.Clear();
    }

}
