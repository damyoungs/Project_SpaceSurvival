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
            LoadingScean.SceanLoading(data.SceanName);
            if (TurnManager.Instance.TurnIndex > 0) //��Ʋ�ʿ��� �ε��ѰŸ� 
            {
                if (data.SceanName == EnumList.SceanName.TestBattleMap) // �ҷ����°��� ��Ʋ���̸�  
                {
                    SpaceSurvival_GameManager.Instance.BattleMapInitClass.TestReset(true);  //��Ʋ�� ������ �ʱ�ȭ 
                    SpaceSurvival_GameManager.Instance.BattleMapInitClass.TestInit();  //������ ����
                }
                else 
                {
                    SpaceSurvival_GameManager.Instance.BattleMapInitClass.TestReset();  //��Ʋ�� ������ �ʱ�ȭ 
                }
            }

        }
    }
 
}