using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooled_Obj : MonoBehaviour
{
    public Action<Pooled_Obj> on_ReturnPool;
    ParticleSystem ps;

    public int poolIndex { get; set; }
    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        
    }
    private void OnParticleSystemStopped()
    {
        if (ps != null)
        {
            GameManager.EffectPool.ReturnPool(this);
            
        }
    }


    private void OnDisable()
    {
       // on_ReturnPool?.Invoke(this);
    }
}
