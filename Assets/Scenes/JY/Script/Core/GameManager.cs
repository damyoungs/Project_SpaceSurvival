using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Inst { get { Init(); return instance; } }

    public Inventory inventory;
    public UIobjectSpawner ui_Spawner;
    public ItemSpawner itemSpawner;

    

    public static Inventory Inventory { get { return Inst.inventory; } }
    public static UIobjectSpawner UI_Spawner { get { return Inst.ui_Spawner; } }
    public static ItemSpawner Item_Spawner { get { return Inst.itemSpawner; } }


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
