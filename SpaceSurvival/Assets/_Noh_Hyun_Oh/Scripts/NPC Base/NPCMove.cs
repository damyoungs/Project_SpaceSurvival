using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMove : MonoBehaviour
{
    /// <summary>
    /// �̵��� ��ġ��
    /// </summary>
    Transform[] wayPoints;

    /// <summary>
    /// �̵� �� ������Ʈ
    /// </summary>
    Transform moveObject;

    /// <summary>
    /// �̵� ��ǽ����ų ������Ʈ
    /// </summary>
    Animator moveAnimator;

    /// <summary>
    /// �̵� �ӵ�
    /// </summary>
    [SerializeField]
    float moveSpeed;


}
