using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthMove : MonoBehaviour
{
    ////テストコード
    private GameObject _targetSquare = null;
    [SerializeField]
    List<GameObject> _squares;
    [SerializeField]
    private GameObject _camera = null;
    //補完用
    private GameObject _preSquare = null;
    private bool _lerpStart = false;
    private Quaternion _lerpStartRot;
    private Quaternion _lerpEndRot;
    private float _lerpTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        _targetSquare = _preSquare = _squares[0];
        _lerpStartRot = transform.rotation;
        _lerpEndRot = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        //テスト用マス移動
        if (Input.GetKeyDown(KeyCode.Alpha1)) _targetSquare = _squares[0];
        if (Input.GetKeyDown(KeyCode.Alpha2)) _targetSquare = _squares[1];
        if (Input.GetKeyDown(KeyCode.Alpha3)) _targetSquare = _squares[2];


        if(_targetSquare != _preSquare) //ターゲット変更判定
        {
            Vector3 refVec = (_camera.transform.position - transform.position).normalized;
            Vector3 vec = (_targetSquare.transform.localPosition - transform.position).normalized;
            _lerpEndRot = Quaternion.FromToRotation(vec, refVec);
            _lerpTime = 0.0f;

            _lerpStartRot = transform.rotation;
            _preSquare = _targetSquare;
            _lerpStart = true;
        }

        if(_lerpStart)
        {
            transform.rotation = Quaternion.Lerp(_lerpStartRot,_lerpEndRot,_lerpTime);

            if (_lerpTime >= 1.0f)
                _lerpStart = false;

            _lerpTime += Time.deltaTime;
        }
    }
}
