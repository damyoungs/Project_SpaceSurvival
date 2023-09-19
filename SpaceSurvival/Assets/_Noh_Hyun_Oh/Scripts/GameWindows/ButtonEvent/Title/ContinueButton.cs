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
    EnumList.SceneName sceneName;
    private void Awake()
    {
        sceneName = EnumList.SceneName.TestBattleMap;
    }
    public void OnClickContinue()
    {
        LoadingScene.SceneLoading(sceneName);
    }
}
