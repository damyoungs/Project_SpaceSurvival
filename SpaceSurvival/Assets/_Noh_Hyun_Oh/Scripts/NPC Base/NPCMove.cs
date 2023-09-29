using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMove : MonoBehaviour
{
    /// <summary>
    /// 이동할 위치들
    /// </summary>
    Transform[] wayPoints;

    /// <summary>
    /// 이동 할 오브젝트
    /// </summary>
    Transform moveObject;

    /// <summary>
    /// 이동 모션실행시킬 컴포넌트
    /// </summary>
    Animator moveAnimator;

    /// <summary>
    /// 이동 속도
    /// </summary>
    [SerializeField]
    float moveSpeed;


}
