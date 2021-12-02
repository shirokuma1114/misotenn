using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    private Renderer _renderer;
    private int _Width = 0;
    private int _Cutoff = 0;

    [SerializeField]
    private float time = 1f; // �Đ�����
    [SerializeField]
    private float waitTime = 1f; // �Đ��܂ł̑҂�����

    private float duration = 0f; // �c����
    private float halfTime = 0f; // �Đ����Ԃ̔���
    [SerializeField]
    GameObject particle;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _Width = Shader.PropertyToID("_Width");
        _Cutoff = Shader.PropertyToID("_CutOff");

        _renderer.material.SetFloat(_Cutoff, 0f);
        _renderer.material.SetFloat(_Width, 1f);

        halfTime = time / 4f * 3f; // �����Ƃ�����4/3�ɂ��Ă���̂́A�����ڂ̒����̂���
        duration = time;
    }

    // Update is called once per frame
    void Update()
    {
        var originalMaterial = new Material(_renderer.material);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            particle.SetActive(true);
            StartCoroutine(BlinkerCoroutine());
        }
    }

    IEnumerator BlinkerCoroutine()
    {
        for (;;)
        {
            float delta = Time.deltaTime;

            duration -= delta;
            if (duration < 0f) duration = 0f;

            // �������l�̃A�j���[�V�����i�Đ����Ԃ̏㔼���̎��Ԃ�1�`0�ɐ��ځj
            float cutoff = (duration - halfTime) / halfTime;
            if (cutoff < 0f) cutoff = 0f;

            // ���̃A�j���[�V�����i�Đ����Ԃ̉������̎��Ԃ�1�`0�ɐ��ځj
            float width = (halfTime - duration) / halfTime;
            if (width < 0f) width = 0f;
            width = 1f - width;


            // �S�q�I�u�W�F�N�g�̃}�e���A���̃V�F�[�_�ɒl��n��
            _renderer.material.SetFloat(_Cutoff, cutoff);
            _renderer.material.SetFloat(_Width, width);

            if (duration <= 0f)
            {
                duration = time;
                _renderer.material.SetFloat(_Cutoff, 1.0f);
                _renderer.material.SetFloat(_Width, 1.0f);
                yield break;
            }

            yield return null;
        }
    }
}
