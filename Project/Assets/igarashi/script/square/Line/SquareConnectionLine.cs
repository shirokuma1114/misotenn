using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SquareConnectionLine : MonoBehaviour
{
    private LineRenderer _lineRenderer;

    private List<Vector3> _positions = new List<Vector3>();
    public List<Vector3> GetPositions => _positions;

    //<マス,_positionsの中のマスの始まりのindex>
    private Dictionary<GameObject, int> _squareStartIndex = new Dictionary<GameObject, int>();
    public Dictionary<GameObject, int> GetSquareStartIndex => _squareStartIndex;

    Quaternion _startRot;

    [SerializeField]
    private float _floatingVolume = 0.3f;
    [SerializeField]
    private int _curvePointNum = 10;
    public int CurvePointNum => _curvePointNum;



    //===========================================

    // Start is called before the first frame update
    void Awake()
    {
        //japan(初期位置)の回転
        //初期化段階だとまだ地球がjapanに向いてないから手打ちした
        _startRot = Quaternion.Euler(-32.0f, 0, 0);
    }

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.useWorldSpace = false;
        _lineRenderer.loop = true;

        DrawLineFromEarthRotate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DrawLine()
    {
        List<SquareBase> squares = new List<SquareBase>();
        squares.AddRange(FindObjectsOfType<SquareBase>());

        var japan = GameObject.Find("Japan");
        Vector3 start = transform.InverseTransformPoint(japan.transform.position);
        start += start.normalized * _floatingVolume;
        _positions.Add(start);

        GameObject endSquare = japan.GetComponent<SquareBase>().OutConnects[0].gameObject;
        Vector3 end = transform.InverseTransformPoint(endSquare.transform.position);
        end += end.normalized * _floatingVolume;

        for (int i = 0; i < squares.Count; i++)
        {
            for (int t = 1; t <= _curvePointNum; t++)
            {
                var p = Vector3.Slerp(start, end, (float)t / _curvePointNum);
                _positions.Add(p);
            }

            start = end;
            endSquare = endSquare.GetComponent<SquareBase>().OutConnects[0].gameObject;
            end = transform.InverseTransformPoint(endSquare.transform.position);
            end += end.normalized * _floatingVolume;
        }

        _lineRenderer.positionCount = _positions.Count;
        _lineRenderer.SetPositions(_positions.ToArray());        
    }

    private void DrawLineFromEarthRotate()
    {   
        List<SquareBase> squares = new List<SquareBase>();
        squares.AddRange(FindObjectsOfType<SquareBase>());

        var japan = GameObject.Find("Japan");
        Vector3 start = transform.InverseTransformPoint(japan.transform.position).normalized;
        _positions.Add(start * _floatingVolume);
        _squareStartIndex.Add(japan, 0);

        GameObject endSquare = japan.GetComponent<SquareBase>().OutConnects[0].gameObject;
        Vector3 end = transform.InverseTransformPoint(endSquare.transform.position).normalized;

        for (int i = 0; i < squares.Count; i++)
        {
            EarthMoveSimulationLerp(start, _startRot, end);

            if (endSquare == japan)
                break;

            start = end;
            _squareStartIndex.Add(endSquare, (i + 1) * _curvePointNum);

            endSquare = endSquare.GetComponent<SquareBase>().OutConnects[0].gameObject;
            end = transform.InverseTransformPoint(endSquare.transform.position).normalized;
        }

        _lineRenderer.positionCount = _positions.Count;
        _lineRenderer.SetPositions(_positions.ToArray());
    }

    private Vector3 BezierLerp(Vector3 start, Vector3 end, Vector3 control, float t)
    {
        Vector3 sc = Vector3.Lerp(start, control, t);
        Vector3 ce = Vector3.Lerp(control, end, t);

        Vector3 ret = Vector3.Lerp(sc, ce, t);        
        return ret;
    }

    private void EarthMoveSimulationLerp(Vector3 startPos,Quaternion startRot,Vector3 endPos)
    {
        Quaternion endRot;

        //EarthMoveのアルゴリズム
        Vector3 xzTargetPos = new Vector3(endPos.x, 0.0f, endPos.z);
        float angle = Vector3.SignedAngle(xzTargetPos, endPos, Vector3.Cross(endPos, -Vector3.up));
        endRot = Quaternion.AngleAxis(angle, Vector3.Cross(endPos, Vector3.up));

        Vector3 xzPrevTarget = new Vector3(0.0f, 0.0f, -1.0f);
        Vector3 xzTarget = new Vector3(endPos.x, 0.0f, endPos.z);
        float xzAngle = Vector3.SignedAngle(xzPrevTarget, xzTargetPos, -Vector3.up);
        endRot = Quaternion.Euler(0.0f, xzAngle, 0.0f) * endRot;


        for (int t = 1; t <= _curvePointNum; t++)
        {
            GameObject virtualEarth = new GameObject();

            var lerpRot = Quaternion.Lerp(startRot, endRot, (float)t / _curvePointNum);
            virtualEarth.transform.rotation = lerpRot;

            var p = virtualEarth.transform.InverseTransformPoint(new Vector3(0, 0, -_floatingVolume));
            _positions.Add(p);

            Destroy(virtualEarth);
        }

        _startRot = endRot;
    }
}
