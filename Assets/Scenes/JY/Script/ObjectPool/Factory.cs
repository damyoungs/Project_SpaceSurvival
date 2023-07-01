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
    Dagger,
    Pistol,
    Rifle,
    ShotGun,
    SwordLaser,
    SwordLaser_Advanced,
    Shield,
    Shield_Extended

}

public class Factory : MonoBehaviour
{
    private static Factory instance;
    public static Factory I { get { if (instance == null) Init(); return instance; } }


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

    }
    public GameObject GetObject(Pool_Object_Type type)
    {
        GameObject result;
        switch (type)
        {
 

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
