using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SquareBase : MonoBehaviour
{
    // 種類
    private SquareType _squareType;

    // イン
    protected List<SquareBase> _inConnects = new List<SquareBase>();

    public List<SquareBase> InConnects
    {
        get { return _inConnects; }
    }

    // アウト
    protected List<SquareBase> _outConnects = new List<SquareBase>();

    // マスに止まっているきキャラクター
    private LinkedList<CharacterBase> _stoppedCharacters = new LinkedList<CharacterBase>();

    //マス情報
    protected string _squareInfo;
    public string SquareInfo => _squareInfo;

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

    public bool AlreadyStopped()
    {
        return _stoppedCharacters.Count >= 1 ? true : false;
    }

    public int GetStoppedCharacterNum()
    {
        return _stoppedCharacters.Count;
    }

    public int GetAlignmentIndexByCharacter(CharacterBase character)
    {
        //Debug.Log(character);

        int index = 0;
        foreach(var x in _stoppedCharacters)
        {
            if (x == character) {
                //Debug.Log(index);
                return index; }
            index++;
        }
        Debug.Assert(false);
        return -1;
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

    public Vector3 GetPosition()
    {
        return GetComponent<Transform>().localPosition;
    }
    
    //評価を調べる
    public virtual int GetScore(CharacterBase character, CharacterType characterType)
    {
        int totalScore = 0;
        if (_stoppedCharacters.Count >= 1)
        {


            // 持ってないカードリスト
            var dontHaveTypes = new HashSet<SouvenirType>();
            for(int i = 0; i < (int)SouvenirType.MAX_TYPE; i++)
            {
                // カードが無い
                if(character.Souvenirs.Where(x => x.Type == (SouvenirType)i).Count() == 0)
                {
                    dontHaveTypes.Add((SouvenirType)i);
                }
            }

            // 持っていないカードを持っているか
            foreach (var x in _stoppedCharacters)
            {
                foreach(var y in x.Souvenirs)
                {
                    //　持っていないカードリストに含まれている
                    if(dontHaveTypes.Where(z => z == y.Type).Count() >= 1)
                    {
                        // 揃ったら勝利の場合
                        if(dontHaveTypes.Count == 1)
                        {
                            // 持っていないお土産マスに止まり勝利するスコアと同じ
                            return (int)SquareScore.DONT_HAVE_SOUVENIR_TO_WIN;
                        }
                        // お土産マスに止まるよりも評価が高い
                        totalScore += (int)SquareScore.DONT_HAVE_SOUVENIR;
                    }
                    else
                    {
                        totalScore += 2;
                    }   
                }
                
            }
        }

        return totalScore;
    }

    public Quaternion GetQuaternion()
    {
        //Vector3 direction = (_inConnects.First().transform.position - transform.position).normalized;
        Vector3 direction = (transform.position - _inConnects.First().transform.position).normalized;
        Vector3 xAxis = Vector3.Cross(new Vector3(0, 0, 1), direction).normalized;
        Vector3 zAxis = Vector3.Cross(xAxis, new Vector3(0, 0, 1)).normalized;
        return Quaternion.LookRotation(zAxis, new Vector3(0, 0, 1));
    }

    public Vector3 GetUpVector()
    {
        return transform.up;
    }
}
