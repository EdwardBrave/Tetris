using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGrid : MonoBehaviour
{
    // Start is called before the first frame update
    [ContextMenuItem("Refresh", "Refresh")]
    public Vector2Int gridSize;
    [ContextMenuItem("Refresh", "Refresh")]
    public Vector2 deltaSpace;
    public GameObject cell;

    public long spawnCounter;
    public long removeCounter;

    private List<List<GameObject>> _gridData;

    public List<List<GameObject>> Data
    {
        get
        {
            if (_gridData == null)
            {
                _gridData = new List<List<GameObject>>();
                for (int n = (int) gridSize.y - 1; n >= 0; n--)
                {
                    _gridData.Add(new List<GameObject>());
                    for (int m = (int) gridSize.x - 1; m >= 0; m--)
                    {
                        _gridData[_gridData.Count-1].Add(null);
                    }
                }
            }

            return _gridData;
        }
    }

    public List<GameObject> _cells;
    void Refresh()
    {
        _cells = _cells ?? new List<GameObject>();
        for (int i = _cells.Count-1; i >= 0; i--)
        {
            DestroyImmediate(_cells[i]);
            _cells.RemoveAt(i);
        }
        
        for (int ySize = 0; ySize < gridSize.y ; ySize++) 
        {
            for (int xSize = 0; xSize < gridSize.x ; xSize++)
            {
                GameObject newCell = Instantiate(cell, transform);
                _cells.Add(newCell);
                newCell.transform.localPosition = new Vector3(
                    transform.position.x + deltaSpace.x * xSize, 
                    0, 
                    transform.position.z + deltaSpace.y * ySize);
            } 
        } 
    }

    public void Start()
    {
        spawnCounter = removeCounter = 0;
        Refresh();
        Clear();
    }
   
    public bool CheckPermission(Vector2Int position) => 0 <= position.x && position.x < gridSize.x &&
                                                        0 <= position.y && position.y < gridSize.y;

    public bool Add(GameObject obj, Vector2Int position, bool ignore = false)
    {
        if (obj == null || !CheckPermission(position) || Data[(int)position.y][(int) position.x] != null)
            return false;
        RemoveByPos(Find(obj));
        if (!ignore)
            spawnCounter++;
        Data[(int) position.y][(int) position.x] = obj;
        Vector3 selfPos = _cells[(int)(gridSize.x * position.y + position.x)].transform.position;
        obj.transform.position = new Vector3(selfPos.x, selfPos.y, selfPos.z);
        return true;
    }

    public Vector2Int Find(GameObject obj)
    {
        for (int y = gridSize.y - 1; y >= 0; y--) 
        {
            for (int x = gridSize.x - 1; x >= 0; x--) 
            {
                if (Data[y][x] == obj)
                {
                    return new Vector2Int(x,y);
                }
            }
        }
        return Vector2Int.one * -1;
    }

    public bool Move(GameObject obj, Vector2Int position)
    {
        
    
        Vector2Int oldPos = Find(obj);
        if (CheckPermission(position) && Data[(int) position.y][(int) position.x] == null && CheckPermission(oldPos))
        {
            Data[(int) oldPos.y][(int) oldPos.x] = null;
            Data[(int) position.y][(int) position.x] = obj;
            Vector3 selfPos = _cells[(int)(gridSize.x * position.y + position.x)].transform.position;
            obj.transform.position = new Vector3(selfPos.x, selfPos.y, selfPos.z);
            return true;
        }
        return false;
    }

    public GameObject Remove(GameObject obj, bool ignore = false)
    {
        Vector2Int oldPos = Find(obj);
        return RemoveByPos(oldPos, ignore);
    }

    public GameObject RemoveByPos(Vector2Int position, bool ignore = false)
    {
        if (!CheckPermission(position))
            return null;
        GameObject oldObject = Data[(int) position.y][(int) position.x];
        if (!oldObject)
            return null;
        Data[(int) position.y][(int) position.x] = null;
        if (!ignore)
        removeCounter++;
        return oldObject;
    }
    
    public void RemoveRow(int row)
    {
        _removedCubes.Add(new List<GameObject>());
        for (int x = gridSize.x - 1; x >= 0; x--)
        {
            var item = RemoveByPos(new Vector2Int(x, row));
            _removedCubes[_removedCubes.Count-1].Add(item);
            item.AddComponent<Rigidbody>();
            Vector3 delta = item.transform.localPosition;
            delta.z -= 0.6F;
            item.transform.localPosition = delta;
            item.transform.localScale *= 0.95F;
            
        }
        Invoke(nameof(RemoveCubes),3);
        
        for (int y = row-1; y >= 0; y--)
        for (int x = gridSize.x - 1; x >= 0; x--)
            if (Data[y][x] != null)
                Move(Data[y][x], new Vector2Int(x, y + 1));
    }

    private readonly List<List<GameObject>> _removedCubes = new List<List<GameObject>>(); 
    private void RemoveCubes()
    {
        if (_removedCubes.Count <= 0) return;
        foreach (var item in _removedCubes[0])
            Destroy(item);
        _removedCubes.RemoveAt(0);
    }

    public void Clear()
    {
        for (int y = gridSize.y - 1; y >= 0; y--) 
        {
            for (int x = gridSize.x - 1; x >= 0; x--) 
            {
                if (Data[y][x] != null)
                {
                    Destroy(Data[y][x]);
                    Data[y][x] = null;
                }
            }
        }
    }
}
