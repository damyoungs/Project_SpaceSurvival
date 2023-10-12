using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cho_SceneLoading : MonoBehaviour
{

    public Color clearLight;

    InteractionUI interaction;
    Cho_PlayerMove player;

    ParticleSystem shortRay;
    ParticleSystem longRay;
    AudioSource audioSource;
    SphereCollider sphereCollider;

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
        audioSource = GetComponent<AudioSource>();
        sphereCollider = GetComponent<SphereCollider>();
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
        player.Controller.enabled = false;
        player.transform.position = shortRay.transform.position;
        player.Controller.enabled = true;
        // 배틀끝나면 돌아올 위치값셋팅
        SpaceSurvival_GameManager.Instance.ShipStartPos = player.transform.position;
        StartCoroutine(WarpCoroutine());
    }

    public void ClearReturn()
    {

    }

    public void DeathReturn()
    {

    }

    // 수정 해야됨 + 인풋키 락 걸어야함
    IEnumerator WarpCoroutine()
    {
        player.InputActions.Disable();
        sphereCollider.enabled = false;
        interaction.invisibleUI?.Invoke();
        player.interaction = null;
        player.Cinemachine.Priority = 20;
        audioSource.Play();
        shortRay.Stop();
        longRay.Play();
        yield return new WaitForSeconds(3.0f);
        player.InputActions.Enable();
        player.Cinemachine.Priority = -10;
        //shortRay.Play();
        //longRay.Stop();
        sphereCollider.enabled = true;

        //스테이지 관련 셋팅 
        SpaceSurvival_GameManager.Instance.CurrentStage = currentStage; //이동할 스테이지 셋팅
        LoadingScene.SceneLoading(EnumList.SceneName.TestBattleMap);
    }

    /// <summary>
    /// 해당 함수 실행시켜서 해당 스테이지가 클리어 됬는지 체크한다.
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


    // z버튼은 무엇인가
    // e, u, k종료버튼 안됨
    // 마을로 돌아가는 법
    // 대화 종료시 자동으로 커서 안보이게 만들기
    // 포탈이동시 ui없애기
    // 포탈 돌아올 때 카메라 순위 지정
    // 시작시 클릭 소리 끄기
    // 마을에서 아무 상호작용 없을 때 f누르면 커서 나오는 것
    // 점프에 소리 넣기


    // 전투 끝났을 때 ui 손보기
    // 대화시 npc 애니메이션 변경
}
