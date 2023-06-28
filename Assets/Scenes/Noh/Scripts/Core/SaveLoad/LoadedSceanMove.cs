using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadedSceanMove : MonoBehaviour
{
    private void Start()
    {
        SaveLoadManager.Instance.loadedSceanMove += FileLoadAction;
    }

    /// <summary>
    /// 로드 눌렀을때 화면이동과 데이터 셋팅 함수
    /// </summary>
    /// <param name="data">로드된 데이터</param>
    private void FileLoadAction(JsonGameData data)
    {
        //여기에 파싱작업이필요하다 실제로사용되는 작업
        Debug.Log($"파싱작업해야됩니다 : {data} 이걸로");
        if (data != null)
        {
            Debug.Log($"{data} 파일이 정상로드됬습니다 , {data.SceanName} 파싱작업후 맵이동 작성을 해야하니 맵이 필요합니다.");
            LoadingScean.SceanLoading(data.SceanName);
        }
    }

}