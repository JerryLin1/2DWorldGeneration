using System.Collections;
using System.Collections.Generic;

public static class WorldGenerator
{
    public static int worldWidth { get { return 4200; } }
    public static int worldHeight { get { return 1200; } }
    static int groundLevel;
    static int[,] worldData;
    public static void GenerateWorld()
    {
        groundLevel = 1200 / 2;

        worldData = new int[worldHeight, worldWidth];
        for (int x = 0; x < worldWidth; x++)
        {
            for (int y = 0; y < groundLevel; y++)
            {
                worldData[y, x] = 1;
            }
        }
    }
    public static int[,] GetWorldData() { return worldData; }

}
