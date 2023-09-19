using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Inst { get { Init(); return instance; } }

    bool isBattle = false;
    public bool IsBattle
    {
        get => isBattle;
        set
        {
            if (isBattle != value)
            {
                isBattle = value;
                switch (isBattle)
                {
                    case true:
                        
                        break;
                    case false:
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public Texture2D normalCursor;
    public Texture2D skillCursor;

    Player_ player;
    Inventory inventory;
    SlotManager slotManager;
    ItemSpawner itemSpawner;
    Item_Enhancer enhancer;
    Item_Mixer mixer;
    Item_Mixing_Table mixingTable;
    QuickSlot_Manager quickSlot_Box;
    ItemDataManager itemDataManager;
    EquipBox equipBox;

    Vector2 cursorHotspot;
    
    public static ItemDataManager Itemdata { get { return Inst.itemDataManager; } }
    public static Inventory Inventory { get { return Inst.inventory; } }
    public static SlotManager SlotManager { get { return Inst.slotManager; } }
    public static ItemSpawner Item_Spawner { get { return Inst.itemSpawner; } }
    public static Player_ Player_ { get { return Inst.player; } }
    public static Item_Enhancer Enhancer { get { return Inst.enhancer; } }
    public static Item_Mixer Mixer { get { return Inst.mixer; } }
    public static Item_Mixing_Table Mixing_Table { get { return Inst.mixingTable; } }
    public static QuickSlot_Manager QuickSlot_Manager { get { return Inst.quickSlot_Box; } }
    public static EquipBox EquipBox { get { return Inst.equipBox; } }


    private void Awake()
    {
        Init();
        itemDataManager = GetComponent<ItemDataManager>();
        inventory = FindObjectOfType<Inventory>();
        slotManager = FindObjectOfType<SlotManager>();
        itemSpawner = FindObjectOfType<ItemSpawner>();
        enhancer = FindObjectOfType<Item_Enhancer>();
        mixer = FindObjectOfType<Item_Mixer>();
        mixingTable = FindObjectOfType<Item_Mixing_Table>();
        quickSlot_Box = FindObjectOfType<QuickSlot_Manager>();
        equipBox = FindObjectOfType<EquipBox>();
    }
    private void Start()
    {
        player = FindObjectOfType<Player_>(true);
        player.on_CursorChange += ChangeCursor;
        ChangeCursor(false);
        cursorHotspot = new Vector2(skillCursor.width * 0.5f, skillCursor.height * 0.5f);
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
    
    void ChangeCursor(bool duringSkill)
    {

        if (duringSkill)
        {
            Cursor.SetCursor(skillCursor, cursorHotspot, CursorMode.ForceSoftware);
        }
        else
        {
            Cursor.SetCursor(normalCursor, Vector2.zero, CursorMode.ForceSoftware);
        }
    }
}
