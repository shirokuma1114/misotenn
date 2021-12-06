using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointController : MonoBehaviour
{
    void Start()
    {
        //MeshFilter meshFilter = GetComponent<MeshFilter>();
        //meshFilter.mesh.SetIndices(meshFilter.mesh.GetIndices(0), MeshTopology.Points, 0);

        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>(); ;

        //meshFilters = GetComponentsInChildren<MeshFilter>();

        // 全子オブジェクトのマテリアルのシェーダに値を渡す
        foreach (var meshFilter in meshFilters)
        {
            if (meshFilter.mesh != null)
            {
                meshFilter.mesh.SetIndices(meshFilter.mesh.GetIndices(0), MeshTopology.Lines, 0);
            }
        }
    }
}