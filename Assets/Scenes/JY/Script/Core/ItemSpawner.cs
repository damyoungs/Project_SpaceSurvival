using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemSpawner : TestBase
{
    List<GameObject> spawnedObjects = new List<GameObject>();
    Player player;
    protected override void Test1(InputAction.CallbackContext _)
    {
        GameObject go = Factory.I.GetObject(Pool_Object_Type.HpPotion);
        spawnedObjects.Add(go);
    }
    protected override void Test2(InputAction.CallbackContext context)
    {
        while(spawnedObjects.Count > 0)
        {
            GameObject del = spawnedObjects[0];
            spawnedObjects.RemoveAt(0);
            del.SetActive(false);
        }
        spawnedObjects.Clear();
    }
    protected override void Test3(InputAction.CallbackContext context)
    {
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }
        player.Die();
    }
}
