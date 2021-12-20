using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StandbyFade : MonoBehaviour
{
    private Image _image;
    [SerializeField]
    private float _fadeSpeed = 1.0f;
    [SerializeField]
    private GameObject _standby;

    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
    }

    public IEnumerator FadeStart()
    {
        var c = _image.color;
        float a = 0.0f;
        while (a <= 1.0f)
        {
            a += Time.deltaTime * _fadeSpeed;
            _image.color = new Color(c.r, c.g, c.b, a);
            Debug.Log("入っている1");
            yield return null;
        }

        // ゲームスタート
        StartCoroutine(FadeEnd());
        _standby.SetActive(false);
    }

    public IEnumerator FadeEnd()
    {
        var c = _image.color;
        float a = 1.0f;
        while (a >= 0.0f)
        {
            a -= Time.deltaTime * _fadeSpeed;
            _image.color = new Color(c.r, c.g, c.b, a);
            Debug.Log("入っている2");
            yield return null;
        }
    }
}
