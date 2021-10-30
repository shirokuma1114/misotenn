using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthFreeRotation : MonoBehaviour
{
    private List<CharacterBase> _characters = new List<CharacterBase>();

    private bool _freeRotationMode;
    public bool IsFreeRotationMode => _freeRotationMode;

    private Quaternion _startRot;
    private Quaternion _endRot;
    private float _xzAngle;
    private float _yAngle;

    [Header("ëÄçÏÉLÅ[")]
    [SerializeField]
    private KeyCode _upKey = KeyCode.W;
    [SerializeField]
    private KeyCode _downKey = KeyCode.S;
    [SerializeField]
    private KeyCode _rightKey = KeyCode.D;
    [SerializeField]
    private KeyCode _leftKey = KeyCode.A;
    [SerializeField]
    private KeyCode _modeChangeKey = KeyCode.E;

    [Header("âÒì]ë¨ìx")]
    [SerializeField]
    private float _rotationSpeed = 50;

    private void Awake()
    {
        _freeRotationMode = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        _characters.AddRange(FindObjectsOfType<CharacterBase>());
    }

    // Update is called once per frame
    void Update()
    {
        Operation();
    }

    public void ChangeMode()
    {
        if (_freeRotationMode)
        {
            transform.rotation = _startRot;

            _freeRotationMode = false;
        }
        else
        {
            _startRot = transform.rotation;
            _endRot = transform.rotation;
            _xzAngle = 0;
            _yAngle = 0;

            foreach (var c in _characters)
                c.SetWaitEnable(true);

            _freeRotationMode = true;
        }
    }


    private void Operation()
    {
        if(Input.GetKeyDown(_modeChangeKey))
        {
            if(_freeRotationMode)
            {
                transform.rotation = _startRot;

                _freeRotationMode = false;
            }
            else
            {
                _startRot = transform.rotation;
                _endRot = transform.rotation;
                _xzAngle = 0;
                _yAngle = 0;

                foreach (var c in _characters)
                   c.SetWaitEnable(true);

                _freeRotationMode = true;
            }
        }


        if (_freeRotationMode)
        {
            if (Input.GetKey(_upKey))
            {
                _xzAngle -= _rotationSpeed * Time.deltaTime;
            }
            if (Input.GetKey(_downKey))
            {
                _xzAngle += _rotationSpeed * Time.deltaTime;
            }

            if (Input.GetKey(_rightKey))
            {
                _yAngle += _rotationSpeed * Time.deltaTime;
            }
            if (Input.GetKey(_leftKey))
            {
                _yAngle -= _rotationSpeed * Time.deltaTime;
            }

            transform.rotation = _endRot * Quaternion.Euler(0.0f, _yAngle, 0.0f) * Quaternion.AngleAxis(_xzAngle, Vector3.Cross(transform.position - new Vector3(0, 0, -1), Vector3.up));
        }
    }
}
