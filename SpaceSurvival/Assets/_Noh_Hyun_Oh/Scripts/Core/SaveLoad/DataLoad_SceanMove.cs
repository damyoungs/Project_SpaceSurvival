using StructList;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoad_SceanMove : MonoBehaviour
{
    SlotManager slotManager;
    private void Awake()
    {
        slotManager = FindObjectOfType<SlotManager>(true);
    }
    private void Start()
    {
        SaveLoadManager.Instance.loadedSceanMove = FileLoadAction;
    }
    /// <summary>
    /// 로드 눌렀을때 화면이동과 데이터 셋팅 함수
    /// </summary>
    /// <param name="data">로드된 데이터</param>
    private void FileLoadAction(JsonGameData data)
    {
        //여기에 파싱작업이필요하다 실제로사용되는 작업
        if (data != null)
        {
            SaveLoadManager.Instance.ParsingProcess.LoadParsing(data);
            Debug.Log($"{data} 파일이 정상로드됬습니다 , {data.SceanName} 파싱작업후 맵이동 작성을 해야하니 맵이 필요합니다.");
            if (SpaceSurvival_GameManager.Instance.GetBattleMapInit != null) //배틀맵데이터가 셋팅되있으면
            {
                SpaceSurvival_GameManager.Instance.BattleMapInitClass.TestReset();  //초기화 하기
            }
            LoadingScean.SceanLoading(data.SceanName);

        }
    }
 
}