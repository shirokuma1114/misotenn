using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SquareBase : MonoBehaviour
{
    // ���
    private SquareType _squareType;

    // �C��
    protected List<SquareBase> _inConnects = new List<SquareBase>();

    public List<SquareBase> InConnects
    {
        get { return _inConnects; }
    }

    // �A�E�g
    protected List<SquareBase> _outConnects = new List<SquareBase>();

    // �}�X�Ɏ~�܂��Ă��邫�L�����N�^�[
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
        // �q�G�����L�[�̏�Ɖ���InOut�ɓ����

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
    
    //�]���𒲂ׂ�
    public virtual int GetScore(CharacterBase character)
    {
        int addScore = 0;
        if (_stoppedCharacters.Count >= 1)
        {

            // �����ĂȂ��J�[�h���X�g
            var dontHaveTypes = new HashSet<SouvenirType>();
            for(int i = 0; i < (int)SouvenirType.MAX_TYPE; i++)
            {
                // �J�[�h������
                if(character.Souvenirs.Where(x => x.Type == (SouvenirType)i).Count() >= 1)
                {
                    dontHaveTypes.Add((SouvenirType)i);
                }
            }

            // �����Ă��Ȃ��J�[�h�������Ă��邩
            foreach (var x in _stoppedCharacters)
            {
                foreach(var y in x.Souvenirs)
                {
                    //�@�����Ă��Ȃ��J�[�h���X�g�Ɋ܂܂�Ă���
                    if(dontHaveTypes.Where(z => z == y.Type).Count() >= 1)
                    {
                        // �������珟���̏ꍇ
                        if(dontHaveTypes.Count == 1)
                        {
                            // �����Ă��Ȃ����y�Y�}�X�Ɏ~�܂菟������X�R�A�Ɠ���
                            return (int)SquareScore.DONT_HAVE_SOUVENIR_TO_WIN;
                        }
                        // ���y�Y�}�X�Ɏ~�܂�����]��������
                        addScore += (int)SquareScore.DONT_HAVE_SOUVENIR;
                    }   
                }
                
            }
        }

        // �}�X�ɏ���Ă���l�̐�
        return _stoppedCharacters.Count + addScore;
    }
}
