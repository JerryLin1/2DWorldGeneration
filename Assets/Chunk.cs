using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Chunk : MonoBehaviour
{
    int WIDTH = 8;
    int HEIGHT = 5;
    float SCALE = 0.5f;
    int[,] blocks;
    Mesh mesh;
    List<Vector3> vertices = new List<Vector3>();
    List<Vector2> uv = new List<Vector2>();
    List<int> triangles = new List<int>();
    int squareCount = 0;

    private Vector2 tStone = new Vector2(0, 0);
    private Vector2 tDirt = new Vector2(1, 0);
    private void Start()
    {
        blocks = new int[HEIGHT, WIDTH];
        Generate();
    }
    void Generate()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Grid";

        for (int ti = 0, vi = 0, y = 0; y < HEIGHT; y++, vi++)
        {
            for (int x = 0; x < WIDTH; x++, ti += 6, vi++)
            {
                if (blocks[y, x] == 1)
                {
                    GenSquare(x, y, tStone);
                }
            }
        }
    }
    void GenSquare(int x, int y, Vector2 texture)
    {
        vertices.Add(new Vector3(x, y, 0));
        vertices.Add(new Vector3(x + 1, y, 0));
        vertices.Add(new Vector3(x + 1, y - 1, 0));
        vertices.Add(new Vector3(x, y - 1, 0));

        triangles.Add(squareCount * 4);
        triangles.Add((squareCount * 4) + 1);
        triangles.Add((squareCount * 4) + 3);
        triangles.Add((squareCount * 4) + 1);
        triangles.Add((squareCount * 4) + 2);
        triangles.Add((squareCount * 4) + 3);

        uv.Add(new Vector2(SCALE * texture.x, SCALE * texture.y + SCALE));
        uv.Add(new Vector2(SCALE * texture.x + SCALE, SCALE * texture.y + SCALE));
        uv.Add(new Vector2(SCALE * texture.x + SCALE, SCALE * texture.y));
        uv.Add(new Vector2(SCALE * texture.x, SCALE * texture.y));

        squareCount++;

    }
    private void Update()
    {
        Update();
    }
    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uv.ToArray();
        mesh.Optimize();
        mesh.RecalculateNormals();

        squareCount = 0;
        vertices.Clear();
        triangles.Clear();
        uv.Clear();
    }
    private void OnDrawGizmos()
    {
        if (vertices == null)
        {
            return;
        }
        Gizmos.color = Color.black;
        for (int i = 0; i < vertices.Count; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }
    void GenerateBlock(int width, int height)
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                int tileId = Random.Range(1, 3);
                blocks[i, j] = tileId;
            }
        }
    }
}
