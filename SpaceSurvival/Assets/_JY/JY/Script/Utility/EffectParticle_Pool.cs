using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectParticle_Pool : MonoBehaviour
{
    public GameObject[] prefab;
    Queue<GameObject>[] pools;


    [System.Serializable]
    public class PoolPrefab
    {
        public GameObject prefab;
        [Range(1, 100)]
        public int poolSize;
    }

    public PoolPrefab[] PoolPrefabs;


    void Init()
    {
        pools = new Queue<GameObject>[PoolPrefabs.Length];
        for (int  i = 0; i < PoolPrefabs.Length; i++)
        {
            GameObject[] arr = new GameObject[PoolPrefabs[i].poolSize];
            GameObject prefab = PoolPrefabs[i].prefab;
            Transform parent = new GameObject($"{PoolPrefabs[i].prefab.name}_Pool").transform;
            for (int j = 0; j < PoolPrefabs[i].poolSize; j++)
            {
                GameObject obj = Instantiate(prefab, parent);
                obj.name = $"{prefab.name}_{j}";
                arr[j] = prefab;

            }
           // pools[i] = arr;
        }

    }
    public GameObject GetObject()
    {
        return null;
    }
    void ReturnPool()
    {

    }
    void GenerateObject()
    {

    }
}
