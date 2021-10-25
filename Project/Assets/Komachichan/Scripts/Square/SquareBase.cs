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

    // マスに止まっているきキャラクター
    private LinkedList<CharacterBase> _stoppedCharacters = new LinkedList<CharacterBase>();

    public LinkedList<CharacterBase> StoppedCharacters
    {
        get { return _stoppedCharacters; }
    }

    public List<SquareBase> OutConnects
    {
        get { return _outConnects; }
    }

    public void SetInOut()
    {
        // ヒエラルキーの上と下をInOutに入れる

        var index = transform.GetSiblingIndex();

        if (index == 1)
        {
            _inConnects.Add(transform.parent.GetChild(23).GetComponent<SquareBase>());
            _outConnects.Add(transform.parent.GetChild(2).GetComponent<SquareBase>());
            return;
        }
        if (index == 23)
        {
            _inConnects.Add(transform.parent.GetChild(22).GetComponent<SquareBase>());
            _outConnects.Add(transform.parent.GetChild(1).GetComponent<SquareBase>());
            return;
        }

        _inConnects.Add(transform.parent.GetChild(index - 1).GetComponent<SquareBase>());
        _outConnects.Add(transform.parent.GetChild(index + 1).GetComponent<SquareBase>());
    }

    public void JudgeCollision(CharacterBase character)
    {
        
        
    }

    public bool AlreadyStopped()
    {
        return _stoppedCharacters.Count >= 1 ? true : false;
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
        // マスに乗っている人の数
        return _stoppedCharacters.Count;
    }
}
