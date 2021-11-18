using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horming : MonoBehaviour
{
    ParticleSystem ps;
    ParticleSystem.Particle[] m_Particles;

    // �^�[�Q�b�g���Z�b�g����
    //[SerializeField] private Transform target;
    //[SerializeField] private float _rotSpeed = 180.0f;  // 1�b�Ԃɉ�]����p�x
   

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    public float threshold = 100f;
    public float intensity = 1f;

    // �^�[�Q�b�g���Z�b�g����
    public Transform target;

    void Update()
    {
        m_Particles = new ParticleSystem.Particle[ps.main.maxParticles];
        int numParticlesAlive = ps.GetParticles(m_Particles);
        for (int i = 0; i < numParticlesAlive; i++)
        {
            var velocity = ps.transform.TransformPoint(m_Particles[i].velocity);
            var position = ps.transform.TransformPoint(m_Particles[i].position);

            var period = m_Particles[i].remainingLifetime * 0.9f;

            //�^�[�Q�b�g�Ǝ������g�̍�
            var diff = target.position - position;
            Vector3 accel = (diff - velocity * period) * 2f / (period * period);

            //�����x�����ȏゾ�ƒǔ����キ����
            if (accel.magnitude > threshold)
            {
                accel = accel.normalized * threshold;
            }

            // ���x�̌v�Z
            velocity += accel * Time.deltaTime * intensity;
            m_Particles[i].velocity = ps.transform.InverseTransformPoint(velocity);
        }
        ps.SetParticles(m_Particles, numParticlesAlive);
    }
}
