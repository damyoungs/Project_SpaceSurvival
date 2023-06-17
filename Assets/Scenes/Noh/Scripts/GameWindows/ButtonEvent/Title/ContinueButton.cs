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
    EnumList.SceanName sceanName;
    private void Awake()
    {
        sceanName = EnumList.SceanName.Title;
    }
    public void OnClickContinue()
    {
        LoadingScean.SceanLoading(sceanName);
    }
}