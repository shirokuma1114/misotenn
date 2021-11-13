using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedStar : MonoBehaviour
{
    [SerializeField]private Transform _target;

    // Start is called before the first frame update
    void Start()
    {
        _target = this.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(Vector3.zero, _target.right, -45 * Time.deltaTime);
    }
}
