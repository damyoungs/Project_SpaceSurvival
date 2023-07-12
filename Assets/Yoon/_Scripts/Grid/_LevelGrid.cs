using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _LevelGrid : MonoBehaviour
{
    public static _LevelGrid Instance { get; private set; }


    [SerializeField] private Transform gridDebugObjectPrefab;

    private _GridSystem gridSystem;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one UnitActionSystem" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        gridSystem = new _GridSystem(10, 10, 2f);
        gridSystem.CreateDebugObejects(gridDebugObjectPrefab);
    }

    public void AddUnitAtGridPosition(_GridPosition gridPosition, Unit unit)
    {
        _GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.AddUnit(unit);
    }

    public List<Unit> GetUnitListAtGridPosition(_GridPosition gridPosition)
    {
        _GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnitList();
    }

    public void RemoveUnitAtGridPosition(_GridPosition gridPosition, Unit unit)
    {
        _GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.RemoveUnit(unit);
    }

    public void UnitMovedGridPosition(Unit unit, _GridPosition fromGridPosition, _GridPosition toGridPosition)
    {
        RemoveUnitAtGridPosition(fromGridPosition, unit);

        AddUnitAtGridPosition(toGridPosition, unit);
    }
    public _GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);
    
}
