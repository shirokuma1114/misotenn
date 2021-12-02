using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectManager : MonoBehaviour
{
    [SerializeField]
    Animator _fadeAnimation;
    // Start is called before the first frame update
    void Start()
    {
        _fadeAnimation.Play("FadeIn");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
