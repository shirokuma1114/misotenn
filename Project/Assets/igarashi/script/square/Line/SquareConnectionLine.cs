using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SquareConnectionLine : MonoBehaviour
{
    private LineRenderer _lineRenderer;


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
        start *= 1.05f;

        GameObject endSquare = japan.GetComponent<SquareBase>().OutConnects[0].gameObject;
        Vector3 end = transform.InverseTransformPoint(endSquare.transform.position);
        end *= 1.05f;

        for (int i = 0; i < squares.Count; i++)
        {
            for (float t = 0; t <= 1.1f; t += 0.1f)
            {
                var p = Vector3.Slerp(start, end, t);
                positions.Add(p);
            }

            start = end;
            endSquare = endSquare.GetComponent<SquareBase>().OutConnects[0].gameObject;
            end = transform.InverseTransformPoint(endSquare.transform.position);
            end *= 1.05f;
        }

        _lineRenderer.positionCount = positions.Count;
        _lineRenderer.SetPositions(positions.ToArray());
    }

    private Vector3 SampleCurve(Vector3 start, Vector3 end, Vector3 control, float t)
    {
        Vector3 Q0 = Vector3.Lerp(start, control, t);
        Vector3 Q1 = Vector3.Lerp(control, end, t);
        Vector3 Q2 = Vector3.Lerp(Q0, Q1, t);

        return Q2;
    }
}
