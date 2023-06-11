using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class DrawLight : MonoBehaviour
{
    private Mesh mesh;
    private MeshFilter meshFilter;
    private List<Vector3> points = new List<Vector3>();

    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = new Mesh();
    }
    
    public void AddVertice(Vector3 point)
    {
        points.Add(point);
    }

    public void Clear()
    {
        points.Clear();
    }

    public void DrawMesh()
    {
        int vertexCount = points.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(points[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 2;
                triangles[i * 3 + 2] = i + 1;
            }
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }
}
