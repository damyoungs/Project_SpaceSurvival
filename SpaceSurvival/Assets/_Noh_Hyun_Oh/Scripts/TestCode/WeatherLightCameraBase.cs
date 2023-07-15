using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 공부중
/// </summary>
public class WeatherLightCameraBase : MonoBehaviour
{
    [Header("메인카메라 관련 셋팅")]

    /// <summary>
    /// PC캐릭터 오브젝트 위치값 찾기위해 가져온다. 
    /// </summary>
    [SerializeField]
    private GameObject player = null;

    /// <summary>
    /// 메인카메라 오브젝트 
    /// </summary>
    [SerializeField]
    protected GameObject mainCamera = null;
    
    /// <summary>
    /// 태양 빛  오브젝트
    /// </summary>
    [SerializeField]
    protected GameObject DirectionalLight = null;

    /// <summary>
    /// 캐릭터와의 높이 간격
    /// </summary>
    [SerializeField]
    private float cameraIntervalY = 10.0f;
    
    public float CameraIntervalY
    {
        get => cameraIntervalY;

        protected set
        {
            cameraIntervalY = value;
        }
    }
    /// <summary>
    /// 카메라 쿼터뷰를 유지할수있게하는 카메라 각도
    /// </summary>
    [SerializeField]
    private float cameraAngle = 40.0f;
    public float CameraAngle
    {
        get => cameraAngle;
        protected set
        {
            cameraAngle = value;
        }
    }
    /// <summary>
    /// 카메라와  캐릭터와의 거리 간격
    /// </summary>
    [SerializeField]
    private float cameraIntervalZ = -10.0f;
    public float CameraIntervalZ
    {
        get => cameraIntervalZ;
        protected set
        {
            cameraIntervalZ = value;
        }
    }
   
    //[Header("태양빛 관련 셋팅")]
    //private float test1 = 0.0f;

    //[Header("날씨 관련 셋팅")]
    //[SerializeField]
    //private float test2 = 0.0f;
    /// <summary>
    /// 오브젝트없으면 태그로 찾아본다.
    /// 없어도되는코드.
    /// </summary>
    private void Awake()
    {
        if (player == null) { 
            player = GameObject.FindWithTag("Player");
        }
        if (mainCamera == null)
        {
            mainCamera = GameObject.FindWithTag("MainCamera").gameObject;
        }
        if (DirectionalLight == null) 
        { 
            DirectionalLight = GameObject.FindWithTag("Light").gameObject;
        }
        Debug.Log($"player = {player} \n mainCamera = {mainCamera}  DirectionalLight = {DirectionalLight}");
        
    }
    /// <summary>
    /// 플레이어가 있으면 플레이어 기준으로 카메라위치잡거나 
    /// 없으면 임의위치를 잡는다.
    /// </summary>
    private void Start()
    {
        if (player != null)
        {
            mainCamera.transform.Rotate(new Vector3(CameraAngle, 0, 0)); 
            MoveActionChangeWLC(player.transform.position);
        }
        else
        {
            DirectionalLight.transform.Translate(new Vector3(0, cameraIntervalY, 0));
            //카메라의 기준점을 잡을필요가 있다.
            mainCamera.transform.Translate(new Vector3(0, cameraIntervalY, cameraIntervalZ));
            mainCamera.transform.Rotate(new Vector3(40, 0, 0));
            //DirectionalLight.transform.Rotate(new Vector3(40, 0, 0));
        }

    }

    public virtual void CameraAngleSetting(GameObject mainCamera) {
        
    }

    /// <summary>
    /// 캐릭터의 움직임이 있을때마다 카메라도 따라간다
    /// 캐릭터가 바라보는 방향이 어느쪽인지 찾을필요가있음
    /// 카메라 이동이 자연스럽게 이동시키는 로직필요
    /// </summary>
    /// <param name="playerPosition">캐릭터의 위치</param>
    public virtual void MoveActionChangeWLC(Vector3 playerPosition) {
        //캐릭터 기준으로 얼마나 떨어지고 어느쪽에서 바라보고있는지에대한 설정값은 해당클래스안에서 정의한다.
        mainCamera.transform.position = playerPosition + new Vector3(
                                               0,
                                               cameraIntervalY,
                                               cameraIntervalZ
                                               );
        Mathf.Sin(transform.position.x);
        //DirectionalLight.transform.Rotate();
        Debug.Log($"main : {mainCamera.transform.position}");
    }
}
