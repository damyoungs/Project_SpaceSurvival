using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Ÿ��Ʋ��������� ��ư �ε�ȭ�� ���۳����� 
/// �׽�Ʈ�� �ε�ȭ�鿡�� �������������� ���ѷε��˴ϴ�..
/// </summary>
public class ContinueButton : MonoBehaviour
{
    [SerializeField]
    EnumList.SceneName sceanName;
    private void Awake()
    {
        sceanName = EnumList.SceneName.TestBattleMap;
    }
    public void OnClickContinue()
    {
        LoadingScean.SceanLoading(sceanName);
    }
}