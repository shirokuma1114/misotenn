using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuraPlayerCon : MonoBehaviour
{
    [SerializeField]
    private CharaManager _chara;
    [SerializeField] private float speed = 3f;

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

        moveX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        moveZ = Input.GetAxis("Vertical") * speed * Time.deltaTime;
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
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Player")
        {
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