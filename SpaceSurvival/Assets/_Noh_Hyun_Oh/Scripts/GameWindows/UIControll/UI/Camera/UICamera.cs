using EnumList;
using System.Collections;
using UnityEngine;


/// <summary>
/// UI 에서 사용될 캐릭터 따라다닐 카메라 
/// Texture 적용할 카메라
/// </summary>
public class UICamera : MonoBehaviour ,ICameraBase
{

    [SerializeField]

    EnumList.CameraTargetType cameraTarget = EnumList.CameraTargetType.Player;
    public CameraTargetType TargetType 
    {
        get => cameraTarget; 
        set => cameraTarget =value; 
    }
    

    /// <summary>
    /// 캐릭터 추적 카메라
    /// </summary>
    Camera actionCam = null;
    public Camera FollowCamera 
    { 
        set 
        { 
            actionCam = value; 
        } 
    }
   

    [SerializeField]
    /// <summary>
    /// 추적당할 오브젝트
    /// </summary>
    private Transform target = null;
    public Transform TargetObject 
    {
        get => target; 
        set => target = value; 
    }

    public Vector3 Distance 
    { 
        get => distance; 
        set => distance =value; 
    }

    public float FollowSpeed { get => 0.0f; set{ } }

    /// <summary>
    /// 카메라와 목표간의 간격
    ///  - 1 캐릭터 얼굴보기  1 캐릭터 뒤통수 보기
    /// </summary>
    [SerializeField]
    [Range(-1.0f,1.0f)]
    Vector3 distance = new(0.0f,0.0f,1.0f);
    
   
    private void Awake()
    {
        //gameObject.layer = LayerMask.NameToLayer("UI");///설정한 레이어 검색해서 번호 가져오기
        //gameObject.tag = "Respawn"; //태그 변경시키기
        actionCam = GetComponent<Camera>();
    }


    IEnumerator MoveCamera()
    {
        while (cameraTarget != EnumList.CameraTargetType.None)
        {
            TrackingValueSetting();
            yield return null;

        }
    }
    
    private void OnEnable()
    {
        StartCoroutine(MoveCamera());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// 카메라가 특정 목표를 같은 x,y 선상의 z 값기준으로 바라보게만든다.
    /// </summary>
    private void TrackingValueSetting() 
    {
        transform.position = target.transform.position // 목표 지점에서 
                            -(target.transform.forward * distance.z); // 목표의 정면방향에  distance 를 더한뒤  - 를 해서 바라보는방향 반대쪽으로 위치 시킨다.

        actionCam.transform.rotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up); //방향백터로 바라보게 만들기 y축기준으로 돌린다.
    }

}
