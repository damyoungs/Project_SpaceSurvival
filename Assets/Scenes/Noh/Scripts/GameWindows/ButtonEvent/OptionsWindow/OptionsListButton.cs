using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class OptionsListButton : MonoBehaviour
{
    public void SaveAction() 
    {
        SaveLoadPopupWindow.Instance.OpenPopupAction(EnumList.SaveLoadButtonList.SAVE);
    }

    public void LoadAction()
    {
        SaveLoadPopupWindow.Instance.OpenPopupAction(EnumList.SaveLoadButtonList.LOAD);

    }

    public void CopyAction() 
    {
        if (SaveLoadPopupWindow.Instance.NewIndex > -1 &&  //선택 값이 있거나
            SaveLoadManager.Instance?.SaveDataList[SaveLoadPopupWindow.Instance.NewIndex] != null) //선택한값의 데이터가 있을때
        {
            SaveLoadPopupWindow.Instance.CopyCheck = true; //카피 플래그 온

            Debug.Log("복사될 위치를 클릭하세요");
        }
        else {

            Debug.Log("복사할 파일을 클릭하세요");

        }
    }

    public void DeleteAction()
    {
        SaveLoadPopupWindow.Instance.OpenPopupAction(EnumList.SaveLoadButtonList.DELETE);
       
    }

    public void OptionsAction()
    {
        TestSaveData<string> testData = new();//테스트 데이터 생성
        testData.TestFunc(); //값추가
        testData.SetSaveData();//이것도값추가
        SaveLoadManager.Instance.GameSaveData = testData; //저장데이터에 넣기
    }

    public void TitleAction()
    {
        LoadingScean.SceanLoading(EnumList.SceanName.TITLE);
    }

    

}
