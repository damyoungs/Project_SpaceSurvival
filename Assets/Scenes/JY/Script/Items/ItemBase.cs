using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : PooledObject
{
    public float rotateSpeed = 360.0f;
    protected virtual void Update()
    {

        transform.Rotate(Vector3.up, 360.0f);
    }
    void Rotate()
    {
       
    }
    void moving()
    {

    }
}
