using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuraCPUCon : MonoBehaviour
{
    public MiniGameCharacter _character;

    public List<GameObject> _cake = new List<GameObject>();
    public CharaManager _chara;
    private float speed = 1f;
    
    private Vector3 latestPos;  //前回のPosition
    private bool colFlag;  //前回のPosition
    private int rand;
    GameObject particle;

    void Start()
    {
        latestPos = transform.position;
        colFlag = false;
        rand = Random.Range(0, _cake.Count);
        particle = transform.Find("collisionEffect").gameObject;
        particle.SetActive(false);
    }

    void Update()
    {
        if (_cake.Count <= 0) return;

        int cakeCou = _cake.Count;
        _cake.RemoveAll(cake => cake == null);

        if(cakeCou!= _cake.Count) rand = Random.Range(0, _cake.Count);

        if (colFlag || !_chara._countFlg || _chara._endFlg) return;

        Vector3 lerpPos = new Vector3(_cake[rand].transform.position.x, transform.position.y, _cake[rand].transform.position.z);
        transform.position = Vector3.Lerp(transform.position, lerpPos, Time.deltaTime * speed);

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
        if (collision.gameObject.tag == "Player")
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
        while (outLength <= 1.5f)
        {
            outLength += 0.3f;
            transform.position -= transform.forward * 0.3f;
            yield return new WaitForSeconds(0.01f);
        }

        rand = Random.Range(0, _cake.Count);
        latestPos = transform.position;  //前回のPositionの更新
        colFlag = false;
    }
}