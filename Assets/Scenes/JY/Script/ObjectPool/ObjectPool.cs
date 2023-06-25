using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private static ObjectPool instance;
    public static ObjectPool Inst { get { Init(); return instance; } }
    static void Init()
    {
        if (instance == null)
        {
            GameObject go = GameObject.Find("ObjectPool");
            if (go == null)
            {
                go = new GameObject { name = "ObjectPool" };
                go.AddComponent<ObjectPool>();
            }
            DontDestroyOnLoad(go);
            instance = go.GetComponent<ObjectPool>();
        }
    }
    public class Pool
    {
        public GameObject prefab;
        public int poolSize;
    }

    Pool[] pools;
    Dictionary<int, Queue<GameObject>> pooledObject = new Dictionary<int, Queue<GameObject>>();

}
