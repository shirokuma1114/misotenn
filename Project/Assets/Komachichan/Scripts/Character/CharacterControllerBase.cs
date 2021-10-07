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

    //移動カードを選び次のマスに止まるまで
    public virtual void Move()
    {

    }

    public virtual void SetRoot()
    {

    }
}
