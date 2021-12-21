using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SouvenirScrambleItem : MonoBehaviour
{
    private Vector3 _targetPos;
    public Vector3 TargetPos => _targetPos;

    private SouvenirScrambleManager _manager;


    public void Init(Vector3 targetPos,SouvenirScrambleManager manager)
    {
        _targetPos = targetPos;
        _manager = manager;
    }

    //===============

    void Start()
    {
        
    }

    void Update()
    {
        switch (_manager.State) {
            case SouvenirScrambleManager.SouvenirScrambleState.PLAY:
                transform.Rotate(new Vector3(1, 1, 0));
            break;
            case SouvenirScrambleManager.SouvenirScrambleState.RESULT:
                if (gameObject.activeSelf) gameObject.SetActive(false);
                break;
            default:
                break;
        };
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "ItemDestroyArea")
            Destroy(gameObject);
    }
}
