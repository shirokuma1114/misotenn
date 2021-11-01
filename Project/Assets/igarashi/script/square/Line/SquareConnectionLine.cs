using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SquareConnectionLine : MonoBehaviour
{
    private LineRenderer _lineRenderer;

    [SerializeField]
    private float _floatingVolume = 0.3f;
    [SerializeField]
    private int _curvePointNum = 10;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();

        DrawLine();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DrawLine()
    {
        List<Vector3> positions = new List<Vector3>();
        List<SquareBase> squares = new List<SquareBase>();
        squares.AddRange(FindObjectsOfType<SquareBase>());

        var japan = GameObject.Find("Japan");
        Vector3 start = transform.InverseTransformPoint(japan.transform.position);
        start += start.normalized * _floatingVolume;
        positions.Add(start);

        GameObject endSquare = japan.GetComponent<SquareBase>().OutConnects[0].gameObject;
        Vector3 end = transform.InverseTransformPoint(endSquare.transform.position);
        end += end.normalized * _floatingVolume;

        for (int i = 0; i < squares.Count; i++)
        {
            for (int t = 1; t <= _curvePointNum; t++)
            {
                var p = Vector3.Slerp(start, end, (float)t /_curvePointNum);
                positions.Add(p);
            }

            start = end;
            endSquare = endSquare.GetComponent<SquareBase>().OutConnects[0].gameObject;
            end = transform.InverseTransformPoint(endSquare.transform.position);
            end += end.normalized * _floatingVolume;
        }

        _lineRenderer.positionCount = positions.Count;
        _lineRenderer.SetPositions(positions.ToArray());
    }

    private Vector3 BezierLerp(Vector3 start, Vector3 end, Vector3 control, float t)
    {
        Vector3 sc = Vector3.Lerp(start, control, t);
        Vector3 ce = Vector3.Lerp(control, end, t);

        Vector3 ret = Vector3.Lerp(sc, ce, t);        
        return ret;
    }
}
