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
        if (SaveLoadPopupWindow.Instance.NewIndex > -1 &&  //���� ���� �ְų�
            SaveLoadManager.Instance?.SaveDataList[SaveLoadPopupWindow.Instance.NewIndex] != null) //�����Ѱ��� �����Ͱ� ������
        {
            SaveLoadPopupWindow.Instance.CopyCheck = true; //ī�� �÷��� ��

            Debug.Log("����� ��ġ�� Ŭ���ϼ���");
        }
        else {

            Debug.Log("������ ������ Ŭ���ϼ���");

        }
    }

    public void DeleteAction()
    {
        SaveLoadPopupWindow.Instance.OpenPopupAction(EnumList.SaveLoadButtonList.DELETE);
       
    }

    public void OptionsAction()
    {
        TestSaveData<string> testData = new();//�׽�Ʈ ������ ����
        testData.TestFunc(); //���߰�
        testData.SetSaveData();//�̰͵����߰�
        SaveLoadManager.Instance.GameSaveData = testData; //���嵥���Ϳ� �ֱ�
    }

    public void TitleAction()
    {
        LoadingScean.SceanLoading(EnumList.SceanName.TITLE);
    }

    

}