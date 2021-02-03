using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    float speed = 12.0F;
    private Vector2 moveDirection = Vector2.zero;
    Rigidbody2D rb;
    Grid grid;
    TileMapCreator renderer;
    Tilemap tilemapSolid;
    Camera cam;

    void Start()
    {
        // Store reference to attached component
        rb = GetComponent<Rigidbody2D>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        renderer = grid.transform.GetComponent<TileMapCreator>();
        tilemapSolid = grid.transform.GetChild(0).GetComponent<Tilemap>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && rb.velocity.y > -0.1 && rb.velocity.y <0.1)
        {
            rb.velocity = new Vector2(rb.velocity.x, 20);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            WorldGenerator.GenerateWorld(Random.Range(0, 10000));
        }
        if (Input.GetMouseButton(0)) {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int mouseTilePos = grid.WorldToCell(mousePos);
            if (WorldGenerator.IsValidCell(mouseTilePos.x, mouseTilePos.y) && WorldGenerator.GetCell(mouseTilePos.x, mouseTilePos.y) != 0) {
                WorldGenerator.DestroyTile(mouseTilePos.x, mouseTilePos.y);
                renderer.UpdateTile(mouseTilePos.x, mouseTilePos.y);
            }
        }
        float hAxis = Input.GetAxis("Horizontal");
        hAxis *= speed;
        rb.velocity = new Vector2(hAxis, rb.velocity.y);
    }
}
