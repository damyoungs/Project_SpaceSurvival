using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;



/// <summary>
/// �ε��� ������ Ŭ����
/// ������� :
///     1. ���丮 �� Ǯ (ť)�� ������ Ÿ��Ʋ�� ���� �ʱ�ȭ ���� 
/// </summary>
public class LoadingScean : MonoBehaviour
{
    /// <summary>
    /// �ε���Ȳ�� üũ�ϴº���
    /// </summary>
    static bool isLoading = false;
    public static bool IsLoading => isLoading;

    /// <summary>
    /// ���������� �Ѿ ���̸�
    /// �������� �Է¾ȵǸ� Ÿ��Ʋ�γѾ��.
    /// </summary>
    static  string nextSceanName = "Title"; // ���������̱����� �������.
    //static int nextSceanName = 0; //���� ���̰� �̹��ϰ� ��������

    /// <summary>
    /// ���� ���̸����� ������Ƽ
    /// �����Ҷ� ����ҰŰ���.
    /// </summary>
    public static string SceanName => nextSceanName;

    /// <summary>
    /// �ε� ���൵ �̹��� ����
    /// EnumList �������̽��� �����س��� ���� �����Ѵ�.
    /// </summary>
    static EnumList.ProgressType progressType;

    /// <summary>
    /// ���൵�� ó���� �̹�������
    /// </summary>
    Image progressImg;

    /// <summary>
    /// ����ũ �ε� �ð����� 
    /// �ε��� �ּҽð��̶󺸸�ȴ�.
    /// </summary>
    [Range(1.0f, 5.0f)]
    public float fakeTimer = 3.0f;

    /// <summary>
    /// ���ε��� ���൵�� �����ִ¾����� �Ѿ�� �Լ� �񵿱������
    /// �ε������� ��� �Ѿ�ٰ� �̵��Ѵ�.
    /// </summary>
    /// <param name="sceanName">�̵��� �� �̸�</param>
    /// <param name="type">���� ��Ȳ ǥ���� progressType  EnumList�� ����Ȯ��</param>
    public static void SceanLoading(EnumList.SceanName sceanName = EnumList.SceanName.Title, EnumList.ProgressType type = EnumList.ProgressType.Bar)
    {
        nextSceanName = sceanName.ToString(); 
        progressType = type;
        SceneManager.LoadSceneAsync("Loading");
    }

    /// <summary>
    /// �ε�ȭ�� �ε��� �ٷ� �ڷ�ƾ �����Ͽ� ������������ �񵿱�� �ε��� �ϰ�
    /// �׿����� ������ �޾ƿ´�.
    /// </summary>
    void Start()
    {
        isLoading = true;//�ε����ۉ�ٰ� ����
        StopAllCoroutines();//�ε��� �������� �̷����°�쿡 �����ڷ�ƾ�� ���߰� ���ν����Ѵ�.
        StartCoroutine(LoadSceanProcess());
    }

    /// <summary>
    /// �ε�ȭ�鿡�� �������� �ε��� �Ϸ����� Ȯ���ϱ����� ó���ϴ��۾�
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadSceanProcess()
    {


        //�񵿱� ���ε������� �ޱ����� �������� ����
        AsyncOperation op = SceneManager.LoadSceneAsync(nextSceanName);

        //op.allowSceneActivation ���� true �̸� �� �ε��� 90%(0.9f)�̻��� �Ǹ� �ڵ����� ���������� �Ѿ����
        op.allowSceneActivation = false;
        //���� ������� �ε��ð��� ª�Ƽ� �ε� ȭ���� ���İ��� ���������־ �ϴ� false �� 
        //�ε������� ���������� �̵��� ����ΰ� ����ũ�ε��� �ؿ� �����Ѵ�.

        float timer = 0.0f; //�ε� ���൵�� ���� ���� (�Լ��� ����߱⶧���� ���� ó���ϱ����� ����ߴ�.)
        float loadingTime = 0.0f; //���α׷����� ����ð�üũ
        switch (progressType)
        {
            case EnumList.ProgressType.Bar:
                //������̹��� ���������� ��������
                progressImg = ProgressList.Instance.GetProgress(EnumList.ProgressType.Bar, transform)
                                .transform.GetChild(1).GetComponent<Image>(); //Prefab �� ����1��°�� �����صξ���  

                while (!op.isDone)  //isDone ���� ������ �ε��� �������� üũ�Ҽ��ִ�.
                {
                    yield return null; //����ٰ� �ٲ���ְ� ������� �ѱ��.
                    loadingTime += Time.unscaledDeltaTime; //�ε��ð� üũ



                    // ���൵ ǥ�ø� �ٲٷ��� �ؿ� ������ �߰�
                    if (op.progress < 0.9f)
                    {
                        //���� �ε� ���� 
                        progressImg.fillAmount = op.progress; //ȭ�� �̹����� �����Ȳ�� ����
                    }
                    else
                    {
                        //����ũ�ε� ����

                        timer += Time.unscaledDeltaTime; //Lerp �Լ��� ���� ��Ȳ����
                        progressImg.fillAmount = Mathf.Lerp(0.9f, 1f, timer); // 0.9 ~ 1.0 ������ ������ ǥ��

                        //�ε�â�� �ʹ������� �Ѿ�°��� �����ϱ����� ����ũ Ÿ�� üũ
                        if (fakeTimer < loadingTime) //�����Ϳ��� ����ũ�ε��ð��� �����Ѵ�.
                        {
                            Debug.Log(loadingTime);    //�� �ɸ��ð� üũ
                            isLoading = false;//�ε������ٰ� ����
                            op.allowSceneActivation = true; //�ش� ������ true�� progress ���� 0.9(90%)���� �Ѿ�¼��� �������� �ε��Ѵ�.
                            yield break; //����ǳѱ��
                        }
                    }
                }
                break;
            //�ε��̹��� �߰��� ����ġ���� �ۼ�
            default: //Ÿ�԰��� �߸��Է�������� �̰����� �̵�
                Debug.LogWarning($"{this.name} �� ���α׷���(progress)�� Ÿ�Լ����� �߸��߽��ϴ�. ");
                yield break;
        }



    }
}
