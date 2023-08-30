using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Inst { get { Init(); return instance; } }

    PlayerDummy player;
    Inventory inventory;
    SlotManager slotManager;
    ItemSpawner itemSpawner;
    Item_Enhancer enhancer;
    Item_Mixer mixer;
    Item_Mixing_Table mixingTable;
    QuickSlot_Manager quickSlot_Box;
    ItemDataManager itemDataManager;
    EquipBox equipBox;
    
    public static ItemDataManager Itemdata { get { return Inst.itemDataManager; } }
    public static Inventory Inventory { get { return Inst.inventory; } }
    public static SlotManager SlotManager { get { return Inst.slotManager; } }
    public static ItemSpawner Item_Spawner { get { return Inst.itemSpawner; } }
    public static PlayerDummy playerDummy { get { return Inst.player; } }
    public static Item_Enhancer Enhancer { get { return Inst.enhancer; } }
    public static Item_Mixer Mixer { get { return Inst.mixer; } }
    public static Item_Mixing_Table Mixing_Table { get { return Inst.mixingTable; } }
    public static QuickSlot_Manager QuickSlot_Manager { get { return Inst.quickSlot_Box; } }
    public static EquipBox EquipBox { get { return Inst.equipBox; } }


    private void Awake()
    {
        Init();
        itemDataManager = GetComponent<ItemDataManager>();
        player = FindObjectOfType<PlayerDummy>();
        inventory = FindObjectOfType<Inventory>();
        slotManager = FindObjectOfType<SlotManager>();
        itemSpawner = FindObjectOfType<ItemSpawner>();
        enhancer = FindObjectOfType<Item_Enhancer>();
        mixer = FindObjectOfType<Item_Mixer>();
        mixingTable = FindObjectOfType<Item_Mixing_Table>();
        quickSlot_Box = FindObjectOfType<QuickSlot_Manager>();
        equipBox = FindObjectOfType<EquipBox>();
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
