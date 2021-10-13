using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareBase : MonoBehaviour
{
    // 種類
    private SquareType _squareType;

    // イン
    [SerializeField]
    protected List<SquareConnect> _inConnects = new List<SquareConnect>();

    public List<SquareConnect> InConnects
    {
        get { return _inConnects; }
    }

    // アウト
    [SerializeField]
    protected List<SquareConnect> _outConnects = new List<SquareConnect>();

    // マスに止まっているきキャラクター
    private LinkedList<CharacterBase> _stoppedCharacters = new LinkedList<CharacterBase>();

    public List<SquareConnect> OutConnects
    {
        get { return _outConnects; }
    }

    public virtual void Stop(CharacterBase character)
    {
        character.CompleteStopExec();
    }

    public void AddCharacter(CharacterBase character)
    {
        _stoppedCharacters.AddLast(character);
    }

    public void RemoveCharacter(CharacterBase character)
    {
        _stoppedCharacters.Remove(character);
    }

    public void AlignmentCharacters()
    {
        int count = 0;

        foreach(var x in _stoppedCharacters)
        {
            var pos = x.transform.position;
            count++;
            

        }

    }

    public Vector3 GetPosition()
    {
        return GetComponent<Transform>().localPosition;
    }
    
    //評価を調べる
    public virtual int GetScore(CharacterBase character)
    {
        // 何もないマスなので評価は0
        return 0;
    }
}
