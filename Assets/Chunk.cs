using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    int WIDTH_RENDERED = 100;
    int HEIGHT_RENDERED = 80;

    // the percentage of the width of the spritesheet that each tile takes up
    // Remember, spritesheet must be square
    float tileScale = 0.5f;
    int[,] blocks;
    Mesh mesh;
    List<Vector3> vertices = new List<Vector3>();
    List<Vector2> uv = new List<Vector2>();
    List<int> triangles = new List<int>();

    // vertices and triangles for collider
    List<Vector3> colVertices = new List<Vector3>();
    List<int> colTriangles = new List<int>();
    MeshCollider col;
    int colCount;

    int squareCount = 0;

    private Vector2 tStone = new Vector2(0, 0);
    private Vector2 tDirt = new Vector2(1, 0);
    private void Start()
    {
        blocks = new int[HEIGHT_RENDERED, WIDTH_RENDERED];
        GenerateRandom();
        Generate();
    }
    void Generate()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        col = GetComponent<MeshCollider>();
        mesh.name = "Grid";

        GenMesh();
        UpdateMesh();
    }
    void GenerateRandom()
    {
        for (int i = 0; i < HEIGHT_RENDERED; i++)
        {
            for (int j = 0; j < WIDTH_RENDERED; j++)
            {
                int tileId = Random.Range(0, 3);
                blocks[i, j] = tileId;
            }
        }
    }
    void GenMesh()
    {
        for (int y = 0; y < HEIGHT_RENDERED; y++)
        {
            for (int x = 0; x < WIDTH_RENDERED; x++)
            {
                if (blocks[y, x] != 0)
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
                    GenCollider(x,y);
                    GenSquare(x, y, tileToUse);
                }
            }
        }
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
        uv.Add(new Vector2(tileScale * texture.x, tileScale * texture.y + tileScale));
        uv.Add(new Vector2(tileScale * texture.x + tileScale, tileScale * texture.y + tileScale));
        uv.Add(new Vector2(tileScale * texture.x + tileScale, tileScale * texture.y));
        uv.Add(new Vector2(tileScale * texture.x, tileScale * texture.y));

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
    void GenCollider(int x, int y)
    {
        // Add 4 vertices around the block
        colVertices.Add(new Vector3(x, y));
        colVertices.Add(new Vector3(x + 1, y));
        colVertices.Add(new Vector3(x + 1, y - 1));
        colVertices.Add(new Vector3(x, y - 1, 0));

        // Add 6 triangle points to form 2 right angled triangles to form a square
        colTriangles.Add(squareCount * 4);
        colTriangles.Add((squareCount * 4) + 1);
        colTriangles.Add((squareCount * 4) + 3);
        colTriangles.Add((squareCount * 4) + 1);
        colTriangles.Add((squareCount * 4) + 2);
        colTriangles.Add((squareCount * 4) + 3);

        colCount++;
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
        // vertices.Clear();
        // triangles.Clear();
        // uv.Clear();

        Mesh newMesh = new Mesh();
        newMesh.name = "newMesh";
        newMesh.vertices = colVertices.ToArray();
        newMesh.triangles = colTriangles.ToArray();
        col.sharedMesh = newMesh;

        colVertices.Clear();
        colTriangles.Clear();
        colCount = 0;
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
