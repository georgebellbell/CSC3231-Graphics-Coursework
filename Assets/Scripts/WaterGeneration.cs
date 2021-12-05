using System.Collections.Generic;
using UnityEngine;
public class WaterGeneration : MonoBehaviour
{

    [SerializeField] float sizeOfWater = 1f;
    [SerializeField] int gridSize = 16;

    int vertexCount;
    
    List<Vector3> vertices = new List<Vector3>();
    List<Vector3> normals = new List<Vector3>();
    List<Vector2> UVs = new List<Vector2>();
    List<int> triangles = new List<int>();

    MeshFilter meshFilter;
    private void OnEnable()
    {
        vertexCount = gridSize + 1;
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = GenerateMesh();
    }

    
    private Mesh GenerateMesh()
    {
        Mesh newMesh = new Mesh();

        GenerateVertices();
        GenerateTriangles();

        newMesh.SetVertices(vertices);
        newMesh.SetNormals(normals);
        newMesh.SetUVs(0, UVs);
        newMesh.SetTriangles(triangles, 0);

        return newMesh;

    }

    private void GenerateVertices()
    {
        for (int i = 0; i < gridSize + 1; i++)
        {
            for (int j = 0; j < gridSize + 1; j++)
            {
                Vector3 newVertexPosition = new Vector3(-sizeOfWater * 0.5f + sizeOfWater *
                    (i / ((float)gridSize)), 0, -sizeOfWater * 0.5f + sizeOfWater * (j / ((float)gridSize)));
                vertices.Add(newVertexPosition);
                normals.Add(Vector3.up);
                UVs.Add(new Vector2(i / (float)gridSize, j / (float)gridSize));

            }
        }
    }
    void GenerateTriangles()
    {
        for (int vertex = 0; vertex < vertexCount * vertexCount - vertexCount; vertex++)
        {

            if ((vertex + 1) % vertexCount == 0)
            {
                continue;
            }
            triangles.AddRange(new List<int>() {
                vertex + 1 + vertexCount, vertex + vertexCount, vertex, vertex,
                vertex + 1, vertex + 1 + vertexCount
            });
        }
    }

    
}
