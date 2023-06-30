using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewObjectPool : MonoBehaviour
{
    public static NewObjectPool I;

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int amountToPool;
    }

    public List<Pool> pools;
    private Dictionary<int, Queue<GameObject>> pooledObjects = new Dictionary<int, Queue<GameObject>>();
    bool isInitialized = false;
    private void Awake()
    {
        if (I == null)
        {
            I = this;
        }
    }

    void Start()
    {
        for (int i = 0; i < pools.Count; i++)
        {
            CreatePool(i, pools[i].amountToPool);
        }
    }

    public GameObject GetObject(int prefabIndex)
    {
        if (!pooledObjects.ContainsKey(prefabIndex)) return null;

        Queue<GameObject> pool = pooledObjects[prefabIndex];

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
            ExpandPool(prefabIndex);
            return GetObject(prefabIndex);
        }
        return null;
    }

    public void ReturnToPool(GameObject obj, int prefabIndex)
    {
        if (!pooledObjects.ContainsKey(prefabIndex)) return;

        obj.SetActive(false);
        pooledObjects[prefabIndex].Enqueue(obj);
    }

    private void ExpandPool(int prefabIndex)
    {
        Pool pool = pools[prefabIndex];
        Debug.LogWarning($"{pool.prefab.name} 풀사이즈 증가 {pool.amountToPool} -> {pool.amountToPool * 2}");
        CreatePool(prefabIndex, pool.amountToPool);
        pool.amountToPool *= 2;
    }

    private void CreatePool(int prefabIndex, int amountToPool)
    {
        Pool pool = pools[prefabIndex];
        GameObject root;
        if (!isInitialized)
        {
            root = new GameObject($"{pool.prefab.name}_Pool");
            root.transform.SetParent(transform);
            isInitialized = true;
        }
        else
        {
            root = GameObject.Find($"{pool.prefab.name}_Pool");
        }    

        Queue<GameObject> objectPool = new Queue<GameObject>();
        for (int j = 0; j < amountToPool; j++)
        {
            GameObject obj = Instantiate(pool.prefab, root.transform);
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }

        if (pooledObjects.ContainsKey(prefabIndex))
        {
            pooledObjects[prefabIndex] = objectPool;
        }
        else
        {
            pooledObjects.Add(prefabIndex, objectPool);
        }
    }
}

