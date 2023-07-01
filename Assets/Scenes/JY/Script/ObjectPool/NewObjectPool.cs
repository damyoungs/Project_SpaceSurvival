using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NewObjectPool.Pool;

public class NewObjectPool : MonoBehaviour
{
    public static NewObjectPool I;
    bool isInitialize = false;
    private void Awake()
    {
        I = this;
    }
    [System.Serializable]
    public class Pool
    {
        public enum PrefabName
        {
            hpPotion,
            mpPotion,
        }

        public PrefabName name;
        public GameObject prefab;
        public int amountToPool;
    }
    public List<Pool> pools;
    private Dictionary<Pool.PrefabName, Queue<GameObject>> pooledObjects = new Dictionary<Pool.PrefabName, Queue<GameObject>>();

    void Start()
    {
        foreach (Pool pool in pools)
        {
            CreatePool(pool.name, pool.amountToPool);
        }
    }

    public GameObject GetObject(Pool.PrefabName prefabName)
    {
        if (!pooledObjects.ContainsKey(prefabName)) return null;

        Queue<GameObject> pool = pooledObjects[prefabName];

        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }
        else
        {
            ExpandPool(prefabName);
            return GetObject(prefabName);
        }
        return null;
    }

    public void ReturnToPool(GameObject obj, Pool.PrefabName prefabName)
    {
        if (!pooledObjects.ContainsKey(prefabName)) return;

        obj.SetActive(false);
        pooledObjects[prefabName].Enqueue(obj);
    }

    private void ExpandPool(Pool.PrefabName prefabName)
    {
        Pool pool = pools.Find(p => p.name == prefabName);
        Debug.LogWarning($"풀 사이즈 확장 {prefabName}_Pool {pool.amountToPool} => {pool.amountToPool * 2}");
        CreatePool(prefabName, pool.amountToPool);
        pool.amountToPool *= 2;
    }

    private void CreatePool(Pool.PrefabName prefabName, int amountToPool)
    {
        Pool pool = pools.Find(p => p.name == prefabName);
        GameObject root;
        if (!isInitialize)
        {
            root = new GameObject($"{pool.prefab.name}_Pool");
            root.transform.SetParent(transform);
            isInitialize = true;
        }
        else
        {
            root = GameObject.Find($"{pool.prefab.name}_Pool");
        }
       

        Queue<GameObject> objectPool = new Queue<GameObject>();
        for (int j = 0; j < amountToPool; j++)
        {
            GameObject obj = Instantiate(pool.prefab, root.transform);
            ItemBase itemBase = obj.GetComponent<ItemBase>();
            itemBase.returnPool += ReturnToPool;
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }

        pooledObjects[prefabName] = objectPool;
    }
}
