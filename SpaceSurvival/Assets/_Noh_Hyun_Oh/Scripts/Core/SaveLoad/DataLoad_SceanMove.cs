using StructList;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ε� ������ ������� ó���� ���� �����ϱ����� ���λ���.
/// </summary>
public class DataLoad_SceanMove : MonoBehaviour
{
    
    
   

    private void Start()
    {
        SaveLoadManager.Instance.loadedSceanMove = FileLoadAction;
    }
    /// <summary>
    /// �ε� �������� ȭ���̵��� ������ ���� �Լ�
    /// </summary>
    /// <param name="data">�ε�� ������</param>
    private void FileLoadAction(JsonGameData data)
    {
        //���⿡ �Ľ��۾����ʿ��ϴ� �����λ��Ǵ� �۾�
        if (data != null)
        {
            SaveLoadManager.Instance.ParsingProcess.LoadParsing(data); //����� ������ ���� 
            Debug.Log($"{data} ������ ����ε����ϴ� , {data.SceanName} �Ľ��۾��� ���̵� �ۼ��� �ؾ��ϴ� ���� �ʿ��մϴ�.");
            LoadingScean.SceanLoading(data.SceanName);
            if (TurnManager.Instance.TurnIndex > 0) //���������̸� ��Ʋ�������� 
            {
                SpaceSurvival_GameManager.Instance.BattleMapInitClass.TestReset(); //������ �ʱ�ȭ�ϰ� 
                SpaceSurvival_GameManager.Instance.BattleMapInitClass.TestInit(); //�ٽ� �����ϰ� 
            }

        }
    }
 
}