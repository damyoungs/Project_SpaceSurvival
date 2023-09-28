using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangarDoor : MonoBehaviour
{
    public float speed = 4.0f;
    public Transform door;

    float openHeight = 0.0f;
    float closeHeight = 0.0f;

    private void Awake()
    {
        openHeight = door.position.y + 3.0f;
        closeHeight = door.position.y;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            StartCoroutine(Open());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            StartCoroutine(Close());
        }
    }

    IEnumerator Open()
    {
        while (door.position.y < openHeight)
        {
            door.position += new Vector3(0.0f, Time.deltaTime * speed, 0.0f);
            yield return null;
        }
    }

    IEnumerator Close()
    {
        while (door.position.y > closeHeight)
        {
            door.position -= new Vector3(0.0f, Time.deltaTime * speed, 0.0f);
            yield return null;
        }
    }
}