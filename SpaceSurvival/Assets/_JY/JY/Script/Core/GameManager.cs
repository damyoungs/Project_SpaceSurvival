using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Inst { get { Init(); return instance; } }

    public PlayerDummy player;
    public Inventory inventory;
    public SlotManager slotManager;
    public ItemSpawner itemSpawner;
    public Item_Enhancer enhancer;

    ItemDataManager itemDataManager;
    
    public static ItemDataManager Itemdata { get { return Inst.itemDataManager; } }
    public static Inventory Inventory { get { return Inst.inventory; } }
    public static SlotManager SlotManager { get { return Inst.slotManager; } }
    public static ItemSpawner Item_Spawner { get { return Inst.itemSpawner; } }
    public static PlayerDummy playerDummy { get { return Inst.player; } }
    public static Item_Enhancer Item_Enhancer { get { return Inst.enhancer; } }

    private void Awake()
    {
        Init();
        itemDataManager = GetComponent<ItemDataManager>();

    }
    static void Init()
    {
        
        if (instance == null)
        {
            GameObject go = GameObject.Find("@GameManager");
            if (go == null)
            {
                go = new GameObject { name = $"@GameManager" };
                go.AddComponent<GameManager>();
            }
            DontDestroyOnLoad(go);
            instance = go.GetComponent<GameManager>();
    
        }
    }
}
