using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierDissolve : MonoBehaviour
{
    private Renderer _renderer;
    private int _Width = 0;
    private int _Cutoff = 0;

    [SerializeField]
    private float _speed = 1f; // 再生時間

    [SerializeField]
    GameObject particle;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _Width = Shader.PropertyToID("_Width");
        _Cutoff = Shader.PropertyToID("_CutOff");

        _renderer.material.SetFloat(_Cutoff, 0f);
        _renderer.material.SetFloat(_Width, 0f);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            particle.SetActive(true);
            StartCoroutine(BlinkerCoroutine());
        }
    }

    IEnumerator BlinkerCoroutine()
    {
        float delta = 0;
        for (;;)
        {
            delta += Time.deltaTime* _speed;

            // 全子オブジェクトのマテリアルのシェーダに値を渡す
            _renderer.material.SetFloat(_Cutoff, delta);
            _renderer.material.SetFloat(_Width, delta);

            if (delta >= 1f)
            {
                //EndBarrier();
                yield break;
            }

            yield return null;
        }
    }

    public void StartBarrier()
    {
        particle.SetActive(true);
        StartCoroutine(BlinkerCoroutine());
    }

    public void EndBarrier()
    {
        particle.SetActive(false);
        _renderer.material.SetFloat(_Cutoff, 0.0f);
        _renderer.material.SetFloat(_Width, 0.0f);
    }
}
