using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �ɼǿ��� ���α׷����̹��� ���� ��� �ֱ����� ���� ����  
/// </summary>
public class ProgressList : SingletonBase<ProgressList> 
    /// �ϴ� ������ 
    
    /// ���α׷����� ����� �̽�ũ��Ʈ�� �߰�
{
    /// <summary>
    /// prefab �����̶� transform ���� �����̾ȵȴ�.
    /// </summary>
    [SerializeField]
    private GameObject originBar;
    /// <summary>
    /// Ʈ������ ���������� �����ص� �̹�������
    /// </summary>
    private GameObject progressBar;

    /// <summary>
    /// ProgressBar  
    /// </summary>
    /// <param name="type"></param>
    /// <param name="transform"></param>
    /// <returns></returns>
    public GameObject GetProgress(EnumList.ProgressType type, Transform transform) {
    
        switch (type) {
            case EnumList.ProgressType.Bar:
                if (progressBar == null) //���ʿ� ���� ������ �����Ѵ�.
                {
                    progressBar = Instantiate(originBar, transform); //�ε�â�� �׻� �ʱ�ȭ �Ǽ� �̸������ξ�� �Ź��ȸ����. - ���丮�� �����۾� ������
                }
                return progressBar;
            default:
                Debug.LogWarning($"{type.ToString()} �� ���� �������� �ʽ��ϴ�");
                return null;
        }
    }
}
