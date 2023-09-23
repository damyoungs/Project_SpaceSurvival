using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ӳ����� �ѹ��� �����ǰ� ����Ʈ �� ������ ������ Ŭ���� 
/// </summary>
public class QuestScriptableGenerate : BaseScriptableObjectGenerate<Gyu_QuestBaseData>
{
    /// <summary>
    /// ���ӽ��۽� ����Ʈ������ �ε����� �����ϱ����� �� 
    /// ����Ʈ �ε��� �� ���� ������
    /// </summary>
    int questIndex = 0;

    /// <summary>
    /// ����Ʈ ������ ���̵�� �����ϱ����� �ε��� ���� �ش�. 
    /// ����Ʈ �������� �ش� ���̻����� ��������� �ϱ����� ����
    /// </summary>
    int questIndexGap = 1000;

    /// <summary>
    /// ����Ʈ�� ������ �ľ��� ������
    /// </summary>
    [SerializeField]
    int questCount = 0;
    /// <summary>
    /// ���� ����Ʈ ��ũ���ͺ� ������ ���� �迭
    /// </summary>
    [SerializeField]
    Gyu_QuestBaseData[] originMainStoryQuestArray;

    /// <summary>
    /// ��� ����Ʈ ��ũ���ͺ� ������ ���� �迭
    /// </summary>
    [SerializeField]
    Gyu_QuestBaseData[] originKillcountQuestArray;

    /// <summary>
    /// ���� ����Ʈ ��ũ���ͺ� ������ ���� �迭
    /// </summary>
    [SerializeField]
    Gyu_QuestBaseData[] originGatheringQuestArray;


    [Header("���ӻ��� ��� ����Ʈ �迭")]
    /// <summary>
    /// ���� ����Ʈ ��Ƶ� �迭 
    /// </summary>
    Gyu_QuestBaseData[] mainStoryQuestArray;
    public Gyu_QuestBaseData[] MainStoryQuestArray => mainStoryQuestArray;

    /// <summary>
    /// ��� ����Ʈ ��Ƶ� �迭 
    /// </summary>
    Gyu_QuestBaseData[] killcountQuestArray;
    public Gyu_QuestBaseData[] KillcountQuestArray => killcountQuestArray;

    /// <summary>
    /// ���� ����Ʈ ��Ƶ� �迭
    /// </summary>
    Gyu_QuestBaseData[] gatheringQuestArray;
    public Gyu_QuestBaseData[] GatheringQuestArray => gatheringQuestArray;

    private void Awake()
    {
        mainStoryQuestArray = QuestDataGenerate(originMainStoryQuestArray);
        killcountQuestArray = QuestDataGenerate(originKillcountQuestArray);
        gatheringQuestArray = QuestDataGenerate(originGatheringQuestArray);
        SetAllDataIndexing();
    }
    /// <summary>
    /// ���� �ʱ�ȭ�� ����Ʈ�� �ε����� �����ϱ����� �����ų���� 
    /// </summary>
    private void SetAllDataIndexing() 
    {
        questIndex = questIndexGap * 0; // �ε��� 0���� �ʱ�ȭ 
        ArrayIndexsing(mainStoryQuestArray);

        questIndex = questIndexGap * 1; // �������Ʈ�ε����� questIndexGap * 1 ���� ����
        ArrayIndexsing(killcountQuestArray);
        
        questIndex = questIndexGap * 2; // ��������Ʈ�ε����� questIndexGap * 2 ���� ����
        ArrayIndexsing(gatheringQuestArray);
    }
    /// <summary>
    /// �ʱ�ȭ�� ����Ǹ�
    /// �ε����� �����ϱ����� �Լ�
    /// </summary>
    /// <param name="resetDataArray">�ε��� ������ ����Ʈ �迭</param>
    private void ArrayIndexsing(Gyu_QuestBaseData[] resetDataArray) 
    {

        foreach (Gyu_QuestBaseData questData in resetDataArray)
        {
            questData.QuestId = questIndex;
            questIndex++;
        }
    }
    /// <summary>
    /// ���������ʹ� ���ΰ� �������� ���鸸 ���� �ʱ�ȭ �ϴ� �Լ� 
    /// </summary>
    public void ResetData() 
    {
        ArrayDataReset(mainStoryQuestArray);
        ArrayDataReset(killcountQuestArray);
        ArrayDataReset(gatheringQuestArray);
    }
    /// <summary>
    /// ������ �ȵǾ� �� ���� ���ΰ� 
    /// ��������� �����Ǵ� �����͸� ���½�Ű�� �Լ� 
    /// </summary>
    /// <param name="resetDataArray">������ �迭</param>
    private void ArrayDataReset(Gyu_QuestBaseData[] resetDataArray)
    {
        foreach (Gyu_QuestBaseData questData in resetDataArray)
        {
            questData.ResetData();
        }
    }
}
