using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class WorldGenerator
{
    public static int worldWidth { get { return 4200; } }
    public static int worldHeight { get { return 1200; } }
    public static float seed;
    static float noiseScale = 1000f;
    static int roughness = 10;
    static int groundLevel;
    static int[,] worldData;
    static public UnityEvent e_worldGenerated = new UnityEvent();
    public static void GenerateWorld(float seed)
    {
        WorldGenerator.seed = seed;
        groundLevel = 1200 / 2;
        worldData = new int[worldHeight, worldWidth];
        PassDirt();
        e_worldGenerated.Invoke();
    }
    static void PassDirt()
    {
        for (int x = 0; x < worldWidth; x++)
        {
            float sample = Mathf.PerlinNoise(((float)x) / worldWidth * noiseScale + seed, 0);
            // Debug.Log((float)x / worldWidth * noiseScale + seed);
            int changeInGroundLevel = Mathf.RoundToInt((sample * roughness) - roughness / 2);
            // Debug.Log(changeInGroundLevel);
            for (int y = 0; y < groundLevel + changeInGroundLevel; y++)
            {
                worldData[y, x] = 1;
            }
        }
    }
    public static int[,] GetWorldData() { return worldData; }

}
