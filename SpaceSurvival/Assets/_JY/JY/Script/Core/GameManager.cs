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
    public Item_Mixer mixer;
    public Item_Mixing_Table mixingTable;
    public QuickSlots quickSlot_Box;

    ItemDataManager itemDataManager;
    
    public static ItemDataManager Itemdata { get { return Inst.itemDataManager; } }
    public static Inventory Inventory { get { return Inst.inventory; } }
    public static SlotManager SlotManager { get { return Inst.slotManager; } }
    public static ItemSpawner Item_Spawner { get { return Inst.itemSpawner; } }
    public static PlayerDummy playerDummy { get { return Inst.player; } }
    public static Item_Enhancer Enhancer { get { return Inst.enhancer; } }
    public static Item_Mixer Mixer { get { return Inst.mixer; } }
    public static Item_Mixing_Table Mixing_Table { get { return Inst.mixingTable; } }
    public static QuickSlots QuickSlot_Box { get { return Inst.quickSlot_Box; } }


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
