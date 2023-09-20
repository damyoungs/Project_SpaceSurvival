using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    EffectParticle_Pool particlePool;

    Vector2 cursorHotspot;
    
    public static ItemDataManager Itemdata => Inst.itemDataManager;
    public static Inventory Inventory => Inst.inventory;
    public static SlotManager SlotManager => Inst.slotManager;
    public static ItemSpawner Item_Spawner => Inst.itemSpawner;
    public static Player_ Player_ => Inst.player;
    public static Item_Enhancer Enhancer => Inst.enhancer;
    public static Item_Mixer Mixer => Inst.mixer;
    public static Item_Mixing_Table Mixing_Table => Inst.mixingTable;
    public static QuickSlot_Manager QuickSlot_Manager => Inst.quickSlot_Box;
    public static EquipBox EquipBox => Inst.equipBox; 
    public static EffectParticle_Pool PS_Pool => Inst.particlePool;

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
        particlePool = GetComponentInChildren<EffectParticle_Pool>();
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
    
    public void ChangeCursor(bool duringSkill)
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
