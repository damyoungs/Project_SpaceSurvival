using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject<T> : MonoBehaviour where T : ObjectedPool
{
    [SerializeField]
    private int poolSize;
    [SerializeField]
    private GameObject prefabObject;

    T[] pool;

    Queue<T> readyQueue;
    
    public void Init() {
        if (pool == null) 
        {
            pool = new T[poolSize];
            readyQueue = new Queue<T>(poolSize);
            GenerateObject(0,poolSize,pool);
        }
        else
        {
            foreach (T obj in pool)
            {
                //��Ȱ��ȭ�� ��������Ʈ�Լ� ȣ���̵ǵ��� ������ִ�
                //��Ȱ��ȭ�� ť���ٰ� �����͸� ����־�д�
                obj.gameObject.SetActive(false); 
            }
        }
    }
    public T GetObject()
    {
        //ť�ȿ� �����Ͱ� ������ �������۾������ �����߻�
        if (readyQueue.Count > 0)
        {
            T comp = readyQueue.Dequeue();
            comp.gameObject.SetActive(true);
            return comp;
        }
        else 
        {
            ExpandPool();

            return GetObject();
        }
    }

    private void ExpandPool() {

        int newPoolSize = poolSize * 2;
        
        Debug.LogWarning($"{gameObject.name} �� Ǯ������ ���� : {poolSize} => {newPoolSize}");
        
        T[] newPool = new T[newPoolSize];
        
        for (int i = 0; i < poolSize; i++)
        {
            newPool[i] = pool[i];

        }
       
        GenerateObject(poolSize,newPoolSize,newPool);
        pool = newPool;
        poolSize = newPoolSize;

    }
    private void GenerateObject(int startIndex, int endIndex, T[] oldPool) 
    {
        for (int i = startIndex; i < endIndex; i++)
        {
            GameObject obj = Instantiate(prefabObject, transform);
            obj.name = $"{prefabObject.name}_{i}";

            T comp = obj.GetComponent<T>();
            comp.onDisable += () => readyQueue.Enqueue(comp);

            oldPool[i] = comp;
            obj.SetActive(false);
        }

    }

}
