using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cash : ItemBase
{
    protected override void Initailize()
    {
        ItemType = ItemType.Etc;
    }
    private void OnEnable()
    {
        StartCoroutine(LifeOver(5.0f));
    }
}
