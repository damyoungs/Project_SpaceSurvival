using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Effect_Pool : MonoBehaviour
{
    [System.Serializable]
    public class PoolPrefab
    {
        public GameObject prefab;
        [Range(1, 200)]
        public int poolSize;
    }

    public PoolPrefab[] PoolPrefabs;
    Queue<GameObject>[] pools;
    GameObject[] parents;

    Vector3 Text_Position;
    private void Start()
    {
        Init();
        Text_Position = new Vector3(0, 2, 0);

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
                poolObj.poolIndex = i;// ë¹„í™œì„±í™”ì‹œ ë‹¤ì‹œ í’€ë¡œ ë˜ëŒë¦´ë•Œ ì‚¬ìš©
              
                pools[i].Enqueue(obj);
            }
        }

    }
    /// <summary>
    /// ìŠ¤í‚¬ì´í™íŠ¸ìš©
    /// </summary>
    /// <param name="type"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public GameObject GetObject(SkillType type, Vector3 position)
    {
        GameObject result = pools[(int)type].Dequeue();
        position.y += 1;
        result.transform.position = position;
        result.SetActive(true);
        if (type == SkillType.Blessing)
        {
            result.transform.SetParent(GameManager.Player_.transform);
        }
        return null;
    }
    /// <summary>
    /// ë ˆë²¨ì—… ì´í™íŠ¸
    /// </summary>
    /// <param name="position">í”Œë ˆì´ì–´ì˜ í¬ì§€ì…˜</param>
    /// <returns></returns>
    public GameObject GetLevelUp_Effect(Transform target)
    {
        StartCoroutine(LevelUpTextPopup(target));

        GameObject levelUp_PS = pools[6].Dequeue();
        levelUp_PS.transform.SetParent(target);
        levelUp_PS.transform.position = target.position;
        levelUp_PS.gameObject.SetActive(true);


        return null;
    }

    IEnumerator LevelUpTextPopup(Transform target)
    {
        int i = 0;
        while(i < 4)
        {
            GameObject damageText_Prefab = pools[5].Dequeue();
            DamageText damageText_Comp = damageText_Prefab.GetComponent<DamageText>();
            damageText_Comp.SetText_LevelUp();
            damageText_Prefab.transform.SetParent(target);
            damageText_Prefab.transform.localPosition = Text_Position;
            damageText_Prefab.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            i++;
        }
    }
    /// <summary>
    /// ë°ë¯¸ì§€ íŒì—…ìš©
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="target"></param>
    /// <param name="isCritical"></param>
    /// <returns></returns>
    public GameObject GetObject(float damage, Transform target, bool isCritical)
    {
        GameObject result = pools[5].Dequeue();
        DamageText damageText = result.GetComponent<DamageText>();
        result.transform.SetParent(target);
        result.transform.localPosition = Text_Position;
        damageText.SetText(damage, isCritical);
        result.SetActive(true);

        return null;
    }
    public void PopupMiss(Transform target)
    {
        GameObject result = pools[5].Dequeue();
        DamageText damageText = result.GetComponent<DamageText>();
        result.transform.SetParent(target);
        result.transform.localPosition = Text_Position;
        damageText.SetTextMiss();
        result.SetActive(true);
    }
    public void ReturnPool(Pooled_Obj obj)
    {
        Queue<GameObject> queue = pools[obj.poolIndex];
        queue.Enqueue(obj.gameObject);

        if (obj.poolIndex == 3 )//ë²„í”„ìŠ¤í‚¬ì¼ ê²½ìš°
        {
            StartCoroutine(SetParent(obj));
        }
        else if (obj.poolIndex == 5) //ë°ë¯¸ì§€ í…ìŠ¤íŠ¸ì¼ ê²½ìš°
        {
            StartCoroutine(SetParent(obj));
        }
        else if (obj.poolIndex == 6)
        {
            StartCoroutine(SetParent(obj));
        }
    }
    IEnumerator SetParent(Pooled_Obj obj)
    {
        yield return null;
        if (obj == null)
        {
            Debug.Log($"ÀÌÆåÆ® Ç® ¹ö±× 1: {obj} °¡ ¾ø¾îÁ³½À´Ï´Ù.");
        }
        else 
        {
            if (parents[obj.poolIndex] != null)
            {
                obj.transform.SetParent(parents[obj.poolIndex].transform);
            }
            else 
            {
                Debug.Log($"ÀÌÆåÆ® Ç® ¹ö±× 2: {parents[obj.poolIndex]} °¡ ¾ø¾îÁ³½À´Ï´Ù.");
            }
        }
    }
    void GenerateObject()
    {

    }
}
