using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareBase : MonoBehaviour
{
    // ���
    private SquareType _squareType;

    // �C��
    [SerializeField]
    protected List<SquareBase> _inConnects = new List<SquareBase>();

    public List<SquareBase> InConnects
    {
        get { return _inConnects; }
    }

    // �A�E�g
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
