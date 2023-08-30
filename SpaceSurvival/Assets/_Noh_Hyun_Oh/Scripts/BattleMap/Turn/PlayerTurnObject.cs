using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �÷��̾� �� 
/// </summary>
public class PlayerTurnObject : TurnBaseObject
{
    public Func<ICharcterBase[]> initPlayer;
    UICamera cam;
    protected override void Awake()
    {
        base.Awake();
        onEnable_InitData += () =>
        {
            battleIndex = -1;
            if (TurnManager.Instance != null)
            {
                //Ȱ��ȭ �ϱ����� �������� �� ���� �Լ�
                //�̷��� ���丮���� get �Ҷ� ��Ȱ��ȭ���� -> �ʱ�ȭ -> Ȱ��ȭ �� �����ϴ� 
                battleIndex = TurnManager.Instance.BattleIndex;
            }
        };
        //�ּ������� �ʱ�ȭ����� �Ҹɹ���
        turnAddValue = 0.06f;                       // ���ϴ� ȸ���� ��ġ
        maxTurnValue = 1.8f;                        // �ִ�� ȸ���� ��ġ
        TurnActionValue = 0.0f;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        if (battleIndex > -1) //��Ʋ �ε����� ���õ������� ��Ʋ�������� ��Ʋ�� �϶���  
        {
            ICharcterBase[] playerList = initPlayer?.Invoke(); //�ܺο��� ĳ���͹迭�� ���Դ��� üũ
            if (playerList == null || playerList.Length == 0) //ĳ���� �ʱ�ȭ�� �ȵ�������  
            {
                //�׽�Ʈ ������ ����
                for (int i = 0; i < 3; i++)//ĳ���͵� �����ؼ� ���� 
                {
                    GameObject go = MultipleObjectsFactory.Instance.GetObject(EnumList.MultipleFactoryObjectList.CHARCTER_PLAYER_POOL).gameObject;
                    charcterList.Add(go.GetComponent<ICharcterBase>());
                    go.name = $"Player_{i}";
                    go.SetActive(true);
                }
            }
            else // �ܺο��� �����Ͱ� ���������  
            {
                foreach (ICharcterBase player in playerList)
                {
                    charcterList.Add(player); //�ϰ����� ĳ���ͷ� ����
                }
            }
        }
    }

}
