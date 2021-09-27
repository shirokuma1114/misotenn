using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareBase : MonoBehaviour
{
    // 種類
    private SquareType _squareType;

    // イン
    [SerializeField]
    protected List<SquareBase> _inConnects = new List<SquareBase>();

    public List<SquareBase> InConnects
    {
        get { return _inConnects; }
    }

    // アウト
    [SerializeField]
    protected List<SquareBase> _outConnects = new List<SquareBase>();

    public List<SquareBase> OutConnects
    {
        get { return _outConnects; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Stop()
    {

    }

    public void AddInConnect(SquareBase squareBase)
    {
        _inConnects.Add(squareBase);
    }

    public void AddOutConnect(SquareBase squareBase)
    {
        _outConnects.Add(squareBase);
    }

    public Vector3 GetPosition()
    {
        return GetComponent<Transform>().position;
    }
    
}
