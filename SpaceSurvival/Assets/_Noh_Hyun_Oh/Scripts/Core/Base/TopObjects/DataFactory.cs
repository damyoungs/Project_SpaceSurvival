using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����Ͱ��� �ֻ��� ������Ʈ�����
/// ������ ���� ����Ȱ��� ������������̴�
/// </summary>
public class DataFactory : Singleton<DataFactory> {
    /// <summary>
    /// ����Ʈ �������ִ°�
    /// </summary>
    QuestScriptableGenerate questScriptableGenerate;
    public QuestScriptableGenerate QuestScriptableGenerate;

    protected override void Awake()
    {
        base.Awake();
        questScriptableGenerate = GetComponent<QuestScriptableGenerate>();
    }
}
