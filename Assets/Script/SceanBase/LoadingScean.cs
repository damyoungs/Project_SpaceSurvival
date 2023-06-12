using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LoadingScean : MonoBehaviour
{
    /// <summary>
    /// ���������� �Ѿ ���̸�
    /// �������� �Է¾ȵǸ� Ÿ��Ʋ�γѾ��.
    /// </summary>
    static string nextSceanName = "Title";
    public static string SceanName => nextSceanName;
    /// <summary>
    /// �ε� ���൵ �̹��� ����
    /// EnumList �������̽��� �����س��� ���� �����Ѵ�.
    /// </summary>
    static EnumList.ProgressType progressType;

    /// <summary>
    /// ���൵�� ó���� �̹�������
    /// 
    /// �߰��۾�: EnumList �� Factory �� ������ �߰����ʿ� �Ʒ� switch���� �����߰�
    /// </summary>
    [SerializeField]
    Image progressBar;


    /// <summary>
    /// ����ũ �ε� �ð����� 
    /// �ε��� �ּҽð��̶󺸸�ȴ�.
    /// </summary>
    [Range(1.0f,5.0f)]
    public float fakeTimer = 3.0f;

    

    /// <summary>
    /// ���� �ε��Ǳ����� �Ѿ���ʰ� �ε�ȭ������ �ϴܳѾ�� 
    /// �������� �񵿱�� �ε������ϰ� �����൵�� �����ֱ����� �ۼ����Լ�
    /// </summary>
    /// <param name="sceanName">�̵��� �� �̸�</param>
    /// <param name="type">���� ��Ȳ ǥ���� Ÿ�� �⺻�� =  ���������</param>
    public static void LoadScean(EnumList.SceanName sceanName , EnumList.ProgressType type = EnumList.ProgressType.Bar) { 
        nextSceanName = sceanName.ToString(); //enum�ɹ��������� ���̸����θ�����Ѵ�.
        progressType = type;
        SceneManager.LoadSceneAsync("Loading");
    }

    /// <summary>
    /// �ε�ȭ�� �ε��� �ٷ� �ڷ�ƾ �����Ͽ� ������������ �񵿱�� �ε��� �ϰ�
    /// �׿����� ������ �޾ƿ´�.
    /// </summary>
    void Start()
    {
        StopAllCoroutines();//�ε��� �������� �̷����°�쿡 �����ڷ�ƾ�� ���߰� ���ν����Ѵ�.
        StartCoroutine(LoadSceanProcess());  
    }

    /// <summary>
    /// �ε�ȭ�鿡�� �������� �ε��� �Ϸ����� Ȯ���ϱ����� ó���ϴ��۾�
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadSceanProcess() {

       
        //�񵿱� ���ε������� �ޱ����� �������� ����
        AsyncOperation op = SceneManager.LoadSceneAsync(nextSceanName);
        
        //op.allowSceneActivation ���� true �̸� �� �ε��� 90%(0.9f)�̻��� �Ǹ� �ڵ����� ���������� �Ѿ����
        op.allowSceneActivation = false;
        //���� ������� �ε��ð��� ª�Ƽ� �ε� ȭ���� ���İ��� ���������־ �ϴ� false �� 
        //�ε������� ���������� �̵��� ����ΰ� ����ũ�ε��� �ؿ� �����Ѵ�.
        
       

        float timer = 0.0f; //�ε� ���൵�� ���� ���� (�Լ��� ����߱⶧���� ���� ó���ϱ����� ����ߴ�.)
        float loadingTime = 0.0f; //���α׷����� ����ð�üũ
        while (!op.isDone)  //isDone ���� ������ �ε��� �������� üũ�Ҽ��ִ�.
        {
            
            yield return null; //����ٰ� �ٲ���ְ� ������� �ѱ��.
            loadingTime  += Time.deltaTime; //�ε��ð� üũ

            // ���൵ ǥ�ø� �ٲٷ��� �ؿ� ������ �߰�
            switch (progressType)
            {
                case EnumList.ProgressType.Bar:
                    if (op.progress < 0.9f) 
                    {
                        //���� �ε� ���� 
                        progressBar.fillAmount = op.progress; //ȭ�� �̹����� �����Ȳ�� ����
                    }
                    else
                    {
                        //����ũ�ε� ����

                        timer += Time.unscaledDeltaTime; //Lerp �Լ��� ���� ��Ȳ����
                        progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer); // 0.9 ~ 1.0 ������ ������ ǥ��

                        //�ε�â�� �ʹ������� �Ѿ�°��� �����ϱ����� ����ũ Ÿ�� üũ
                        if (fakeTimer <  loadingTime) //�����Ϳ��� ����ũ�ε��ð��� �����Ѵ�.
                        { 
                            Debug.Log(loadingTime);    //�� �ɸ��ð� üũ
                            op.allowSceneActivation = true; //�ش� ������ true�� progress ���� 0.9(90%)���� �Ѿ�¼��� �������� �ε��Ѵ�.
                            yield break; //�ڷ�ƾ ������
                        }
                    }
                    break;
                default: //Ÿ�԰��� �߸��Է�������� �̰����� �̵�
                    Debug.LogWarning($"{this.name} �� ���α׷���(progress)�� Ÿ�Լ����� �߸��߽��ϴ�. ");
                    yield break;
            }
        }
    }



    
}
