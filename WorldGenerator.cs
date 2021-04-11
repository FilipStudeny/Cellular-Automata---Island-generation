using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public int mapSize;
    public int numberOfSmoothSteps;
    public string worldSeed;
    public bool useRandomSeed;

    [Range(0, 100)]
    public int percentOfMapIsLand;

    public Tile tile;
    public List<Tile> tiles;
    public Tile[,] mapTiles;

    int[,] mappedMap;

    // Start is called before the first frame update
    void Start()
    {
        RandomlyFillMap();
        SmoothMap();
        GenerateStartingGrid();
        AssignTileNeighbors();
    }

    void RandomlyFillMap() 
    {
        mappedMap = new int[mapSize, mapSize];
        
        if (useRandomSeed) { worldSeed = Time.time.ToString(); }

        System.Random randomNumber = new System.Random(worldSeed.GetHashCode());

        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                mappedMap[x,y] = (randomNumber.Next(0, 100) < percentOfMapIsLand)? 0 : 1;
            }
        }
    }

    void SmoothMap() 
    {
        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);

                if (neighbourWallTiles > 4)
                    mappedMap[x, y] = 1;
                else if (neighbourWallTiles < 4)
                    mappedMap[x, y] = 0;

            }
        }
    }

    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < mapSize && neighbourY >= 0 && neighbourY < mapSize)
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        wallCount += mappedMap[neighbourX, neighbourY];
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }

        return wallCount;
    }


    void GenerateStartingGrid()
    {
        mapTiles = new Tile[mapSize, mapSize];

        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                    
                Vector3 tilePosition = new Vector3(-mapSize / 2 + x, 0, -mapSize / 2 + y);
                Tile newTile = Instantiate(tile, tilePosition, Quaternion.identity);

                newTile.transform.SetParent(transform, false);
                newTile.GridPosition = new Vector2Int(x, y);

                if(mappedMap[x,y] == 0) { newTile.IsLand = true; } else { newTile.IsLand = false; }
                
                tiles.Add(newTile);
                mapTiles[x, y] = newTile;              
            }
        }
    }
    
    void AssignTileNeighbors()
    {
        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                Tile tile = mapTiles[x, y];

                if(y < mapSize - 1) 
                {
                    tile.neighbor_UP = mapTiles[x, y + 1];
                }
                if (y > 0)
                {
                    tile.neighbor_DOWN = mapTiles[x, y - 1];
                }
                if (x > 0)
                {
                    tile.neighbor_LEFT = mapTiles[x - 1, y];
                }
                if (x < mapSize - 1)
                {
                    tile.neighbor_RIGHT = mapTiles[x + 1, y];
                }
                
            }
        }
    }
}
