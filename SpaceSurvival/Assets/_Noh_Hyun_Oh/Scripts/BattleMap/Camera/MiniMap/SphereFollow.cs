using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereFollow : MonoBehaviour
{
    Transform followTarget;
    [SerializeField]
    Vector3 dir = new Vector3(0.0f,50.0f,0.0f);
    private void Awake()
    {
        followTarget = transform.parent.GetChild(0);
    }
    private void Update()
    {
        transform.position = followTarget.position + dir;
    }
}
