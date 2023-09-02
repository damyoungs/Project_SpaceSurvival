using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitCharcterSetting : MonoBehaviour
{
    /// <summary>
    /// ���� ���� 
    /// </summary>
    [SerializeField]
    int teamLength = 2;


    /// <summary>
    /// ���� ������ �� �迭
    /// </summary>
    ITurnBaseData[] teamArray;


    /// <summary>
    /// �̴ϸ� ī�޶� �������� �̿ϼ� - ���߿� Ÿ�ٵ� �ŰܾߵǼ� ������ �ʿ��ϴ� 
    /// </summary>
    MiniMapCamera miniCam;

    /// <summary>
    /// �̴ϸʰ� ���������� ���߿� Ÿ���� �ٲ�� �̰͵� �ٲ��ߵ����� ������ �ʿ� 
    /// </summary>
    CameraOriginTarget cameraOriginTarget;


    private void Awake()
    {
        miniCam = FindObjectOfType<MiniMapCamera>(true);
        cameraOriginTarget = FindObjectOfType<CameraOriginTarget>(true);
    }

    /// <summary>
    /// �׽�Ʈ�� ������ ���� 
    /// </summary>
    public void TestInit()
    {
        teamArray = new ITurnBaseData[teamLength]; //�迭 ũ�����
        if (TurnManager.Instance.IsViewGauge) // ������ �������� üũ�ؼ� 
        {
            WindowList.Instance.TurnGaugeUI.gameObject.SetActive(true); //�����ָ� ǥ��
        }
        ITurnBaseData tbo; //�ӽ÷� ���� ���� �����ϰ� 
        for (int i = 0; i < teamLength; i++)
        {
            if (i == 0) // ù��°�� ������ �Ʊ� ���� ����ְ� 
            {
                tbo = (PlayerTurnObject)Multiple_Factory.Instance.GetObject(EnumList.MultipleFactoryObjectList.BATTLEMAP_PLAYER_POOL); //
                tbo.UnitBattleIndex = i; 
                tbo.gameObject.name = $"Player_Team_{i}";
                tbo.InitData();
                miniCam.player = tbo.CharcterList[0].transform; //�̴ϸ� Ȱ��ȭ�� �����ʿ� 
                cameraOriginTarget.Target = tbo.CharcterList[0].transform;
            }
            else //���̿ܿ��� ���ͳ� �߸� �� ������ȴ� ���ǹ� �߰��ʿ� 
            {
                tbo = (EnemyTurnObject)Multiple_Factory.Instance.GetObject(EnumList.MultipleFactoryObjectList.BATTLEMAP_ENEMY_POOL); //��������
                tbo.UnitBattleIndex = i;
                tbo.gameObject.name = $"ENEMY_Team_{i}";
                tbo.InitData();
            }
            tbo.TurnActionValue = UnityEngine.Random.Range(0.0f, 8.0f); // -�׽�Ʈ�� ����
            teamArray[i] = tbo;
        }

        //������ �ʱ�ȭ������ �Ͻ��� 
        TurnManager.Instance.InitTurnData(teamArray);

        Transform battleActionButtons = WindowList.Instance.BattleActionButtons;
        int childCount = battleActionButtons.childCount;
        for (int i = 0; i < childCount; i++)
        {
            battleActionButtons.GetChild(i).gameObject.SetActive(true);
        }
        miniCam.gameObject.SetActive(true); //�̴ϸ� Ȱ��ȭ �����ʿ� 
        cameraOriginTarget.gameObject.SetActive(true);
    }

    public void TestReset() 
    {
        TurnManager.Instance.ResetBattleData(); //�ϵ����� �ʱ�ȭ 

        Transform battleActionButtons = WindowList.Instance.BattleActionButtons;
        int childCount = battleActionButtons.childCount;
        for (int i = 0; i < childCount; i++)
        {
            battleActionButtons.GetChild(i).gameObject.SetActive(false); //��Ʋ�� �׼ǹ�ư ���� 
        }
        WindowList.Instance.TeamBorderManager.UnView(); //�� ��� ������ ���� 
    }

}
     //UI ����
        //for (int i = 1; i<uiParent.childCount - 1; i++)
        //{
        //    for (int j = 0; j<uiParent.GetChild(i).childCount; j++)
        //    {
        //        uiParent.GetChild(i).GetChild(j).gameObject.SetActive(false); //�׼ǹ�ư ���� 
        //    }
        //}
        //turnGaugeUI.gameObject.SetActive(false); //�ϰ����� ����

/*======================================== �׽�Ʈ�� ==================================================*/


/// <summary>
/// �ϸ���Ʈ�� ��Ȯ�ο�
/// </summary>
//public void ViewTurnList()
//{
//    foreach (ITurnBaseData j in turnObjectList)
//    {
//        Debug.Log($"{j} �� : {j.TurnActionValue}");
//    }
//    Debug.Log(turnObjectList.Count);
//}
///// <summary>
///// ���� �������� ������ ã�ƿ��� 
///// </summary>
///// <returns></returns>
//public ITurnBaseData GetNode()
//{
//    ITurnBaseData isTurnNode = null;
//    foreach (ITurnBaseData node in turnObjectList)
//    {
//        if (node.TurnEndAction != null) //���� ���������̸� endAction �� ��ϵ��ִ� 
//        {
//            Debug.Log(node.transform.name);
//            isTurnNode = node;//�������� ��� ��Ƽ� 
//        }
//    }
//    return isTurnNode;//��ȯ
//}
///// <summary>
///// �׽�Ʈ�� ������ ���� �������� 
///// </summary>
///// <returns></returns>
//public ITurnBaseData RandomGetNode()
//{
//    ITurnBaseData isRandNode = null;
//    int randomIndex = UnityEngine.Random.Range(0, turnObjectList.Count); //����Ʈ�� ������ �ε��� �� �������� 
//    LinkedListNode<ITurnBaseData> node = turnObjectList.First;
//    for (int i = 0; i < turnObjectList.Count; i++)
//    {
//        if (randomIndex == i)
//        {
//            isRandNode = node.Value;
//            break;
//        }
//        node = node.Next;
//    }
//    return isRandNode;
//}
//}