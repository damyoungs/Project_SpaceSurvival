using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cash : ItemBase
{
    protected override void Initailize()
    {
        ItemType = ItemType.Etc;
        prefabName = ObjectPool.Pool.PrefabName.Cash;
    }
    private void OnEnable()
    {
        StartCoroutine(LifeOver(5.0f));
    }
}
