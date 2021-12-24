using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuraPlayerCon : MonoBehaviour
{
    public MiniGameCharacter _character;

    public CharaManager _chara;
    private float speed = 3f;

    private float moveX = 0f;
    private float moveZ = 0f;
    private Rigidbody rb;
    private Vector3 latestPos;  //前回のPosition
    private bool colFlag;  //前回のPosition
    GameObject particle;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        latestPos = transform.position;
        colFlag = false;
        particle = transform.Find("collisionEffect").gameObject;
        particle.SetActive(false);
    }

    void Update()
    {
        if (colFlag || !_chara._countFlg|| _chara._endFlg) return;

        //moveX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        //moveZ = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        KeyCode[] keyCode = new KeyCode[4];

        if(_character.Input.ControllerId == 0)
        {
            keyCode[0] = KeyCode.W;
            keyCode[1] = KeyCode.A;
            keyCode[2] = KeyCode.S;
            keyCode[3] = KeyCode.D;
        }
        if (_character.Input.ControllerId == 1)
        {
            keyCode[0] = KeyCode.T;
            keyCode[1] = KeyCode.F;
            keyCode[2] = KeyCode.G;
            keyCode[3] = KeyCode.H;
        }
        if (_character.Input.ControllerId == 2)
        {
            keyCode[0] = KeyCode.I;
            keyCode[1] = KeyCode.J;
            keyCode[2] = KeyCode.K;
            keyCode[3] = KeyCode.L;
        }
        if (_character.Input.ControllerId == 3)
        {
            keyCode[0] = KeyCode.UpArrow;
            keyCode[1] = KeyCode.LeftArrow;
            keyCode[2] = KeyCode.DownArrow;
            keyCode[3] = KeyCode.RightArrow;
        }
        
        moveX = 0f;
        moveZ = 0f;
        if (Input.GetKey(keyCode[0]))
        {
            moveZ = speed * Time.deltaTime;
        }
        if (Input.GetKey(keyCode[1]))
        {
            moveX = -speed * Time.deltaTime;
        }
        if (Input.GetKey(keyCode[2]))
        {
            moveZ = -speed * Time.deltaTime;
        }
        if (Input.GetKey(keyCode[3]))
        {
            moveX = speed * Time.deltaTime;
        }
        
        moveX += _character.Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        moveZ += -_character.Input.GetAxis("Vertical") * speed * Time.deltaTime;
        
        //rb.velocity = new Vector3(moveX, rb.velocity.y, moveZ);
        transform.position += new Vector3(moveX, 0f, moveZ);

        Vector3 diff = transform.position - latestPos;   //前回からどこに進んだかをベクトルで取得
        latestPos = transform.position;  //前回のPositionの更新

        diff.y = 0f;

        //ベクトルの大きさが0.01以上の時に向きを変える処理をする
        if (diff.magnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(diff); //向きを変更する
        }


        if (transform.position.y <= -20)
        {
            _chara.AddCake(gameObject);
            this.enabled = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            Control_SE.Get_Instance().Play_SE("Hit");
            colFlag = true;
            particle.SetActive(true);
            StartCoroutine(CollisionOut());
        }
    }

    public IEnumerator CollisionOut()
    {
        float outLength = 0f;
        while(outLength<=1.5f)
        {
            outLength += 0.3f;
            transform.position -= transform.forward * 0.3f;
            yield return new WaitForSeconds(0.01f);
        }

        latestPos = transform.position;  //前回のPositionの更新
        colFlag = false;
    }
}