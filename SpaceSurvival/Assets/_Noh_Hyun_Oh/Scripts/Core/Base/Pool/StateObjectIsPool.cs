using EnumList;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StateObjectIsPool : ObjectIsPool, IStateData
{
    /// <summary>
    /// 씬이동 또는 상태이상이 끝났을경우 풀로 돌릴수있도록 위치값 저장
    /// </summary>
    Transform poolTransform;
    public Transform PoolTransform 
    {
        protected get => poolTransform;
        set 
        {
            if (poolTransform == null) //풀위치는 처음 생성될때 한번만 적용  
            {
                poolTransform = value;
            }
        }
    }
    /// <summary>
    /// 상태이상의 종류
    /// </summary>
    StateType stateType;
    public StateType Type 
    {
        get => stateType;
        set 
        {
            stateType = value;
        }
    }


    /// <summary>
    /// 상태이상의 아이콘 이미지
    /// </summary>
    private Image icon; 
    public Image Icon => icon;

    /// <summary>
    /// 한턴당 감소되는 수치 
    /// </summary>
    private float reducedDuration = 0.0f;
    public float ReducedDuration { get => reducedDuration; set => reducedDuration = value; }
    /// <summary>
    /// 상태이상의 최대 지속시간
    /// </summary>
    private float maxDuation = 0.0f;
    public float MaxDuration { get => maxDuation; set => maxDuation =value; }
    /// <summary>
    /// 남은 지속시간
    /// </summary>
    private float currentDuration = 0.0f;
    public float CurrentDuration { 
        get => currentDuration;

        set 
        {
            currentDuration = value; 
            if (value < 0.0f) //지속시간이 다되면
            {
                InitValue();//초기화

            }
        }
    }


    /// <summary>
    /// 초기화 작업
    /// </summary>
    public void InitValue() 
    {
        //상태값 초기화하고 
        Type = StateType.None;
        ReducedDuration = -1.0f;
        maxDuation = -1.0f;
        currentDuration = -1.0f;
        gameObject.SetActive(false); //큐에 다시 넣기위해 비활성화 
        transform.SetParent(poolTransform); //풀로 돌린다.
        /*
         transform.SetParent 함수는 객체가 활성화 비활성화 교체되는시점에서 사용될경우 오류가난다.
         그래서 활성화체크를하여 SetParent 를 실행한다.
         에러확인 : 오브젝트가 활성화 상태일때 gameobject.SetActive(false)실행후 바로  transform.SetParent(부모) 를 실행하니 터진다.
         */
    }

   
}
