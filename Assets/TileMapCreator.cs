using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapCreator : MonoBehaviour
{
    GameObject solidObject;
    Grid grid;
    Tilemap tilemapSolid;
    int RENDER_DIST_X = 40;
    int RENDER_DIST_Y = 25;
    int REGENERATE_DIST = 25;
    public Transform cameraTransform;
    GameObject player;
    int[,] worldData;
    Tile[] tileL;
    Vector3Int generatedPosition;
    void Start()
    {
        solidObject = transform.GetChild(0).gameObject;
        grid = GetComponent<Grid>();
        tilemapSolid = solidObject.GetComponent<Tilemap>();
        WorldGenerator.GenerateWorld(Random.Range(0, 10000));
        UpdateWorldData();
        player = GameObject.Find("Player");

        player.transform.position = new Vector3(worldData.GetLength(1) / 2, worldData.GetLength(0) / 2 + 10, 0);

        generatedPosition = grid.WorldToCell(cameraTransform.position);

        tileL = Resources.FindObjectsOfTypeAll<Tile>();
        // foreach (Tile t in tileL) Debug.Log(t.name);

        WorldGenerator.e_worldUpdated.AddListener(UpdateTilemap);
        WorldGenerator.e_worldUpdated.AddListener(ClearAllTiles);
        WorldGenerator.e_worldUpdated.AddListener(UpdateWorldData);
    }
    void UpdateTilemap()
    {
        Vector3Int playerPosition = grid.WorldToCell(cameraTransform.position);
        if (Vector3Int.Distance(playerPosition, generatedPosition) > REGENERATE_DIST)
        {
            tilemapSolid.ClearAllTiles();
            generatedPosition = playerPosition;
        }
        // Iterate through tiles around player
        for (int x = playerPosition.x - (RENDER_DIST_X / 2); x <= playerPosition.x + (RENDER_DIST_X / 2); x++)
        {
            for (int y = playerPosition.y - (RENDER_DIST_Y / 2); y <= playerPosition.y + (RENDER_DIST_Y / 2); y++)
            {
                if (tilemapSolid.GetTile(new Vector3Int(x, y, 0)) == null && WorldGenerator.IsValidCell(x, y))
                {
                    UpdateTile(x, y);
                }
            }
        }
    }

    void Update()
    {
        UpdateTilemap();
    }
    public void UpdateTile(int x, int y)
    {
        worldData = WorldGenerator.GetWorldData();
        int tileId = worldData[y, x];
        switch (tileId) {
            case 0:
                tilemapSolid.SetTile(new Vector3Int(x, y, 0), null);
                break;
            case 1:
                tilemapSolid.SetTile(new Vector3Int(x, y, 0), tileL[1]);
                break;
        }
    }
    void ClearAllTiles() {tilemapSolid.ClearAllTiles();}
    void UpdateWorldData()
    {
        worldData = WorldGenerator.GetWorldData();
    }
}
