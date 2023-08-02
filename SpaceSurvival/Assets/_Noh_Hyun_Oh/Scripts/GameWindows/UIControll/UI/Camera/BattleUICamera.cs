using UnityEngine;


/// <summary>
/// 팀 아이콘위치에 보여줄 카메라 위치셋팅
/// </summary>
public class BattleUICamera : MonoBehaviour
{

    [SerializeField]
    int index = 0;
    public int Index => index;
    /// <summary>
    /// 캐릭터 추적 카메라
    /// </summary>
    Camera actionCam;
   
    /// <summary>
    /// 추적카메라 위치값 
    /// </summary>
    Vector3 camPosition = Vector3.zero;

    /// <summary>
    /// 추적당할 오브젝트
    /// </summary>
    private Transform target = null;
    public Transform TObj 
    {
        get => target;
        set 
        {
            target = value;
        }
    }

    /// <summary>
    /// 카메라와 목표간의 간격
    /// </summary>
    [SerializeField]
    [Range(-1.0f,1.0f)]
    float distance = 1.0f;
    
    /// <summary>
    /// 카메라가 목표를 바라보는 방향
    /// </summary>
    Vector3 dir;
    private void Awake()
    {
        actionCam = GetComponent<Camera>();
    }

   
    //IEnumerator FollowCamera()
    //{
    //    while (true)
    //    {

    //        if (TargetObject != null) 
    //        {
    //            TrackingValueSetting();
    //        }
    //        yield return null;

    //    }

    //}
    private void Update()
    {
       
        if (TObj != null) TrackingValueSetting();
    }
    /// <summary>
    /// 카메라가 특정 목표를 같은 x,y 선상의 z 값기준으로 바라보게만든다.
    /// </summary>
    private void TrackingValueSetting() 
    {
        camPosition = target.position; //타겟 위치가져오고
        camPosition.z += distance; // 거리수정하고 
        actionCam.transform.position = camPosition; //타겟 위치는 거리수정한만큼 잡고 

        dir = target.position - transform.position; //타겟 방향벡터 구하고
        dir.Normalize(); //방향값만 추출하고 
        actionCam.transform.rotation = Quaternion.LookRotation(dir,Vector3.up); //방향백터로 바라보게 만들기 y축기준으로 돌린다.
    }

}
