using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthMove : MonoBehaviour
{
    //テストコード
    private GameObject _targetSquare = null;
    [SerializeField]
    List<GameObject> _squares;
    [SerializeField]
    private GameObject _camera = null;

    // Start is called before the first frame update
    void Start()
    {
        _targetSquare = _squares[0];
    }

    // Update is called once per frame
    void Update()
    {
        //テスト用マス移動
        if (Input.GetKeyDown(KeyCode.Alpha1)) _targetSquare = _squares[0];
        if (Input.GetKeyDown(KeyCode.Alpha2)) _targetSquare = _squares[1];
        if (Input.GetKeyDown(KeyCode.Alpha3)) _targetSquare = _squares[2];



        Vector3 refVec = (_camera.transform.position - transform.position).normalized;
        Vector3 vec = (_targetSquare.transform.localPosition - transform.position).normalized;

        transform.rotation = Quaternion.FromToRotation(vec, refVec);
    }
}
