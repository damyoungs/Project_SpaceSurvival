using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 타이틀에서사용할 버튼 로드화면 제작끝나면 
/// 테스트시 로딩화면에는 연결하지마세요 무한로딩됩니다..
/// </summary>
public class ContinueButton : MonoBehaviour
{
    [SerializeField]
    EnumList.SceanName sceanName;
    private void Awake()
    {
        sceanName = EnumList.SceanName.SAVELOADTEST;
    }
    public void OnClickContinue()
    {
        LoadingScean.SceanLoading(sceanName);
    }
}
