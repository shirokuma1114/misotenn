using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingLaser : MonoBehaviour
{
    //���x
    Vector3 velocity;
    //���˂���Ƃ��̏����ʒu
    Vector3 position;
    // �����x
    [SerializeField] Vector3 acceleration;
    // �^�[�Q�b�g���Z�b�g����
    [SerializeField] Transform target;
    // ���e����
    float period = 2f;
    // �^�[�Q�b�g�Ƃ̋���
    float length;
    // ��~����܂ł̋���
    const float StopDistance=0.1f;
    // �����x�̃����_���͈�
    [SerializeField]
    const float velRange=2.0f;

    void Start()
    {
        SetInit();
     }


    void Update()
    {

        acceleration = Vector3.zero;

        //�^�[�Q�b�g�Ǝ������g�̍�
        var diff = target.position - transform.position;

        length = Mathf.Abs(diff.magnitude);

        if (length < StopDistance)
        {
            gameObject.SetActive(false);
        }

        //�����x�����߂Ă�炵��
        acceleration += (diff - velocity * period) * 2f
                        / (period * period);


        //�����x�����ȏゾ�ƒǔ����キ����
        if (acceleration.magnitude > 100f)
        {
            acceleration = acceleration.normalized * 100f;
        }

        // ���e���Ԃ����X�Ɍ��炵�Ă���
        period -= Time.deltaTime;

        // ���x�̌v�Z
        velocity += acceleration * Time.deltaTime;

    }

    void FixedUpdate()
    {
        //if (length < StopDistance) return;

        // �ړ�����
        transform.position += velocity * Time.deltaTime;
    }

    public void SetInit()
    {
        // ���e���ԃ��Z�b�g
        period = 2.0f;

        // �����ʒu��posion�Ɋi�[
        position = transform.position;

        // �����������_���ŗ^����
        velocity = new Vector3(Random.Range(-velRange, velRange), Random.Range(-velRange, velRange), 0);
    }

    void OnCollisionEnter()
    {
        // �����ɓ��������玩�����g���폜
        //Destroy(this.gameObject);

    }


}