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
        LoadingScene.SceneLoading(EnumList.SceneName.TestBattleMap);
    }
}
