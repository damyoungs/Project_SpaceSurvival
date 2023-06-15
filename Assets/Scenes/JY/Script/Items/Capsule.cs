using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capsule : PooledObject
{
    public float rotateSpeed = 3.0f;
    Vector3 rotateangle = new Vector3(1,1,1);
    Vector3 MyPosition = new Vector3(7, 1, 12);
    private void Start()
    {
        transform.position = MyPosition;
    }
    private void Update()
    {
        transform.Rotate(rotateangle);
    }

}
