using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cho_EndingScene : MonoBehaviour
{
    public Transform endingEffect;

    ParticleSystem particle;
    AudioSource audioSource;

    private void Awake()
    {
        particle = endingEffect.GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            endingEffect.gameObject.SetActive(true);
            particle.Play();
            audioSource.Play();
            StartCoroutine(Effect());
            StartCoroutine(EndSceneLoading());
        }
    }

    IEnumerator Effect()
    {
        Vector3 value = 0.5f * Time.deltaTime * new Vector3(1, 1, 1);
        while (endingEffect.localScale.x < 20.0f)
        {
            endingEffect.localScale += value;
            yield return null;
        }
    }

    IEnumerator EndSceneLoading()
    {
        yield return new WaitForSeconds(1.5f);
        WindowList.Instance.EndingCutImageFunc.EndingCutScene();
    }
}
