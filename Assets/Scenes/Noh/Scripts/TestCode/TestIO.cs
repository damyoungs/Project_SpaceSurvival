using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.IO;
using UnityEngine.InputSystem;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using EnumList;

public class TestIO : MonoBehaviour

{


    const string testPath = "Assets/ImportPack/SoundFiles";
    const string saveFileDirectoryPath = "SaveFile/";
    const string fileExpansionName = ".sav";
    DirectoryInfo di;

    public int index = 0;
    const string soundPath = "/BGM/Piano Instrumental 1.wav";
    protected  void OnLeftClick(InputAction.CallbackContext context)
    {
        if (context.performed) {
            //���� ������ ����� ����
            if (!Directory.Exists(saveFileDirectoryPath))
            {
                Directory.CreateDirectory(saveFileDirectoryPath);
            }

            ////�������� �����׽�Ʈ
            //for (int i= 0; i < 100; i++) {
            //    if (!File.Exists(saveFileDirectoryPath + i + fileExpansionName)) {
            //        File.Create(saveFileDirectoryPath + i + fileExpansionName);
            //        Debug.Log($"{saveFileDirectoryPath + i + fileExpansionName} ���ϻ����Ϸ�");
            //    }
            //}
            //SaveLoadManager.Instance.Json_SaveFile(index);


            //SaveLoadManager.Instance.Json_LoadFile(index);


        }
    }
    /// <summary>
    /// �׽�Ʈ ������ ����
    /// </summary>
    BaseSaveData<string> saveData;
    public void SaveTest()
    {
        saveData = new BaseSaveData<string>();
        saveData.TestFunc(); //�������迭? ����ü���� �׽�Ʈ�� �ֱ�
        SaveLoadManager.Instance.Json_Save(saveData,index);   
    }
    public void LoadTest()
    {
        
        SaveLoadManager.Instance.Json_Load(saveData,index);
    }
    float minY = 200.0f;
    int minlength = 10;
    public void FileInfos() ///���Ͽ����� ���������� Ǯ�� ���θ��� ���� �о�
    {
        saveData = new BaseSaveData<string>();
        saveData.TestFunc();
        FileInfo[] fi = SaveLoadManager.Instance.GetSaveFileList();
        for (int i = 0; i < minlength; i++)
        {
            GameObject go = MultipleObjectsFactory.Instance.GetObject(EnumList.MultipleFactoryObjectList.SaveDataPool);
            go.GetComponent<RectTransform>().localPosition = new Vector3(678, - (i * minY) - 85.0f, 0);
            //go.transform.localPosition.Set(go.transform.localPosition.x, go.transform.localPosition.y - (i * minY), go.transform.localPosition.z);
            SaveData sd = go.GetComponent<SaveData>();
            sd.FileIndex = i;
            sd.CreateTime = fi[i].LastWriteTime.ToString();
            sd.Money = saveData.Money;
            sd.CharcterName = saveData.CharcterName;
            sd.SceanName = LoadingScean.SceanName;
        }
        //x = -625   -1303
        //y = -90 -200

        GameObject.FindGameObjectWithTag("SaveList").GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, minY * minlength);
        //foreach (FileInfo saveFile in fi)
        //{

        //    Debug.Log($" ���ϸ� : {saveFile.Name}   ,  ������ ���� �ð� :{saveFile.LastWriteTime} "); 
        //}
        //string temp = SaveLoadManager.Instance.TestFileDataLoad(0);
    }
    //ȭ�鸸��� ���� �ִ��� �˻��ϴ±�� 
    // �ִ����ϸ���Ʈ�� ȭ�鿡 �����ִ� ���
    //���� �������


    public void TitleLoad()
    {
        LoadingScean.SceanLoading(EnumList.SceanName.Title);
        WindowList.Instance.OptionsWindow.SetActive(false);
    }
}
