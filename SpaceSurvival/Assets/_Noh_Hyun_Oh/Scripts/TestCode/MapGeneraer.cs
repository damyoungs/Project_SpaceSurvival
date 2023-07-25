using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapGeneraer : MonoBehaviour
{
    int count = 100;
    int row = 10;
    int col = 10;
   public GameObject tile;
    private void GeneraterMap() {
        for (int i = 0; i < count; i++)
        {

            GameObject  obj = Instantiate(tile, transform);
            Vector3 ve =  obj.transform.position;
            ve.x = i*row;
            ve.z = i*col;
        }
    }

 
}

