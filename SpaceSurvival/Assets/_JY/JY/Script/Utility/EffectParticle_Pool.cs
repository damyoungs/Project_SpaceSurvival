using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectParticle_Pool : MonoBehaviour
{
    [System.Serializable]
    public class PoolPrefab
    {
        public GameObject prefab;
        [Range(1, 100)]
        public int poolSize;
    }

    public PoolPrefab[] PoolPrefabs;
    Queue<GameObject>[] pools;
    GameObject[] parents;
    private void Start()
    {
        Init();
    }
    void Init()
    {
        pools = new Queue<GameObject>[PoolPrefabs.Length];
        parents = new GameObject[PoolPrefabs.Length];

        for (int  i = 0; i < PoolPrefabs.Length; i++)
        {
            GameObject prefab = PoolPrefabs[i].prefab;
            Transform parent = new GameObject($"{PoolPrefabs[i].prefab.name}_Pool").transform;
            parents[i] = parent.gameObject;
            parent.transform.SetParent(this.transform);
            pools[i] = new Queue<GameObject>(PoolPrefabs[i].poolSize);
            for (int j = 0; j < PoolPrefabs[i].poolSize + 1; j++)
            {
                GameObject obj = Instantiate(prefab, parent);
                obj.name = $"{prefab.name}_{j}";
                obj.AddComponent<Pooled_Obj>();
                obj.SetActive(false);
                Pooled_Obj poolObj = obj.GetComponent<Pooled_Obj>();
                poolObj.on_ReturnPool += ReturnPool;
                poolObj.poolIndex = i;
              
                pools[i].Enqueue(obj);
            }
        }

    }
    public GameObject GetObject(SkillType type, Vector3 position)
    {
        GameObject result = pools[(int)type].Dequeue();
        position.y += 2;
        result.transform.position = position;
        result.SetActive(true);
        if (type == SkillType.Blessing)
        {
            result.transform.SetParent(GameManager.Player_.transform);
        }


        return null;
    }
    void ReturnPool(Pooled_Obj obj)
    {
        Queue<GameObject> queue = pools[obj.poolIndex];
        queue.Enqueue(obj.gameObject);

        if (obj.poolIndex == 3)
        {
            StartCoroutine(SetParent(obj));
        }
    }
    IEnumerator SetParent(Pooled_Obj obj)
    {
        yield return null;
        obj.transform.SetParent(parents[3].transform);
    }
    void GenerateObject()
    {

    }
}
