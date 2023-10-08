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
                poolObj.poolIndex = i;// 비활성화시 다시 풀로 되돌릴때 사용
              
                pools[i].Enqueue(obj);
            }
        }

    }
    /// <summary>
    /// 스킬이펙트용
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
    /// 레벨업 이펙트
    /// </summary>
    /// <param name="position">플레이어의 포지션</param>
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
    /// 데미지 팝업용
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
    void ReturnPool(Pooled_Obj obj)
    {
        Queue<GameObject> queue = pools[obj.poolIndex];
        queue.Enqueue(obj.gameObject);

        if (obj.poolIndex == 3 )//버프스킬일 경우
        {
            StartCoroutine(SetParent(obj));
        }
        else if (obj.poolIndex == 5) //데미지 텍스트일 경우
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
        obj.transform.SetParent(parents[obj.poolIndex].transform);
    }
    void GenerateObject()
    {

    }
}
