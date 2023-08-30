using System.Collections;
using System.Collections.Generic;
using Unity.XR.OpenVR;
using UnityEngine;

public class EnemyTurnObject : TurnBaseObject
{
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
        if (UnitBattleIndex > -1) //��Ʋ �ε����� ���õ������� ��Ʋ�������� ��Ʋ�� �϶���  
        {
            for (int i = 0; i < 3; i++)//ĳ���͵� �����ؼ� ���� 
            {
                GameObject go = MultipleObjectsFactory.Instance.GetObject(EnumList.MultipleFactoryObjectList.CHARCTER_ENEMY_POOL).gameObject;
                charcterList.Add(go.GetComponent<ICharcterBase>());
                go.name = $"Enemy_{i}";
                go.SetActive(true);
            }
        }
    }
}
