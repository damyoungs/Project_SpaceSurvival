using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private static ItemManager instance;
    public static ItemManager Inst
    {
        get { Init(); return instance; }
    }
    static void Init()
    {
        if (instance == null)
        {
            GameObject go = GameObject.Find("ItemManager");
            if (go == null)
            {
                go = new GameObject { name = "ItemManager" };
                go.AddComponent<ItemManager>();
            }
            DontDestroyOnLoad(go);
            instance = go.GetComponent<ItemManager>();
        }
    }

    public ItemBase itemBase;
    public Inventory inventory;
    public UIobjectSpawner uiSpawner;
    public Slot slot;

}
