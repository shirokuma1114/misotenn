using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SouvenirScrambleControllerBase : MonoBehaviour
{
    protected Rigidbody _rb;

    protected MiniGameCharacter _character;
    public MiniGameCharacter Character => _character;

    protected float _moveSpeed = 10f;

    private Camera _camera;

    private int _point;
    public int Point => _point;

    private SouvenirScrambleManager _manager;

    private SouvenirScrambleControllerUI _ui;
    public SouvenirScrambleControllerUI UI => _ui;


    public void Init(MiniGameCharacter character,SouvenirScrambleManager manager,SouvenirScrambleControllerUI ui)
    {
        _character = character;
        _manager = manager;

        _ui = ui;
        _ui.Init(character.Name,_character.Icon);
    }

    public void AddPoint()
    {
        _point++;
    }

    //----------------------

    protected void Start()
    {
        _rb = GetComponent<Rigidbody>();

        _camera = Camera.main;

        GetComponent<Animator>().SetBool("Idle", false);
    }

    protected void Update()
    {
        if (_manager.State != SouvenirScrambleManager.SouvenirScrambleState.PLAY)
            return;

        Operation();

        ClampViewPort();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

        if (!rb)
            return;

        Vector3 dir = collision.gameObject.transform.position - transform.position;
        dir.Normalize();

        rb.AddForce(dir * 100.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_manager.State != SouvenirScrambleManager.SouvenirScrambleState.PLAY)
            return;

        var souvenir = other.GetComponent<SouvenirScrambleItem>();
        if(souvenir)
        {
            AddPoint();
            _ui.SetItemCounter(Point);
            Control_SE.Get_Instance().Play_SE("ƒAƒCƒeƒ€");
            Destroy(souvenir.gameObject);
        }
    }


    virtual public void Operation()
    {

    }

    private void ClampViewPort()
    {
        Vector3 vpos = _camera.WorldToViewportPoint(transform.position);
        bool isOutSide = false;
        if (vpos.x < 0)
        {
            vpos.x = 0;
            isOutSide = true;
        }
        if (vpos.x > 1)
        {
            vpos.x = 1;
            isOutSide = true;
        }
        if (vpos.y < 0)
        {
            vpos.y = 0;
            isOutSide = true;
        }
        if (vpos.y > 1)
        {
            vpos.y = 1;
            isOutSide = true;
        }
        if (isOutSide)
        {
            transform.position = _camera.ViewportToWorldPoint(vpos);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
    }
}
