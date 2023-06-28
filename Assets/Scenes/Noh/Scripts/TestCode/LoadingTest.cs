using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 로딩이 자동으로 발동하는지 테스트하는 클래스 
/// </summary>
public class LoadingTest : MonoBehaviour
{
    private void Start() //로딩이되어 해당창으로 이동시 
    {
        StartCoroutine(returnTitle());
    }
    IEnumerator returnTitle() { 
        yield return new WaitForSeconds(1.0f);
        LoadingScean.SceanLoading(); //다시 타이틀로 이동시킨다.
    }
}
