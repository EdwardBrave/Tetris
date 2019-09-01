using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Figure : MonoBehaviour
{
    public Vector2Int axisCords;
    public int figureType = 0;
    public MapGrid grid;
    public GameObject cube;
    public List<GameObject> cubes = new List<GameObject>();
    public GameObject[,] cubesMap = new GameObject[,]
    {
        {null, null, null},
        {null, null, null},
        {null, null, null}
    };
    
    public long spawnCounter;
    public long removeCounter;

    public void SetColor(Color32 color)
    {
        foreach (var current in cubes)
        {
            var materialGen = current.GetComponent<MaterialGen>();
            materialGen.givenColor = color;
        }
    }
    
    public void SetRandomColor(MaterialGen.Pallet[] exclude)
    {
        Color32 randomColor = MaterialGen.RandomColor(exclude);
        foreach (var current in cubes)
        {
            var materialGen = current.GetComponent<MaterialGen>();
            materialGen.givenColor = randomColor;
        }
    }

    public void SetRandomType()
    {
        figureType = Random.Range(0, FigureMaps.Length);
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    };
    
    public static readonly byte[][,] FigureMaps = new []
    {
        new byte[,]
        {
            {0,1,0},
            {0,1,0},
            {0,1,0}
        },
        new byte[,]
        {
            {1,1,0},
            {0,1,0},
            {0,0,0}
        },
        new byte[,]
        {
            {0,1,0},
            {1,1,1},
            {0,0,0}
        },
        new byte[,]
        {
            {1,1,0},
            {0,1,0},
            {0,1,0}
        },
        new byte[,]
        {
            {0,1,1},
            {0,1,0},
            {0,1,0}
        },
        new byte[,]
        {
            {0,1,0},
            {0,1,0},
            {0,0,0}
        }
    };

    public bool Generate(Vector2Int cords, MapGrid mapGrid = null, int type = -1)
    {
        bool isSuccessful = true;
        grid = (mapGrid == null) ? grid : mapGrid;
        figureType = (type == -1) ? figureType : type;
        if (!grid)
            return false;
        
        var chosenMap = FigureMaps[figureType];
        for (int y = chosenMap.GetLength(0)-1; y >= 0; y--)
        {
            for (int x = chosenMap.GetLength(1)-1; x >= 0; x--)
            {
                if (chosenMap[y, x] == 1)
                {
                    GameObject current = Instantiate(cube, transform);
                    if (!grid.Add(current, new Vector2Int(cords.x + x, cords.y + y)))
                        isSuccessful = false;
                    cubes.Add(current);
                    cubesMap[y,x] = current;
                }
            }
        }
        axisCords = cords;
        return isSuccessful;
    }
    
    public bool MoveTo(Direction direction)
    {
        Vector2Int delta = Vector2Int.zero;
        Vector2Int side = new Vector2Int(0,0);
        switch (direction)
        {
            case Direction.Up :
                delta.y--;
                side.y = cubesMap.GetLength(0) - 1;
                break;
            case Direction.Left :
                delta.x--;
                side.x = cubesMap.GetLength(1) - 1;
                break;
            case Direction.Right :
                delta.x++;
                break;
            case Direction.Down :
                delta.y++;
                break;
        }

        int y = - 1, dx, dy;
        for (y = cubesMap.GetLength(0) - 1; y >= 0; y--)
        {
            for (int x = cubesMap.GetLength(1) - 1; x >= 0; x--)
            {
                dx = Math.Abs(side.x - x);
                dy = Math.Abs(side.y - y);
                GameObject current = cubesMap[dy, dx];
                if (current && !grid.Move(current, new Vector2Int(dx, dy) + axisCords + delta))
                    goto multiBreak;
            }
        }
        multiBreak:
        if (y > -1)
        {
            for (; y < cubesMap.GetLength(0); y++)
            {
                for (int x = 0; x < cubesMap.GetLength(1); x++)
                {
                    dx = Math.Abs(side.x - x);
                    dy = Math.Abs(side.y - y);
                    GameObject current = cubesMap[dy, dx];
                    if (current)
                        grid.Move(current, new Vector2Int(dx, dy) + axisCords);
                }
            }
            return false;
        }
        axisCords += delta;
        return true;
    }

    public bool Rotate(bool isToRight = true)
    {
        GameObject[,] rotateMap = cubesMap.Clone() as GameObject[,];
        for (int i = cubes.Count - 1; i >= 0; i--)
            grid.Remove(cubes[i], true);
        int y = -1;
        for (y = rotateMap.GetLength(0) - 1; y >= 0; y--)
        {
            for (int x = rotateMap.GetLength(1) - 1; x >= 0; x--)
            {
                int rx = isToRight ? Math.Abs(y - rotateMap.GetLength(0) + 1) : y,
                    ry = isToRight ? x : Math.Abs(x - rotateMap.GetLength(1) + 1);
                GameObject current = cubesMap[y, x];
                rotateMap[ry, rx] = current;
                if (current && !grid.Add(current, new Vector2Int(rx, ry) + axisCords, true))
                    goto multiBreak;
            }
        }
        multiBreak:
        if (y > -1)
        {
            for (y = cubesMap.GetLength(0) - 1; y >= 0; y--)
            for (int x = cubesMap.GetLength(1) - 1; x >= 0; x--)
                if (cubesMap[y, x])
                    grid.Add(cubesMap[y, x], new Vector2Int(axisCords.x + x, axisCords.y + y), true);
            return false;
        }
        cubesMap = rotateMap;
        return true;
    }
}
