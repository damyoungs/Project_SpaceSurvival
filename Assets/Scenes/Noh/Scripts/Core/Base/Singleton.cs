
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
//����Ƽ���� �̱������� ����� ����ؾ��� �κ� 
/*
    1. �̱���Ŭ������ ������ ���϶� Thread ó�� ���������� ���ôٹ������� �����ϴ°�� ó���� �ʿ�
    2. unity GameObject�� ���� ���� �ö󰥼��ֱ⿡ ������Ʈ �ϳ��� �ϳ����ؼ� �ߺ������� ���� ó���ʿ� 
        2-1 . Awake ���� �����ɶ� Ȯ��
        2-2 . �̱���Ŭ���� ȣ��� Ȯ��
    3. �� ��ȯ �� ������Ʈ�� �ʱ�ȭ�Ǵ� �ʱ�ȭ �ȵǰ� DontDestroyOnLoad(instance.gameObject); �Լ��� ��� 
 
    4. ��ӹ��� Ŭ������ ���׸��̸� AddComponent���� �ڷ��� ���¸� ã����������.
    - ����Ƽ������ �̱����� ���߿� �����Ȱ��� ����ϴ°��� ����.?
    - ������ �Ҵ�?
 */
//T �� �ݵ�� ���۳�Ʈ ���� �Ѵ�
//where �� ���� �ɱ����� �ۼ��Ѵ�.
public class Singleton<T> : MonoBehaviour where T : Component
{
    
    /// <summary>
    /// �̹� ����ó���� ������ Ȯ���ϱ� ���� ����
    /// </summary>
    private static bool isShutDown = false;
    /// <summary>
    /// �̱����� ��ü
    /// </summary>
    private static T instance;
    /// <summary>
    /// �̱��� ��ü�� �б� ���� ������Ƽ. ��ü�� ��������� �ʾ����� ���θ����.
    /// </summary>
    public static T Instance
    {
        get
        {
            if (isShutDown) 
            {//����ó�������� Ȯ��
                Debug.LogWarning($"{typeof(T).Name}�� �̹� ���� ���̴�.");
                return null; //ó����������� null�� �ѱ��.
            }
            if (instance == null)
            {
                if (FindObjectOfType<T>() == null)
                { //���� �̱������ִ��� Ȯ��
                    GameObject gameObj = new GameObject(); //������Ʈ���� 
                    gameObj.name = $"{typeof(T).Name} Singleton"; //�̸��߰��ϰ�
                    instance = gameObj.AddComponent<T>(); //�̱��水ü�� �߰��Ͽ� ����
                    DontDestroyOnLoad(instance.gameObject); //���� ������� ���ӿ�����Ʈ�� �������� ���ϰ��ϴ� �Լ�
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        { //���� ��ġ�Ǿ��ִ� ù��° �̱���  ���ӿ�����Ʈ
            instance = this as T;
            DontDestroyOnLoad(instance.gameObject); //���� ������� ���ӿ�����Ʈ�� �������� ���ϰ��ϴ� �Լ�
        }
        else
        { //ù��° �̱��� ���� ������Ʈ�� ������� ���Ŀ� ������� �̱��� ���� ������Ʈ
            if (instance != this)
            { //�ΰ���������µ� �������ϼ����־ �ƴҰ�츸 ó���Ѵ�. 
                Destroy(this.gameObject);  //ù��° �̱���� �ٸ� ��ü�̸� ����
            }
        }
    }


    protected virtual void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    protected virtual void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        
    }
    /// <summary>
    /// ���α׷��� ����ɶ� ����Ǵ� �Լ�.
    /// </summary>
    private void OnApplicationQuit()
    {
        isShutDown = true; //���� ǥ��
    }

    protected virtual void OnSceneLoaded(Scene scean, LoadSceneMode mode)
    {
        Init(scean,mode);
    }
    protected virtual void Init(Scene scene , LoadSceneMode mode) { 
        
    }
}

