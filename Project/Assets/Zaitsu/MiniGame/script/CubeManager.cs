using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _cube;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void PlayMove()
    {
        StartCoroutine(Shake(1.0f, 0.1f));
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        yield return new WaitForSeconds(7);

        while (_cube.Count>0)
        {
            var rand = Random.Range(0, _cube.Count);

            StartCoroutine(DoShake(duration, magnitude, rand));

            yield return new WaitForSeconds(4);
        }
    }

    private IEnumerator DoShake(float duration, float magnitude,int rand)
    {
        var pos = _cube[rand].transform.localPosition;

        var elapsed = 0f;

        while (elapsed < duration)
        {
            var x = pos.x + Random.Range(-1f, 1f) * magnitude;
            var y = pos.y + Random.Range(-1f, 1f) * magnitude;

            _cube[rand].transform.localPosition = new Vector3(x, y, pos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        _cube[rand].transform.localPosition = pos;

        StartCoroutine(DoFall(rand));
    }

    private IEnumerator DoFall(int rand)
    {
        while (_cube[rand].transform.localPosition.y > -20f)
        {
            var pos = _cube[rand].transform.localPosition;
            pos.y -= Time.deltaTime*10f;
            _cube[rand].transform.localPosition = pos;

            yield return null;
        }

        GameObject cube = _cube[rand];
        _cube.Remove(cube);
        Destroy(cube);
    }
}
