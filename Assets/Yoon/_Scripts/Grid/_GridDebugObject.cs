using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class _GridDebugObject : MonoBehaviour
{

    [SerializeField] private TextMeshPro textMeshPro;

    private _GridObject gridObject;

    public void SetGridObject(_GridObject gridObject)
    {
        this.gridObject = gridObject;
    }

    private void Update()
    {
        textMeshPro.text = gridObject.ToString();
    }
}
