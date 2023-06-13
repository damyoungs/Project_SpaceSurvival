using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T instance;
    static bool isShutDown = false;
    public static T Inst
    {
        get
        {
            if (isShutDown)
            {
                Debug.LogWarning("ShutDown");
                return null;
            }
            if (instance == null)
            {
                T singleton = FindObjectOfType<T>();
                if (singleton == null)
                {
                    GameObject newObj = new GameObject();
                    newObj.name = $"{typeof(T).Name} : Singleton";
                    singleton = newObj.AddComponent<T>();
                }
                instance = singleton;
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(instance.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += On_sceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= On_sceneLoaded;
    }
    private void On_sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        OnInitialize();
    }
    protected virtual void OnInitialize()
    {

    }
    private void OnApplicationQuit()
    {
        isShutDown = true;
    }
}
