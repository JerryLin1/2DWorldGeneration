using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapCreator : MonoBehaviour
{
    GameObject FgObject;
    Grid grid;
    Tilemap tilemapFg;
    Tilemap tilemapBg;
    int RENDER_DIST_X = 80;
    int RENDER_DIST_Y = 40;
    int REGENERATE_DIST = 25;
    public Transform cameraTransform;
    GameObject player;
    int[,] worldDataFg;
    int[,] worldDataBg;
    Tile[] tileL;
    Vector3Int generatedPosition;
    void Start()
    {
        FgObject = transform.GetChild(0).gameObject;
        grid = GetComponent<Grid>();
        tilemapFg = grid.transform.GetChild(0).GetComponent<Tilemap>();
        tilemapBg = grid.transform.GetChild(1).GetComponent<Tilemap>();
        WorldGenerator.GenerateWorld(UnityEngine.Random.Range(0, 10000));
        UpdateWorldData();
        player = GameObject.Find("Player");

        player.transform.position = new Vector3(worldDataFg.GetLength(1) / 2, worldDataFg.GetLength(0) / 2 + 10, 0);

        generatedPosition = grid.WorldToCell(cameraTransform.position);

        tileL = Resources.FindObjectsOfTypeAll<Tile>();
        Array.Reverse(tileL);

        foreach (Tile t in tileL) Debug.Log(t.name);

        WorldGenerator.e_worldUpdated.AddListener(UpdateTilemap);
        WorldGenerator.e_worldUpdated.AddListener(ClearAllTiles);
        WorldGenerator.e_worldUpdated.AddListener(UpdateWorldData);
    }
    void UpdateTilemap()
    {
        Vector3Int playerPosition = grid.WorldToCell(cameraTransform.position);
        if (Vector3Int.Distance(playerPosition, generatedPosition) > REGENERATE_DIST)
        {
            tilemapFg.ClearAllTiles();
            tilemapBg.ClearAllTiles();
            generatedPosition = playerPosition;
        }
        // Iterate through tiles around player
        for (int x = playerPosition.x - (RENDER_DIST_X / 2); x <= playerPosition.x + (RENDER_DIST_X / 2); x++)
        {
            for (int y = playerPosition.y - (RENDER_DIST_Y / 2); y <= playerPosition.y + (RENDER_DIST_Y / 2); y++)
            {
                if (tilemapFg.GetTile(new Vector3Int(x, y, 0)) == null && WorldGenerator.IsValidCell(x, y))
                {
                    UpdateTileFg(x, y);
                }
                if (tilemapBg.GetTile(new Vector3Int(x, y, 0)) == null && WorldGenerator.IsValidCell(x, y))
                {
                    UpdateTileBg(x, y);
                }
            }
        }
    }

    void Update()
    {
        UpdateTilemap();
    }
    public void UpdateTileFg(int x, int y)
    {
        worldDataFg = WorldGenerator.GetWorldDataFg();
        int tileId = worldDataFg[y, x];
        if (tileId == 0) tilemapFg.SetTile(new Vector3Int(x, y, 0), null);
        else tilemapFg.SetTile(new Vector3Int(x, y, 0), tileL[tileId - 1]);
    }
    public void UpdateTileBg(int x, int y)
    {
        worldDataBg = WorldGenerator.GetWorldDataBg();
        int tileId = worldDataBg[y, x];
        if (tileId == 0) tilemapBg.SetTile(new Vector3Int(x, y, 0), null);
        else tilemapBg.SetTile(new Vector3Int(x, y, 0), tileL[tileId - 1]);
    }
    void ClearAllTiles() { tilemapFg.ClearAllTiles(); }
    void UpdateWorldData()
    {
        worldDataFg = WorldGenerator.GetWorldDataFg();
        worldDataBg = WorldGenerator.GetWorldDataBg();
    }
}
