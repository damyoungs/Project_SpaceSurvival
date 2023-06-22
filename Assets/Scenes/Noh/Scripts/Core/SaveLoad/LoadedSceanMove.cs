using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadedSceanMove : MonoBehaviour
{
    private void Start()
    {
        SaveLoadManager.Instance.loadedSceanMove += FileLoadAction;
    }

    /// <summary>
    /// �ε� �������� ȭ���̵��� ������ ���� �Լ�
    /// </summary>
    /// <param name="data">�ε�� ������</param>
    private void FileLoadAction(JsonGameData data)
    {
        //���⿡ �Ľ��۾����ʿ��ϴ� �����λ��Ǵ� �۾�
        Debug.Log($"�Ľ��۾��ؾߵ˴ϴ� : {data} �̰ɷ�");
        if (data != null)
        {
            Debug.Log($"{data} ������ ����ε����ϴ� , {data.SceanName} �Ľ��۾��� ���̵� �ۼ��� �ؾ��ϴ� ���� �ʿ��մϴ�.");
            LoadingScean.SceanLoading(data.SceanName);
        }
    }

}