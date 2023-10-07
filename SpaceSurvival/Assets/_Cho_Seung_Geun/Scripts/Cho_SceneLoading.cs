using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cho_SceneLoading : MonoBehaviour
{
    InteractionUI interaction;
    Cho_PlayerMove player;

    ParticleSystem shortRay;
    ParticleSystem longRay;

    /// <summary>
    /// ���� ��Ż�� �������� ����
    /// </summary>
    [SerializeField]
    StageList currentStage;

    private void Awake()
    {
        interaction = FindObjectOfType<InteractionUI>();
        player = FindObjectOfType<Cho_PlayerMove>();
        Transform parent = transform.parent;
        shortRay = parent.GetChild(0).GetComponent<ParticleSystem>();
        longRay = parent.GetChild(5).GetComponent<ParticleSystem>();
        longRay.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.interaction = Warp;
            interaction.visibleUI?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.interaction = null;
            interaction.invisibleUI?.Invoke();
        }
    }

    private void Warp()
    {
        // ��Ʋ������ ���ƿ� ��ġ������
        SpaceSurvival_GameManager.Instance.ShipStartPos = player.transform.position;
       //player.transform.position = shortRay.transform.position;
        StartCoroutine(WarpCoroutine());
    }

    // ���� �ؾߵ� + ��ǲŰ �� �ɾ����
    IEnumerator WarpCoroutine()
    {
        player.Cinemachine.Priority = 20;
        shortRay.Stop();
        longRay.Play();
        yield return new WaitForSeconds(3.0f);
        player.Cinemachine.Priority = -10;
        shortRay.Play();
        longRay.Stop();

        //�������� ���� ���� 
        SpaceSurvival_GameManager.Instance.CurrentStage = currentStage; //�̵��� �������� ����
        LoadingScene.SceneLoading(EnumList.SceneName.TestBattleMap);
    }

    /// <summary>
    /// �ش� �Լ� ������Ѽ� �ش� ���������� Ŭ���� ����� üũ�Ѵ� .
    /// </summary>
    /// <returns></returns>
    private bool IsStageClear()
    {
        return (SpaceSurvival_GameManager.Instance.StageClear & currentStage) > 0;
    }
    /// <summary>
    /// �ش� �Լ� ������Ѽ� ��ü ���������� Ŭ���� ����� üũ�Ѵ� .
    /// </summary>
    /// <returns></returns>
    private bool IsAllStageClear()
    {
        return SpaceSurvival_GameManager.Instance.StageClear == StageList.All;
    }
}
