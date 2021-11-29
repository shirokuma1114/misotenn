using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingLaser : MonoBehaviour
{
    //速度
    Vector3 velocity;
    //発射するときの初期位置
    Vector3 position;
    // 加速度
    [SerializeField] Vector3 acceleration;
    // ターゲットをセットする
    [SerializeField] Transform target;
    // 着弾時間
    float period = 2f;
    // ターゲットとの距離
    float length;
    // 停止するまでの距離
    const float StopDistance=0.1f;
    // 初速度のランダム範囲
    [SerializeField]
    const float velRange=2.0f;

    void Start()
    {
        SetInit();
     }


    void Update()
    {

        acceleration = Vector3.zero;

        //ターゲットと自分自身の差
        var diff = target.position - transform.position;

        length = Mathf.Abs(diff.magnitude);

        if (length < StopDistance)
        {
            gameObject.SetActive(false);
        }

        //加速度を求めてるらしい
        acceleration += (diff - velocity * period) * 2f
                        / (period * period);


        //加速度が一定以上だと追尾を弱くする
        if (acceleration.magnitude > 100f)
        {
            acceleration = acceleration.normalized * 100f;
        }

        // 着弾時間を徐々に減らしていく
        period -= Time.deltaTime;

        // 速度の計算
        velocity += acceleration * Time.deltaTime;

    }

    void FixedUpdate()
    {
        //if (length < StopDistance) return;

        // 移動処理
        transform.position += velocity * Time.deltaTime;
    }

    public void SetInit()
    {
        // 着弾時間リセット
        period = 2.0f;

        // 初期位置をposionに格納
        position = transform.position;

        // 初速をランダムで与える
        velocity = new Vector3(Random.Range(-velRange, velRange), Random.Range(-velRange, velRange), 0);
    }

    void OnCollisionEnter()
    {
        // 何かに当たったら自分自身を削除
        //Destroy(this.gameObject);

    }


}