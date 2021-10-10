using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareBase : MonoBehaviour
{
    // ���
    private SquareType _squareType;

    // �C��
    [SerializeField]
    protected List<SquareConnect> _inConnects = new List<SquareConnect>();

    public List<SquareConnect> InConnects
    {
        get { return _inConnects; }
    }

    // �A�E�g
    [SerializeField]
    protected List<SquareConnect> _outConnects = new List<SquareConnect>();

    public List<SquareConnect> OutConnects
    {
        get { return _outConnects; }
    }

    public virtual void Stop(CharacterBase character)
    {
        character.CompleteStopExec();
    }

    public Vector3 GetPosition()
    {
        return GetComponent<Transform>().localPosition;
    }
    
    //�]���𒲂ׂ�
    public virtual int GetScore(CharacterBase character)
    {
        // �����Ȃ��}�X�Ȃ̂ŕ]����0
        return 0;
    }
}
