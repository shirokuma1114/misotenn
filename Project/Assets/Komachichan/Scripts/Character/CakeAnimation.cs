using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CakeAnimation : MonoBehaviour
{
    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }
    
    public void StartMove()
    {
        _animator.SetBool("Idle", false);
        if (Control_SE.Get_Instance()) Control_SE.Get_Instance().Play_SE("Fly");
    }

    public void EndMove()
    {
        _animator.SetBool("Idle", true);
        if (Control_SE.Get_Instance()) Control_SE.Get_Instance().Stop_SE();
        if (Control_SE.Get_Instance()) Control_SE.Get_Instance().Play_SE("UI_Correct");
        
    }

    public bool CanMove()
    {
        return _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Fly";
    }

    void Update()
    {
        //Debug.Log(_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
    }
}
