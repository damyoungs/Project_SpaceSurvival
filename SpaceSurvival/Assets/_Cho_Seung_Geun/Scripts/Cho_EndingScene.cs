using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cho_EndingScene : MonoBehaviour
{
    public Transform endingEffect;          // 씬 넘어가기 전에 나오는 이펙트
    ParticleSystem particle;                // 위의 파티클
    AudioSource audioSource;

    /// <summary>
    /// 이펙트 기다리는 시간
    /// </summary>
    [SerializeField]
    float waitEffect = 2.0f;

    private void Awake()
    {
        particle = endingEffect.GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))                 // 영역 안에 플레이어가 들어오면
        {
            endingEffect.gameObject.SetActive(true);    // 이펙트룰 켜고
            particle.Play();                            // 파티클 재생
            audioSource.Play();                         // 소리 재생
            StartCoroutine(Effect());                   // 이펙트 코루틴 실행
        }
    }

    IEnumerator Effect()
    {
        Vector3 value = 0.5f * Time.deltaTime * new Vector3(1, 1, 1);
        while (endingEffect.localScale.x < waitEffect)
        {
            endingEffect.localScale += value;           // 이펙트 크기 키우기
            yield return null;
        }
        yield return EndSceneLoading();
    }

    IEnumerator EndSceneLoading()
    {
        yield return null;
        WindowList.Instance.EndingCutImageFunc.EndingCutScene();
    }
}
