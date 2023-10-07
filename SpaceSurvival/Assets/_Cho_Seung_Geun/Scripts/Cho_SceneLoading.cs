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
    /// 현재 포탈의 스테이지 종류
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
        // 배틀끝나면 돌아올 위치값셋팅
        SpaceSurvival_GameManager.Instance.ShipStartPos = player.transform.position;
       //player.transform.position = shortRay.transform.position;
        StartCoroutine(WarpCoroutine());
    }

    // 수정 해야됨 + 인풋키 락 걸어야함
    IEnumerator WarpCoroutine()
    {
        player.Cinemachine.Priority = 20;
        shortRay.Stop();
        longRay.Play();
        yield return new WaitForSeconds(3.0f);
        player.Cinemachine.Priority = -10;
        shortRay.Play();
        longRay.Stop();

        //스테이지 관련 셋팅 
        SpaceSurvival_GameManager.Instance.CurrentStage = currentStage; //이동할 스테이지 셋팅
        LoadingScene.SceneLoading(EnumList.SceneName.TestBattleMap);
    }

    /// <summary>
    /// 해당 함수 실행시켜서 해당 스테이지가 클리어 됬는지 체크한다 .
    /// </summary>
    /// <returns></returns>
    private bool IsStageClear()
    {
        return (SpaceSurvival_GameManager.Instance.StageClear & currentStage) > 0;
    }
    /// <summary>
    /// 해당 함수 실행시켜서 전체 스테이지가 클리어 됬는지 체크한다 .
    /// </summary>
    /// <returns></returns>
    private bool IsAllStageClear()
    {
        return SpaceSurvival_GameManager.Instance.StageClear == StageList.All;
    }
}
