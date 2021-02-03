using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class WorldGenerator
{
    public static int worldWidth { get { return 4200; } }
    public static int worldHeight { get { return 1200; } }
    public static float seed;

    // NS = noise scale
    static float dirtNS = 200f;
    static float stoneNS = 50f;
    static float cavesNS = 100f;

    // LR = layer roughness
    static int dirtLR = 10;
    static int stoneLR = 30;
    static int stoneLayerOffset = -10;
    static int groundLevel;
    static int[,] worldData;
    static public UnityEvent e_worldUpdated = new UnityEvent();
    public static void GenerateWorld(float seed)
    {
        WorldGenerator.seed = seed;
        groundLevel = 1200 / 2;
        worldData = new int[worldHeight, worldWidth];

        PassInitialDirt();
        PassStone();
        PassCaves();
        PassGrass();

        e_worldUpdated.Invoke();
    }
    static void PassInitialDirt()
    {
        for (int x = 0; x < worldWidth; x++)
        {
            float sample = Mathf.PerlinNoise(((float)x) / worldWidth * dirtNS + seed, 0);
            // Debug.Log((float)x / worldWidth * noiseScale + seed);
            int changeInGroundLevel = Mathf.RoundToInt((sample * dirtLR) - dirtLR / 2);
            // Debug.Log(changeInGroundLevel);
            for (int y = 0; y < groundLevel + changeInGroundLevel; y++)
            {
                worldData[y, x] = 1;
            }
        }
    }

    // Turns dirt blocks underground into stone blocks
    static void PassStone()
    {
        for (int x = 0; x < worldWidth; x++)
        {
            float sample = Mathf.PerlinNoise(((float)x) / worldWidth * stoneNS + seed + 123, 0);
            // Debug.Log((float)x / worldWidth * noiseScale + seed);
            int changeInGroundLevel = Mathf.RoundToInt((sample * stoneLR) - stoneLR / 2);

            for (int y = 0; y < groundLevel + changeInGroundLevel + stoneLayerOffset; y++)
            {
                worldData[y, x] = 3;
            }
        }
    }

    static void PassCaves()
    {
        for (int x = 10; x < worldWidth - 10; x++)
        {
            for (int y = 10; y < groundLevel + stoneLayerOffset; y++)
            {
                float sample = Mathf.PerlinNoise(((float)x) / worldWidth * cavesNS + seed + 321, (((float)y) / worldWidth * cavesNS + seed + 321) * 2);
                if (y > groundLevel)
                {
                    if (sample <= 0.2f) SetCell(x, y, 0);
                }
                else
                {
                    if (sample <= 0.3f) SetCell(x, y, 0);
                }
            }
        }
    }

    // Turns dirt blocks with 5 empty spaces above them into dirt blocks
    static void PassGrass()
    {
        for (int x = 0; x < worldWidth; x++)
        {
            for (int y = 0; y < worldHeight; y++)
            {
                if (GetCell(x, y) == 1)
                {
                    bool isGrass = true;
                    for (int h = 1; h <= 5; h++)
                    {
                        if (GetCell(x, y + h) != 0)
                        {
                            isGrass = false;
                            break;
                        }
                    }
                    if (isGrass) SetCell(x, y, 2);
                }
            }
        }
    }
    public static int[,] GetWorldData() { return worldData; }
    public static void DestroyTile(int x, int y)
    {
        worldData[y, x] = 0;
    }
    public static bool IsValidCell(int x, int y)
    {
        if (x >= 0 && x < worldWidth && y >= 0 && y < worldHeight) return true;
        return false;
    }
    public static int GetCell(int x, int y) { return worldData[y, x]; }
    public static void SetCell(int x, int y, int tileId) { worldData[y, x] = tileId; }
}
