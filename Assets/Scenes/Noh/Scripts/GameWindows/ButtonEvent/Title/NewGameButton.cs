using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Ÿ��Ʋ���� ����� ��ư ���ӽ��� ȭ�� �������� ����
/// </summary>
public class NewGameButton: MonoBehaviour
{
    public void OnClickNewStart()
    {
        LoadingScean.SceanLoading(EnumList.SceanName.TITLE);
    }
}
