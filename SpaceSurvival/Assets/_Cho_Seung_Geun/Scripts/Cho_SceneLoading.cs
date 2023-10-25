using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cho_SceneLoading : MonoBehaviour
{
    public GameObject prop;
    public GameObject clearLight;
    Material clearRay;

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
        prop.SetActive(false);
        clearRay = clearLight.GetComponent<SkinnedMeshRenderer>().materials[0];

        interaction = FindObjectOfType<InteractionUI>();
        player = FindObjectOfType<Cho_PlayerMove>();
        Transform parent = transform.parent;
        shortRay = parent.GetChild(0).GetComponent<ParticleSystem>();
        longRay = parent.GetChild(5).GetComponent<ParticleSystem>();
        longRay.Stop();
        audioSource = GetComponent<AudioSource>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        if (IsStageClear())
        {
            shortRay.Stop();                        // 스테이지가 클리어 돼있으면 이펙트의 짧은 빛 멈추기
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!IsStageClear())                    // 스테이지가 클리어 돼있지 않으면
            {
                player.interaction = Warp;          // 플레이어의 상호작용은 warp로 변경
                interaction.visibleUI?.Invoke();    // ui 보여주게 만들기(F)
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.interaction = null;              // 상호작용 null로 변경
            interaction.invisibleUI?.Invoke();      // ui 가리기(F)
            if (IsStageClear())
            {
                shortRay.Stop();                        // 만약 스테이지를 클리어했다면 짧은 빛 끄고 아래의 코루틴 실행(다른 이펙트가 나옴)
                StartCoroutine(ClearLightVisible());
            }
        }
    }

    private void Warp()
    {
        player.Controller.enabled = false;                              // 플레이어 컨트롤러 임시로 끄기(플레이어 위치 이동을 위해)
        player.transform.position = shortRay.transform.position;
        player.Controller.enabled = true;                               // 플레이어 컨트롤러 다시 켜기
        
        SpaceSurvival_GameManager.Instance.ShipStartPos = player.transform.position;    // 배틀끝나면 돌아올 위치값 셋팅
        StartCoroutine(WarpCoroutine());        // 워프 코루틴 실행
    }

    IEnumerator WarpCoroutine()
    {
        player.InputActions.Disable();
        sphereCollider.enabled = false;
        interaction.invisibleUI?.Invoke();
        player.interaction = null;
        player.Cinemachine.Priority = 20;                   // 시네머신 카메라 우선순위 변경
        audioSource.Play();
        shortRay.Stop();
        longRay.Play();
        yield return new WaitForSeconds(3.0f);

        //스테이지 관련 셋팅 
        SpaceSurvival_GameManager.Instance.CurrentStage = currentStage;             // 이동할 스테이지 셋팅
        LoadingScene.SceneLoading(EnumList.SceneName.TestBattleMap);                // 씬 로딩
    }

    /// <summary>
    /// 해당 함수 실행시켜서 해당 스테이지가 클리어 됬는지 체크한다.
    /// </summary>
    /// <returns></returns>
    private bool IsStageClear()
    {
        return (SpaceSurvival_GameManager.Instance.StageClear & currentStage) > 0;
    }

    IEnumerator ClearLightVisible()
    {
        float value = 0.2705882f * Time.fixedDeltaTime * 0.75f;
        float alpha = 0.0f;
        while (clearRay.GetColor("_TintColor").a < 0.2705882f)
        {
            alpha += value;
            clearRay.SetColor("_TintColor", new Color(0.09077621f, 0.5367647f, 0.4444911f, alpha));
            yield return null;
        }
        prop.SetActive(true);
    }
}
