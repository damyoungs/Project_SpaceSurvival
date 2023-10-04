using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HingedDoor : MonoBehaviour
{
    enum DoorState
    {
        Close,
        Forward,
        Back
    }

    public float speed = 4.0f;

    //float rotateDir = 0.0f;
    //float localRotation = 0.0f;

    Transform door;
    //MeshCollider doorCollider;
    Animator animator;
    Cho_PlayerMove player;
    InteractionUI interactionUI;
    AudioSource audioSource;

    DoorState doorState = DoorState.Close;

    private void Awake()
    {
        door = transform.parent.GetChild(0);
        //doorCollider = door.GetComponent<MeshCollider>();
        animator = door.GetComponent<Animator>();
        player = FindObjectOfType<Cho_PlayerMove>();
        interactionUI = FindObjectOfType<InteractionUI>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //other.ClosestPoint(transform.position);

            //if (other.transform.position.z - transform.position.z > 0)
            //{
            //    rotateDir = 1.0f;
            //}
            //else
            //{
            //    rotateDir = -1.0f;
            //}

            //StopAllCoroutines();
            //StartCoroutine(Open());

            player.interaction += OnInteract;
            interactionUI.visibleUI?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //StopAllCoroutines();
            //StartCoroutine(Close());
            //animator.SetBool("Forward", false);

            player.interaction = null;
            interactionUI.invisibleUI?.Invoke();
        }
    }

    private void OnInteract()
    {
        if (doorState == DoorState.Close)
        {
            Vector3 dir = player.transform.position - transform.position;
            float angle = Vector3.Angle(door.forward, dir);

            if (angle < 90.0f)
            {
                animator.SetInteger("DoorState", 2);                // 뒤로 열기
                doorState = DoorState.Back;
            }
            else
            {
                animator.SetInteger("DoorState", 1);                // 앞으로 열기
                doorState = DoorState.Forward;
            }

            audioSource.Play();
        }
        else
        {
            animator.SetInteger("DoorState", 0);
            doorState = DoorState.Close;
        }
    }

    //IEnumerator Open()
    //{
    //    doorCollider.enabled = false;
    //    while (door.localEulerAngles.y < 120.0f)
    //    {
    //        door.rotation *= Quaternion.Euler(0.0f, Time.deltaTime * speed * rotateDir, 0.0f);
    //        yield return null;
    //    }
    //    Debug.Log(door.localEulerAngles);
    //    doorCollider.enabled = true;
    //}
    //
    //IEnumerator Close()
    //{
    //    doorCollider.enabled = false;
    //    float rotate = Time.deltaTime * speed;
    //
    //    while (door.localEulerAngles.y > rotate)
    //    {
    //        door.rotation *= Quaternion.Euler(0.0f, -rotate, 0.0f);
    //        yield return null;
    //    }
    //    Debug.Log(door.localEulerAngles);
    //    door.localRotation = Quaternion.Euler(door.localEulerAngles.x, 0.0f, door.localEulerAngles.z);
    //    doorCollider.enabled = true;
    //}



}
