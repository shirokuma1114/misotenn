using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    private Renderer _renderer;
    private int _Width = 0;
    private int _Cutoff = 0;

    [SerializeField]
    private float time = 1f; // 再生時間
    [SerializeField]
    private float waitTime = 1f; // 再生までの待ち時間

    private float duration = 0f; // 残時間
    private float halfTime = 0f; // 再生時間の半分
    [SerializeField]
    GameObject particle;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _Width = Shader.PropertyToID("_Width");
        _Cutoff = Shader.PropertyToID("_CutOff");

        _renderer.material.SetFloat(_Cutoff, 0f);
        _renderer.material.SetFloat(_Width, 1f);

        halfTime = time / 4f * 3f; // 半分といいつつ4/3にしているのは、見た目の調整のため
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

            // しきい値のアニメーション（再生時間の上半分の時間で1～0に推移）
            float cutoff = (duration - halfTime) / halfTime;
            if (cutoff < 0f) cutoff = 0f;

            // 幅のアニメーション（再生時間の下半分の時間で1～0に推移）
            float width = (halfTime - duration) / halfTime;
            if (width < 0f) width = 0f;
            width = 1f - width;


            // 全子オブジェクトのマテリアルのシェーダに値を渡す
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
