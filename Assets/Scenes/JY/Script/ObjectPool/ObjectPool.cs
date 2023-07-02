using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static ObjectPool.Pool;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool objectPool;
    private void Awake()
    {
        objectPool = this;
    }

    [System.Serializable]
    public class Pool//기존 풀의 빈오브젝트에 오브젝트 풀 스크립트를  할당하던 것의 기능을 한다.
    {
        //풀에 들어갈 오브젝트 하나당 큐가 하나씩 필요하기 때문에 해당 오브젝트가 어떤큐에 들어갈 것인지 구분하기 위한 열거형
        //또 아이템을 생성할때도 이 열거형을 파라미터로 받는다
        public enum PrefabName
        {
            None,
            Cash,
            HpPotion,
            MpPotion,
            Bat,
            Bow,
            Dagger,
            Pistol,
            Rifle,
            ShotGun,
            SwordLaser,
            SwordLaser_Advanced,
            TwoHandAxe,
            TwoHandSword,
            Wand,
            Shield,
            Shield_Extended,
        }

        public PrefabName name;//열거형 변수선언
        public GameObject prefab;// 
        public int amountToPool;// 각각 풀안에 들어갈 오브젝트의 갯수
    }
    public List<Pool> pools;//풀들의 리스트
    private Dictionary<Pool.PrefabName, Queue<GameObject>> pooledObjects = new Dictionary<Pool.PrefabName, Queue<GameObject>>();// 위 열거형을 키값으로 갖는 GameObject타입의 큐 Dictionary

    void Start()
    {
        foreach (Pool pool in pools) //pools 리스트안의 풀들의 갯수만큼 풀을 만드는 함수 실행
        {
            CreatePool(pool.name, pool.amountToPool);
        }
    }

    public GameObject GetObject(Pool.PrefabName prefabName) //prefabName을 파라미터로 받아서 해당큐에있응 오브젝트를 꺼내어 활성화한다
    {
        if (!pooledObjects.ContainsKey(prefabName)) return null;

        Queue<GameObject> pool = pooledObjects[prefabName]; //파라미터로 받은 prefabName 에 맞는 큐를 불러온다(대입해준다)

        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue(); //큐에서 오브젝트를 꺼낸다.
            obj.SetActive(true);
            return obj;

        }
        else
        {
            ExpandPool(prefabName);
            return GetObject(prefabName);
        }
    }
    public GameObject GetObject(Pool.PrefabName prefabName, Vector3 position)
    {
        GameObject obj =  GetObject(prefabName);
        obj.transform.position = position;
        return obj;
    }
    public void ReturnToPool(GameObject obj, Pool.PrefabName prefabName) //풀에 들어갈 오브젝트의 최상위 클래스에서 만들어진 델리게이트에 연결될 함수
    {
        if (!pooledObjects.ContainsKey(prefabName)) return;//파라미터로 받은 prefabName이 Dictionary 의 Key값에 존재하지 않는다면 return;

        obj.SetActive(false);
        pooledObjects[prefabName].Enqueue(obj); //파라미터로 받은 큐로 리턴
    }

    private void ExpandPool(Pool.PrefabName prefabName)//Queue의 Count가 0보다 작거나 같으면 실행되는 풀 확장 함수
    {
        Pool pool = pools.Find(p => p.name == prefabName);// 확장할 풀 가져오기
        Debug.LogWarning($"풀 사이즈 확장 {prefabName}_Pool {pool.amountToPool} => {pool.amountToPool * 2}");
        CreatePool(prefabName, pool.amountToPool, pooledObjects[prefabName]);
        pool.amountToPool *= 2;
    }


    /// <summary>
    /// prefabName 에 맞는 타입의 풀을 찾아 그곳에 amountToPool 만큼의 오브젝트를 생성, 추가한다. 풀 안에 들어갈 오브젝트는 ItemBase를 상속받게하고 ItemBase 안에 returnPool라는 델리게이트를 만들어
    /// 자신이 어떤 큐로들어갈지 정하는 열거형을 매개변수로 Ondisable 될 때 신호를 보내고 이 ObjectPool 클래스의 ReturnToPool 함수를 여기에 연결시켜준다.
    /// </summary>
    /// <param name="prefabName"></param>
    /// <param name="amountToPool"></param>
    /// <param name="existingQueue">기존에 이미 큐가 만들어져있다면(확장할때) 그 큐를 가져온다. 처음만들때는 Queue가 없기 때문에 함수 안에서 새로 만들어준다</param>
    private void CreatePool(Pool.PrefabName prefabName, int amountToPool, Queue<GameObject> existingQueue = null)
    {
        Pool pool = pools.Find(p => p.name == prefabName); //prefabName에 맞는 pool을 가져온다(대입해준다)
        GameObject root = GameObject.Find($"{pool.prefab.name}_Pool"); //이미 만들어져있는 , 부모역할을 할 풀 오브젝트가 존재하는지 확인
        if (root == null)//없다면 새로 만들어서 할당
        {
            root = new GameObject($"{pool.prefab.name}_Pool");//개선해야할 부분// 새오브젝트를 만들고 이름을 할당
            root.transform.SetParent(transform);// 위에서 만든 오브젝트를 ObjectPool의 자식으로 등록
        }

        //Dictionary의 value값에 해당하는 <GameObject>타입의 큐 생성 // Dictionary 의 내용과 Pool클래스를 같이 작업하지만
        //Dictionary가 Pool에 종속된 개념은 아니다. Pool 안에 내용을 받아서 그 안의 내용에 맞게 Dictionary의 Key, Value를 조작해주는 것이다.
        Queue<GameObject> objectPool = existingQueue ?? new Queue<GameObject>(amountToPool); //existingQueue 가 없다면 (파라미터없이 호출했다면) 새로 큐를 만들어준다
        for (int j = 0; j < amountToPool; j++)
        {
            GameObject obj = Instantiate(pool.prefab, root.transform);//생성 후 root의 자식으로 등록
            ItemBase itemBase = obj.GetComponent<ItemBase>();// 델리게이트를 연결해주기 위해 생성한 오브젝트의 ItemBase컴포넌트를 가져온다
            itemBase.returnPool += ReturnToPool;// 연결
            obj.SetActive(false);
            objectPool.Enqueue(obj);// 큐에 넣어준다
        }

        pooledObjects[prefabName] = objectPool;//만들어진 풀을 pooledObjects Dictionary의 prefabName키값을 가진 큐에 대입해준다.
        Debug.Log($"PoolCount : {pooledObjects[prefabName].Count}");
    }
}
