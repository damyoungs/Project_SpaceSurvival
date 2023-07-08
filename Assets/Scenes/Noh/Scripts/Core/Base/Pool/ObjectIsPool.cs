using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 풀에들어갈 기본 클래스
/// </summary>
public class ObjectIsPool : MonoBehaviour
{
    /// <summary>
    /// 비활성화시 Queue로 반환 처리가 이루어져야한다.
    /// 오브젝트 풀에 들어갈 오브젝트들이 상속받을 클래스 
    /// </summary>
    public Action onDisable;
    /// <summary>
    /// 위치초기화 할지안할지 결정하는 변수 
    /// </summary>
    protected bool isPositionReset = true; 
    protected virtual void OnEnable()
    {
        if (isPositionReset) 
        {
            //위치값 초기화 기본적으로 오브젝트설정할때 트랜스폼을 0,0,0 으로 설정해야한다.
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
    }
    /// <summary>
    /// 비활성화된 부모에 들어간경우 동작을안하여 큐를 초기화할수없다.
    /// </summary>
    protected virtual void OnDisable() 
    {
        onDisable?.Invoke(); //Queue 초기화한다.
    }

    /// <summary>
    /// 일정 시간 후에 이 게임오브젝트를 비활성화 시키는 코루틴
    /// </summary>
    /// <param name="delay">비활성화가 될때까지 걸리는 시간(기본 = 0.0f)</param>
    /// <returns></returns>
    protected virtual IEnumerator LifeOver(float delay = 0.0f)
    {
        yield return new WaitForSeconds(delay); // delay만큼 대기하고
        gameObject.SetActive(false);            // 게임 오브젝트 비활성화
    }

}
