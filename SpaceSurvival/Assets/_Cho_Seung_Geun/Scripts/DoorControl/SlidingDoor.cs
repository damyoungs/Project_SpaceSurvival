using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    public float speed = 4.0f;
    public Transform Icon;
    public Transform Icon2;

    AudioSource audioSource;
    Transform door;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        door = transform.parent.GetChild(0);
        if (Icon != null)
        {
            Icon.SetParent(door);
        }
        if (Icon2 != null)
        {
            Icon2.SetParent(door);
        }

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player") || other.CompareTag("Npc"))
        {
            StopAllCoroutines();
            StartCoroutine(Open());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Npc"))
        {
            StopAllCoroutines();
            StartCoroutine(Close());
        }
    }

    IEnumerator Open()
    {
        audioSource.Play();
        while (door.position.y < 3)
        {
            door.position += new Vector3(0.0f, Time.deltaTime * speed, 0.0f);
            yield return null;
        }
    }

    IEnumerator Close()
    {
        audioSource?.Play();
        while (door.position.y > 0)
        {
            door.position -= new Vector3(0.0f, Time.deltaTime * speed, 0.0f);
            yield return null;
        }
    }
}
