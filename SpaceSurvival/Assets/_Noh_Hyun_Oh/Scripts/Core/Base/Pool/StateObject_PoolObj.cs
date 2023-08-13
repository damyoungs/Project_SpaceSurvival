using EnumList;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StateObject_PoolObj : PoolObjBase, IStateData
{
    /// <summary>
    /// 씬이동 또는 상태이상이 끝났을경우 풀로 돌릴수있도록 위치값 저장
    /// </summary>
    public Transform PoolTransform => poolTransform;

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
    private Sprite icon; 
    public Sprite Icon => icon;

    [SerializeField]
    [Range(0.0f,1.0f)]
    /// <summary>
    /// 한턴당 감소되는 수치 
    /// </summary>
    private float reducedDuration = 0.5f;
    public float ReducedDuration { get => reducedDuration; set => reducedDuration = value; }
    [SerializeField]
    [Range(0.0f, 1.0f)]
    /// <summary>
    /// 상태이상의 최대 지속시간
    /// </summary>
    private float maxDuration = 1.0f;
    public float MaxDuration { get => maxDuration; set => maxDuration =value; }
    [SerializeField]
    [Range(0.0f, 1.0f)]
    /// <summary>
    /// 남은 지속시간
    /// </summary>
    private float currentDuration = 0.0f;
    /// <summary>
    /// 0에서 부터 증가하면서 MaxDuration 값보다 크게 되면 해제된다.
    /// </summary>
    public float CurrentDuration { 
        get => currentDuration;

        set 
        {
            if (currentDuration != value) //값이 변경됫으면
            {
                if (value > MaxDuration) //지속시간체크 다되면
                {
                    ResetData();//초기화 하고
                    return; //돌아간다.
                }

                // 지속시간 남아있는경우엔 
                if (value < 0.0f) // 0 이하로는 셋팅이 안되야함으로 체크
                {
                    currentDuration = 0.0f; 

                }
                else //정상적인값이면 
                {
                    currentDuration = value; //값변경하고 
                }

                FillAmoutSetting(currentDuration); //게이지 갱신
            }
            
        }
    }
    /// <summary>
    /// 게이지 보여줄 이미지
    /// </summary>
    Image gaugeImg;
    /// <summary>
    /// 나누기한번만하기위해 배율 미리계산할 변수
    /// </summary>
    float computationalScale = -1.0f;
    protected  override void Awake()
    {
        base.Awake();
        gaugeImg = GetComponent<Image>();

    }
    protected override void OnEnable()
    {
        gaugeImg.fillAmount = 0.0f;// 초기화 
        computationalScale = (MaxDuration / 1); //배율 미리구해두기
    }
    /// <summary>
    /// UI 조절할 함수 
    /// </summary>
    /// <param name="value">조절될 값</param>
    private void FillAmoutSetting(float value) 
    {
        gaugeImg.fillAmount = computationalScale * value; //미리구해둔 배율로 셋팅 
    }
    /// <summary>
    /// 초기화 작업
    /// </summary>
    public void ResetData() 
    {
        //상태값 초기화하고 
        Type = StateType.None; //상태이상 종류 초기화
        reducedDuration = -1.0f; //한턴당 진행될 값초기화 
        maxDuration = -1.0f; // 최대치 초기화
        currentDuration = -1.0f; //현재 진행값 초기화
        computationalScale = -1.0f;//배율 초기화
        gaugeImg.fillAmount = 1.0f;// 게이지 초기화 
        transform.SetParent(poolTransform); //풀로 돌린다.
        gameObject.SetActive(false); //큐에 다시 넣기위해 비활성화 
        /*
         transform.SetParent 함수는 객체가 활성화 비활성화 교체되는시점에서 사용될경우 오류가난다.
         그래서 활성화체크를하여 SetParent 를 실행한다.
         에러확인 : 오브젝트가 활성화 상태일때 gameobject.SetActive(false)실행후 바로  transform.SetParent(부모) 를 실행하니 터진다.
         */
    }

   
}
