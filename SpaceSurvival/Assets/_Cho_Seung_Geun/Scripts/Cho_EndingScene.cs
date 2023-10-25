using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cho_EndingScene : MonoBehaviour
{
    public Transform endingEffect;          // �� �Ѿ�� ���� ������ ����Ʈ
    ParticleSystem particle;                // ���� ��ƼŬ
    AudioSource audioSource;

    /// <summary>
    /// ����Ʈ ��ٸ��� �ð�
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
        if (other.CompareTag("Player"))                 // ���� �ȿ� �÷��̾ ������
        {
            endingEffect.gameObject.SetActive(true);    // ����Ʈ�� �Ѱ�
            particle.Play();                            // ��ƼŬ ���
            audioSource.Play();                         // �Ҹ� ���
            StartCoroutine(Effect());                   // ����Ʈ �ڷ�ƾ ����
        }
    }

    IEnumerator Effect()
    {
        Vector3 value = 0.5f * Time.deltaTime * new Vector3(1, 1, 1);
        while (endingEffect.localScale.x < waitEffect)
        {
            endingEffect.localScale += value;           // ����Ʈ ũ�� Ű���
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
