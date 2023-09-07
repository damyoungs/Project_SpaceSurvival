using StructList;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoad_SceanMove : MonoBehaviour
{
    SlotManager slotManager;
    private void Awake()
    {
        slotManager = FindObjectOfType<SlotManager>(true);
    }
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
            SaveLoadManager.Instance.ParsingProcess.LoadParsing(data);
            Debug.Log($"{data} ������ ����ε����ϴ� , {data.SceanName} �Ľ��۾��� ���̵� �ۼ��� �ؾ��ϴ� ���� �ʿ��մϴ�.");
            if (SpaceSurvival_GameManager.Instance.GetBattleMapInit != null) //��Ʋ�ʵ����Ͱ� ���õ�������
            {
                SpaceSurvival_GameManager.Instance.BattleMapInitClass.TestReset();  //�ʱ�ȭ �ϱ�
            }
            LoadingScean.SceanLoading(data.SceanName);

        }
    }
 
}