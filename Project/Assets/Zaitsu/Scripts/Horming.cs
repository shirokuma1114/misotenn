using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horming : MonoBehaviour
{
    ParticleSystem ps;
    ParticleSystem.Particle[] m_Particles;

    // �^�[�Q�b�g���Z�b�g����
    [SerializeField] private Transform target;
    [SerializeField] private float _rotSpeed = 180.0f;  // 1�b�Ԃɉ�]����p�x
   

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        m_Particles = new ParticleSystem.Particle[ps.main.maxParticles];
        int numParticlesAlive = ps.GetParticles(m_Particles);
        for (int i = 0; i < numParticlesAlive; i++)
        {
            var velocity = m_Particles[i].velocity;
            var position = m_Particles[i].position;

            // �^�[�Q�b�g�ւ̃x�N�g��
            var direction = ps.transform.InverseTransformPoint(target.TransformPoint(target.position)) - position;

            // �^�[�Q�b�g�܂ł̊p�x
            float angleDiff = Vector3.Angle(velocity, direction);

            // ��]�p
            float angleAdd = (_rotSpeed * Time.deltaTime);

            // �^�[�Q�b�g�֌�����N�H�[�^�j�I��
            Quaternion rotTarget = Quaternion.FromToRotation(velocity, direction);
            if (angleDiff <= angleAdd)
            {
                // �^�[�Q�b�g����]�p�ȓ��Ȃ犮�S�Ƀ^�[�Q�b�g�̕�������
                m_Particles[i].velocity = (rotTarget * velocity);
            }
            else
            {
                // �^�[�Q�b�g����]�p�̊O�Ȃ�A�w��p�x�����^�[�Q�b�g�Ɍ�����
                float t = (angleAdd / angleDiff);
                m_Particles[i].velocity = Quaternion.Slerp(Quaternion.identity, rotTarget, t) * velocity;
            }
        }
        ps.SetParticles(m_Particles, numParticlesAlive);
    }
}
