using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    public float speed = 2.0f;
    public Transform Icon;
    public Transform Icon2;

    Transform door;

    private void Awake()
    {
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
        while (door.position.y < 3)
        {
            door.position += new Vector3(0.0f, Time.deltaTime * speed, 0.0f);
            yield return null;
        }
    }

    IEnumerator Close()
    {
        while (door.position.y > 0)
        {
            door.position -= new Vector3(0.0f, Time.deltaTime * speed, 0.0f);
            yield return null;
        }
    }
}
