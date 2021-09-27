using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _earth;

    [SerializeField]
    private GameObject _targetPos;

    // Start is called before the first frame update
    void Start()
    {
        _targetPos = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(_targetPos)
        {
            _earth.GetComponent<EarthMove>().MoveToPosition(_targetPos.transform.position);

            _targetPos = null;
        }
    }
}
