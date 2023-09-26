using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HingedDoor : MonoBehaviour
{
    public float speed = 4.0f;

    float rotateDir = 0.0f;
    //float localRotation = 0.0f;

    Transform door;
    MeshCollider doorCollider;

    private void Awake()
    {
        door = transform.parent.GetChild(0);
        doorCollider = door.GetComponent<MeshCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //other.ClosestPoint(transform.position);

            if (other.transform.position.z - transform.position.z > 0)
            {
                rotateDir = 1.0f;
            }
            else
            {
                rotateDir = -1.0f;
            }

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
        doorCollider.enabled = false;
        while (door.localEulerAngles.y < 120.0f)
        {
            door.rotation *= Quaternion.Euler(0.0f, Time.deltaTime * speed * rotateDir, 0.0f);
            yield return null;
        }
        Debug.Log(door.localEulerAngles);
        doorCollider.enabled = true;
    }

    IEnumerator Close()
    {
        doorCollider.enabled = false;
        float rotate = Time.deltaTime * speed;

        while (door.localEulerAngles.y > rotate)
        {
            door.rotation *= Quaternion.Euler(0.0f, -rotate, 0.0f);
            yield return null;
        }
        Debug.Log(door.localEulerAngles);
        door.localRotation = Quaternion.Euler(door.localEulerAngles.x, 0.0f, door.localEulerAngles.z);
        doorCollider.enabled = true;
    }
}
