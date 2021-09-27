using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerBase : MonoBehaviour
{
    [SerializeField]
    protected CharacterBase _character;


    //移動カードを選び次のマスに止まるまで
    public virtual void Move()
    {

    }

   
}
