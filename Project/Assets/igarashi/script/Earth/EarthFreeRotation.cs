using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthFreeRotation : MonoBehaviour
{
    private CharacterBase _operator;

    private bool _freeRotationMode = false;
    public bool IsFreeRotationMode => _freeRotationMode;

    private Quaternion _startRot;
    private float _xzAngle;


    [Header("操作キー")]
    [SerializeField]
    private KeyCode _upKey = KeyCode.W;
    [SerializeField]
    private KeyCode _downKey = KeyCode.S;
    [SerializeField]
    private KeyCode _rightKey = KeyCode.D;
    [SerializeField]
    private KeyCode _leftKey = KeyCode.A;

    [Header("回転速度")]
    [SerializeField]
    private float _freamRotationAngle = 85.0f;

    [Header("上下回転最大角度")]
    [SerializeField]
    private float _maxVerticalAngle = 80.0f;

    [Header("Debug")]
    [SerializeField]
    private KeyCode _modeChangeKey = KeyCode.E;
    [SerializeField]
    private CharacterBase _debugOperator = null;
    

    /// <summary>
    /// フリー回転モードにする関数
    /// すべてのキャラクターがWait状態になる
    /// </summary>
    /// <param name="character">
    /// そのターンのキャラクター
    /// </param>
    public void TrunOn(CharacterBase character)
    {
        _startRot = transform.rotation;

        Vector3 localFront = transform.InverseTransformPoint(0, 0, -1.0f);
        Vector3 xzLocalFront = new Vector3(localFront.x, 0.0f, localFront.z);
        _xzAngle = Vector3.SignedAngle(localFront, xzLocalFront, Vector3.Cross(xzLocalFront,transform.up));

        foreach (var c in FindObjectsOfType<CharacterBase>())
            c.SetWaitEnable(true);
        _operator = character;

        _freeRotationMode = true;
    }
    /// <summary>
    /// フリー回転モードを終える関数
    /// TrunOnでの引数のキャラクターだけのWait状態を解除
    /// </summary>
    public void TrunOff()
    {
        transform.rotation = _startRot;
        _operator.SetWaitEnable(false);

        _freeRotationMode = false;
    }


    //=================================================================

    private void Awake()
    {
        _freeRotationMode = false;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Operation();

        if(_freeRotationMode)
        {
            SquareInfoViewer();
        }
    }


    private void DebugTrunOn()
    {
        TrunOn(_debugOperator);
    }


    private void Operation()
    {
        //Debug
        if(Input.GetKeyDown(_modeChangeKey))
        {
            if (!_freeRotationMode)
                DebugTrunOn();
            else
                TrunOff();
        }

        if (_freeRotationMode)
        {
            if (Input.GetKey(_upKey))
            {
                if(_xzAngle > -_maxVerticalAngle)
                {
                    transform.RotateAround(transform.position,new Vector3(1,0,0), -_freamRotationAngle * Time.deltaTime);
                    _xzAngle += -_freamRotationAngle * Time.deltaTime;
                }
            }
            if (Input.GetKey(_downKey))
            {
                if (_xzAngle < _maxVerticalAngle)
                {
                    transform.RotateAround(transform.position, new Vector3(1, 0, 0), _freamRotationAngle * Time.deltaTime);
                    _xzAngle += _freamRotationAngle * Time.deltaTime;
                }
            }

            if (Input.GetKey(_rightKey))
            {
                transform.RotateAround(transform.position, transform.up, _freamRotationAngle * Time.deltaTime);
            }
            if (Input.GetKey(_leftKey))
            {
                transform.RotateAround(transform.position, transform.up, -_freamRotationAngle * Time.deltaTime);
            }
        }
    }

    private void SquareInfoViewer()
    {
        GameObject camera = Camera.main.gameObject;
        Ray cameraRay = new Ray(camera.transform.position, camera.transform.forward);
        RaycastHit hitInfo = new RaycastHit();

        if(Physics.Raycast(cameraRay,out hitInfo))
        {
            
        }
    }
}