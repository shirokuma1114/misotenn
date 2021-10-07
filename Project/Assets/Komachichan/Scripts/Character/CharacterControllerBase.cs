using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerBase : MonoBehaviour
{
    [SerializeField]
    protected CharacterBase _character;

    public CharacterBase Character
    {
        get { return _character; }
    }

    //�ړ��J�[�h��I�ю��̃}�X�Ɏ~�܂�܂�
    public virtual void Move()
    {

    }

    public virtual void SetRoot()
    {

    }
}
