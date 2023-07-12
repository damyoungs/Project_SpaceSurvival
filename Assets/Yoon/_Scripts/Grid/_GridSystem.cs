using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _GridSystem
{
    private int width;
    private int height;
    private float cellSize;
    private _GridObject[,] gridObjectsArray;

    public _GridSystem(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridObjectsArray = new _GridObject[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                _GridPosition gridPosition = new _GridPosition(x, z);
                gridObjectsArray[x, z] = new _GridObject(this, gridPosition);
            }
        }

    }

    public Vector3 GetWorldPosition(_GridPosition gridPosition)
    {
        return new Vector3(gridPosition.x, 0, gridPosition.z) * cellSize;
    }

    public _GridPosition GetGridPosition(Vector3 worldPosition) 
    {
        return new _GridPosition(
            Mathf.RoundToInt(worldPosition.x / cellSize),
            Mathf.RoundToInt(worldPosition.z / cellSize)
            );
    }

    public void CreateDebugObejects(Transform debugPrefab)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                _GridPosition gridPosition = new _GridPosition(x, z);

               Transform debugTransform = GameObject.Instantiate(debugPrefab,
                   GetWorldPosition(gridPosition), Quaternion.identity);

               _GridDebugObject gridDebugObject =  debugTransform.GetComponent<_GridDebugObject>();
                gridDebugObject.SetGridObject(GetGridObject(gridPosition));
            }
        }
    }

    public _GridObject GetGridObject(_GridPosition gridPosition)
    {
        return gridObjectsArray[gridPosition.x, gridPosition.z];
    }
}
