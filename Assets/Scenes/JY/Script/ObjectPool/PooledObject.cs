using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
    public Action onDie;
    protected virtual void OnEnable()
    {
        transform.localPosition= Vector3.zero;
        transform.localRotation= Quaternion.identity;
    }
    protected virtual void OnDisable()
    {
        onDie?.Invoke();
       
    }
    IEnumerator LifeOver(float delay = 0.0f)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}
