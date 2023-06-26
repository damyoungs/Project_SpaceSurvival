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

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int poolSize;
    }

    public Pool[] pools;
    Dictionary<string, Queue<GameObject>> pooledObject = new Dictionary<string, Queue<GameObject>>();

    private void Start()
    {
        for (int i = 0; i < pools.Length; i++)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            GameObject root = new GameObject($"{pools[i].prefab.name}_Pool");
            root.transform.SetParent(transform);
            for (int j = 0; j < pools[i].poolSize; j++)
            {
                GameObject obj = Instantiate(pools[i].prefab, root.transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            pooledObject.Add(pools[i].tag, objectPool);
        }
    }
    public GameObject GetObject(string tag, Vector3 position, Quaternion rotation)
    {
        if (!pooledObject.ContainsKey(tag)) return null;

        Queue<GameObject> pool = pooledObject[tag];
        
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            if (!obj.activeInHierarchy)
            {
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                return obj;
            }
        }    
        return null;
    }
    public void ReturnToPool(GameObject obj, string tag)
    {
        if (!pooledObject.ContainsKey(tag)) return;

        obj.SetActive(false);
        pooledObject[tag].Enqueue(obj);
    }
}
