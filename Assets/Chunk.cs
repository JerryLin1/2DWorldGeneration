using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    int WIDTH = 8;
    int HEIGHT = 5;
    public float SCALE = 0.5f;
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
        for (int i = 0; i < HEIGHT; i++)
        {
            for (int j = 0; j < WIDTH; j++)
            {
                int tileId = Random.Range(1, 3);
                blocks[i, j] = tileId;
            }
        }

        for (int y = 0; y < HEIGHT; y++)
        {
            for (int x = 0; x < WIDTH; x++)
            {
                Vector2 tileToUse = tDirt;
                switch (blocks[y, x])
                {
                    case 1:
                        tileToUse = tDirt;
                        break;

                    case 2:
                        tileToUse = tStone;
                        break;
                }

                GenSquare(x, y, tileToUse);
            }
        }
        // UpdateMesh();
    }
    void GenSquare(int x, int y, Vector2 texture)
    {
        // Add 4 vertices around the block
        vertices.Add(new Vector3(x, y));
        vertices.Add(new Vector3(x + 1, y));
        vertices.Add(new Vector3(x + 1, y - 1));
        vertices.Add(new Vector3(x, y - 1, 0));

        // Add 6 triangle points to form 2 right angled triangles to form a square
        triangles.Add(squareCount * 4);
        triangles.Add((squareCount * 4) + 1);
        triangles.Add((squareCount * 4) + 3);
        triangles.Add((squareCount * 4) + 1);
        triangles.Add((squareCount * 4) + 2);
        triangles.Add((squareCount * 4) + 3);

        // Add 4 texture vertices around the block of the specific type
        uv.Add(new Vector2(SCALE * texture.x, SCALE * texture.y + SCALE));
        uv.Add(new Vector2(SCALE * texture.x + SCALE, SCALE * texture.y + SCALE));
        uv.Add(new Vector2(SCALE * texture.x + SCALE, SCALE * texture.y));
        uv.Add(new Vector2(SCALE * texture.x, SCALE * texture.y));

        squareCount++;
    }
    private void Update()
    {
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uv.ToArray();
        mesh.Optimize();
        mesh.RecalculateNormals();
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
    // private void OnDrawGizmos()
    // {
    //     if (vertices == null)
    //     {
    //         return;
    //     }
    //     Gizmos.color = Color.black;
    //     for (int i = 0; i < vertices.Count; i++)
    //     {
    //         Gizmos.DrawSphere(vertices[i], 0.1f);
    //     }
    // }
}
