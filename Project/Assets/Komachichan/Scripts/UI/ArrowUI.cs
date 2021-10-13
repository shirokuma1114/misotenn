using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _prefab;

    public List<GameObject> _arrows = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Create(SquareBase[] directions)
    {
        if (directions[2] == null) return;

        for(int i = 0; i < 4; i++)
        {
            if (directions[i])
            {
                var obj = Instantiate(_prefab, transform);
                Vector2 pos2D = Camera.main.ViewportToWorldPoint(Camera.main.WorldToViewportPoint(directions[i].transform.position));
                obj.GetComponent<MoveArrowUI>().SetDirection(pos2D.normalized);

                _arrows.Add(obj);
            }
        }
    }

    public void Delete()
    {
        foreach(var x in _arrows)
        {
            Destroy(x);
        }
        _arrows.Clear();
    }
}
