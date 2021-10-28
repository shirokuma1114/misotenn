using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootAnimator : MonoBehaviour
{
    Animator mAnimator;
   
    [SerializeField] public bool BoolWait;
    [SerializeField] public bool BoolStay;
    [SerializeField] public bool BoolIn;
    [SerializeField] public bool BoolOut;
    [SerializeField] public bool BoolRotate;

    void Awake()
    {
        mAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        mAnimator.SetBool("wait", BoolWait);
        mAnimator.SetBool("stay", BoolStay);
        mAnimator.SetBool("in", BoolIn);
        mAnimator.SetBool("out", BoolOut);
        mAnimator.SetBool("rotate", BoolRotate);
    }
}
