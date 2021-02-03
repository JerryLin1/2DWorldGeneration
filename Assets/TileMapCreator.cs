using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapCreator : MonoBehaviour
{
    GameObject solidObject;
    Grid grid;
    Tilemap tilemapSolid;
    int RENDER_DIST_X = 25;
    int RENDER_DIST_Y = 25;
    int REGENERATE_DIST = 25;
    public Transform playerTransform;
    int[,] worldData;
    Tile[] tileL;
    Vector3Int generatedPosition;
    void Start()
    {
        solidObject = transform.GetChild(0).gameObject;
        grid = GetComponent<Grid>();
        tilemapSolid = solidObject.GetComponent<Tilemap>();
        WorldGenerator.GenerateWorld();
        worldData = WorldGenerator.GetWorldData();

        playerTransform.position = new Vector3(worldData.GetLength(1) / 2, worldData.GetLength(0) / 2 + 1, 0);

        Vector3Int startPosition = grid.WorldToCell(playerTransform.position);
        Debug.Log(worldData[startPosition.y, startPosition.x] == 0);

        // Go up by 1 if player spawns in solid, until they aren't
        while (worldData[startPosition.y, startPosition.x] != 0)
        {
            Debug.Log(playerTransform.position);
            startPosition += new Vector3Int(0, 3, 0);
            playerTransform.position = grid.CellToWorld(startPosition);
        }
        generatedPosition = grid.WorldToCell(playerTransform.position);

        tileL = Resources.FindObjectsOfTypeAll<Tile>();
        foreach (Tile t in tileL) Debug.Log(t.name);
    }
    void UpdateTilemap()
    {
        Vector3Int playerPosition = grid.WorldToCell(playerTransform.position);
        if (Vector3Int.Distance(playerPosition, generatedPosition) > REGENERATE_DIST)
        {
            Debug.Log("joe");
            tilemapSolid.ClearAllTiles();
            generatedPosition = playerPosition;
        }
        // Iterate through tiles around player
        for (int x = playerPosition.x - (RENDER_DIST_X / 2); x <= playerPosition.x + (RENDER_DIST_X / 2); x++)
        {
            for (int y = playerPosition.y - (RENDER_DIST_Y / 2); y <= playerPosition.y + (RENDER_DIST_Y / 2); y++)
            {
                if (tilemapSolid.GetTile(new Vector3Int(x, y, 0)) == null)
                {
                    int tileId = worldData[y, x];
                    if (tileId == 1)
                    {
                        tilemapSolid.SetTile(new Vector3Int(x, y, 0), tileL[1]);
                    }
                }
            }
        }
    }

    void Update()
    {
        UpdateTilemap();
    }
}
