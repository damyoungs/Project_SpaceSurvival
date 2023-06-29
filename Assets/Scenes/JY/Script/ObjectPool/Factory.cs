using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum Pool_Object_Type
{
    Cash,
    HpPotion,
    MpPotion,
    TwoHandSword,
    Wand,
    Bow,
    TwoHandAxe,
    Bat,
    Dagger

}

public class Factory : MonoBehaviour
{
    private static Factory instance;
    public static Factory I { get { if (instance == null) Init(); return instance; } }

    HpPotionPool hpPotionpool;
    MpPotionPool mpPotionPool;
    BatPool batPool;
    BowPool bowPool;
    CashPool cashPool;
    DaggerPool daggerPool;
    TwoHandSwordPool twoHandSwordPool;
    TwoHandAxePool twoHandAxePool;
    WandPool wandPool;
    static void Init()
    {
        GameObject go = GameObject.Find("Factory");
        if (go == null)
        {
            go = new GameObject { name = "Factory" };
            go.AddComponent<Factory>();
        }
        DontDestroyOnLoad(go);
        instance = go.GetComponent<Factory>();

    }
    private void Start()
    {       
        hpPotionpool = transform.GetChild(0).GetComponent<HpPotionPool>();
        mpPotionPool = transform.GetChild(1).GetComponent<MpPotionPool>();
        cashPool = transform.GetChild(2).GetComponent<CashPool>();
        twoHandSwordPool = transform.GetChild(3).GetComponent<TwoHandSwordPool>();
        bowPool = transform.GetChild(4).GetComponent<BowPool>();
        wandPool = transform.GetChild(5).GetComponent<WandPool>();
        daggerPool = transform.GetChild(6).GetComponent<DaggerPool>();
        twoHandAxePool = transform.GetChild(7).GetComponent<TwoHandAxePool>();
        batPool = transform.GetChild(8).GetComponent<BatPool>();



        hpPotionpool.Initialize();
        mpPotionPool.Initialize();
        cashPool.Initialize();
        twoHandSwordPool.Initialize();
        bowPool.Initialize();
        wandPool.Initialize();
        daggerPool.Initialize();
        twoHandSwordPool.Initialize();
        twoHandAxePool.Initialize();
        batPool.Initialize();
    }
    public GameObject GetObject(Pool_Object_Type type)
    {
        GameObject result;
        switch (type)
        {
            case Pool_Object_Type.Cash:
                result = cashPool?.GetObject()?.gameObject;
                break;
            case Pool_Object_Type.HpPotion:
                result = hpPotionpool?.GetObject()?.gameObject;
                break;
            case Pool_Object_Type.MpPotion:
                result = mpPotionPool?.GetObject()?.gameObject;
                break;
            case Pool_Object_Type.TwoHandSword:
                result = twoHandSwordPool?.GetObject()?.gameObject;
                break;
            case Pool_Object_Type.Wand:
                result = wandPool?.GetObject()?.gameObject;
                break;
            case Pool_Object_Type.Bow:
                result = bowPool?.GetObject()?.gameObject;
                break;
            case Pool_Object_Type.TwoHandAxe:
                result = twoHandAxePool?.GetObject()?.gameObject;
                break;
            case Pool_Object_Type.Bat:
                result = batPool?.GetObject()?.gameObject;
                break;
            case Pool_Object_Type.Dagger:
                result = daggerPool?.GetObject()?.gameObject;
                break;

            default:
                result = new GameObject();
                break;
        }
      
        return result;     
    }


    public GameObject GetObject(Pool_Object_Type type,Vector3 position) //오버로딩 함수 꺼내오면서 위치와 각도를 설성하는 함수
    {
        GameObject obj = GetObject(type);
        obj.transform.position = position;
        return obj;
    }

}
