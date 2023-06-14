using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : PooledObject
{
    public GameObject originalPrefab;

    public int poolSize = 32;

    T[] pool;
    Queue<T> readyQueue;


    void Initialize()
    {
        if (pool == null)
        {
            pool = new T[poolSize];
            readyQueue = new Queue<T>(poolSize);

            GenerateObject(0, poolSize, pool);
        }
        else
        {
            foreach(T obj in pool)
            {
                obj.gameObject.SetActive(false);
            }
        }
 
    }
    public T GetObject()
    {
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

    private void ExpandPool()
    {
        int newSize = poolSize * 2;
        T[] newArray = new T[newSize];
        for (int i = 0; i < poolSize; i++)
        {
            newArray[i] = pool[i]; 
        }
        GenerateObject(poolSize, newSize, newArray);
        pool = newArray;
        poolSize = newSize;
    }

    private void GenerateObject(int start, int end, T[] newArray)
    {
        for (int i = start; i < end; i++)
        {
            GameObject obj = Instantiate(originalPrefab, transform);
            obj.name = $"{originalPrefab.name}_{i}";

            T comp = obj.GetComponent<T>();
            comp.onDie += () => readyQueue.Enqueue(comp);

            newArray[i] = comp;
            obj.SetActive(false);
        }
    }
}
