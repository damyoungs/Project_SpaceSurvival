using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool I;

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int amountToPool;
    }

    public List<Pool> pools;
  //  public Pool[] pool_Index_obj;
    private Dictionary<int, Queue<GameObject>> pooledObjects = new Dictionary<int, Queue<GameObject>>();

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
            Queue<GameObject> objectPool = new Queue<GameObject>();
            GameObject root = new GameObject($"{pools[i].prefab.name}_Pool");
            root.transform.SetParent(transform);
            for (int j = 0; j < pools[i].amountToPool; j++)
            {
                GameObject obj = Instantiate(pools[i].prefab, root.transform);
               
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            pooledObjects.Add(i, objectPool);
        }
    }

    public GameObject GetPooledObject(int prefabIndex)
    {
        if (!pooledObjects.ContainsKey(prefabIndex)) return null;

        Queue<GameObject> pool = pooledObjects[prefabIndex];

        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }
        return null;
    }

    public void ReturnToPool(GameObject obj, int prefabIndex)
    {
        if (!pooledObjects.ContainsKey(prefabIndex)) return;

        obj.SetActive(false);
        pooledObjects[prefabIndex].Enqueue(obj);
    }
}
