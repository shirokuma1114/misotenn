using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quad : MonoBehaviour
{
    public Material mat;

    [SerializeField]private MeshFilter _meshFilter;
    private Mesh _mesh;

    private List<Vector3> vertexList = new List<Vector3>();
    private List<int> triangles = new List<int>();


    void Start()
    {
        _mesh = CreatePlaneMesh();
        _meshFilter.mesh = _mesh;

        GetComponent<MeshRenderer>().material = mat;
    }

    private Mesh CreatePlaneMesh()
    {
        var mesh = new Mesh();

        for (int i = 0; i < 4; i++)
        {
            vertexList.Add(new Vector3(i, 2, 0));//0�Ԓ��_
            vertexList.Add(new Vector3(i, -2, 0));//0�Ԓ��_
        }


        mesh.SetVertices(vertexList);//mesh�ɒ��_�Q���Z�b�g
        //mesh.SetTriangles(triangles.ToArray());//���b�V���ɂǂ̒��_�̏��ԂŖʂ���邩�Z�b�g
        return mesh;
    }

    private int cnt = 0;
    public void Update()
    {
        for (var i = 0; i < vertexList.Count; i ++)
        {
            if (i % 4 == 2 || i % 4 == 3)
            {
                var v = vertexList[i];
                v.y = Mathf.Sin((i + cnt) / 15.0f);
                vertexList[i] = v;
            }
        }
        cnt++;
        _mesh.SetVertices(vertexList);
    }
}
