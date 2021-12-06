using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthFreeRotation : MonoBehaviour
{
    private CharacterBase _operator;
    public CharacterBase Operator => _operator;

    private bool _freeRotationMode = false;
    public bool IsFreeRotationMode => _freeRotationMode;


    private Quaternion _startRot;
    private float _xzAngle;

    //private SquareInfoWindow _infoWindow;

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

        character.SetWaitEnable(true);
        _operator = character;

        //_infoWindow.SetEnable(true);
        //FindObjectOfType<SelectWindow>().SetEnable(false);

        _freeRotationMode = true;
    }
    /// <summary>
    /// フリー回転モードを終える関数
    /// TrunOnでの引数のキャラクターだけのWait状態を解除
    /// </summary>
    public void TrunOff()
    {
        if (_operator == null) return;
        transform.rotation = _startRot;
        _operator.SetWaitEnable(false);

        //_infoWindow.SetEnable(false);
        //FindObjectOfType<SelectWindow>().SetEnable(true);

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
        //_infoWindow = FindObjectOfType<SquareInfoWindow>();
        //_infoWindow.SetEnable(false);
    }

    // Update is called once per frame
    void Update()
    {
        Operation();
    }


    private void DebugTrunOn()
    {
    }


    private void Operation()
    {
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
}