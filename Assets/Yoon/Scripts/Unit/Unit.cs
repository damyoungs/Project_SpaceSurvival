using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    [SerializeField] private Animator unitAnimator; // 유닛 애니메이터

    /// <summary>
    /// 유닛이 이동할 타겟위치
    /// </summary>
    private Vector3 targetPosition;
    //private GridPosition gridPosition;

    
    private void Awake()
    {
        targetPosition = transform.position;
    }
    
    
    private void Start()
    {
    }
    private void Update()
    {


        float stoppingDistance = 0.1f;
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            float moveSpeed = 4.0f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            //  움직이는 방향으로 방향전환

            float rotateSpeed = 10.0f;
            // 움직이는 방향으로 전환하는데 Lerp메서드로 느리게 전환
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

            unitAnimator.SetBool("IsWalking", true);
        }
        else
        {
            unitAnimator.SetBool("IsWalking", false);
        }

    }
    public void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}
