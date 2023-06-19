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
    protected void OnLeftClick(InputAction.CallbackContext context)
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
    public static int fileIndex = -1;
    /// <summary>
    /// �׽�Ʈ ������ ����
    /// </summary>
    TestSaveData<string> saveData;
    public void SaveTest()
    {
        if (fileIndex > -1) {
            index = fileIndex;
        }
        saveData = new TestSaveData<string>();
        saveData.TestFunc(); //�������迭? ����ü���� �׽�Ʈ�� �ֱ�
        SaveLoadManager.Instance.Json_Save(saveData, index);
    }
    public void LoadTest()
    {
        if (fileIndex > -1)
        {
            index = fileIndex;
        }
        SaveLoadManager.Instance.Json_Load(saveData, index);

    }
    float minY = 200.0f;
    int minlength = 8;
    
    public void FileInfos() ///���Ͽ����� ���������� Ǯ�� ���θ��� ���� �о�
    {


        SaveLoadManager.Instance.TestCreateFile();
        //if (fileIndex > -1)
        //{
        //    index = fileIndex;
        //}
        //long money;
        //Match match = Regex.Match(toJsonData, @"""money""\s*:\s*(\d+)"); //�����Խ� �۵��Ѵ�.
        //if (match.Success)
        //{
        //    string moneyValue = match.Groups[1].Value;
        //    money = long.Parse(moneyValue);
        //}
        //saveData = new BaseSaveData<string>();
        //saveData.TestFunc();
        
        
        //for (int i = 0; i < minlength; i++)
        //{
        //    GameObject go = MultipleObjectsFactory.Instance.GetObject(EnumList.MultipleFactoryObjectList.SaveDataPool);
        //    go.GetComponent<RectTransform>().localPosition = new Vector3(678, -(i * minY) - 85.0f, 0);
        //    //go.transform.localPosition.Set(go.transform.localPosition.x, go.transform.localPosition.y - (i * minY), go.transform.localPosition.z);
        //    SaveData sd = go.GetComponent<SaveData>();
        //    sd.FileIndex = i;
        //    sd.SceanName = LoadingScean.SceanName;
        //}
        //x = -625   -1303
        //y = -90 -200

        
        //foreach (FileInfo saveFile in fi)
        //{

        //    Debug.Log($" ���ϸ� : {saveFile.Name}   ,  ������ ���� �ð� :{saveFile.LastWriteTime} "); 
        //}
        //string temp = SaveLoadManager.Instance.TestFileDataLoad(0);
    }
    //ȭ�鸸��� ���� �ִ��� �˻��ϴ±�� 
    // �ִ����ϸ���Ʈ�� ȭ�鿡 �����ִ� ���
    //���� �������
    public int fileIndexTest = -1;

    public void TitleLoad()
    {
        SaveLoadManager.Instance.TestReLoad(fileIndexTest);
        //LoadingScean.SceanLoading(EnumList.SceanName.Title);
        //WindowList.Instance.OptionsWindow.SetActive(false);
    }
    readonly int pageDataSize = 8; //���������� ������ ����
    
    bool isSaved = false;
    //����ȵ����� ĳ���� ����Ʈ

    public void SaveFileListInfo(int pageIndex = 0)
    {

        if (isSaved)
        {

            isSaved = false;
        }
        //}else if (sdc.saveDataList == null && sdc.fi == null)//�����о���°Ŷ� �ѹ����ϰ� �ʿ��Ѱ�� �����ϳ����ҷ�����
        //{ // ���ӽ����ϰ� ó�� ȣ��ǰų� �����������Ͽ� ����Ʈ������ �ʿ��Ѱ�� 
        //    sdc = SaveLoadManager.Instance.GetSaveFileList();
        //}
        
        
        //SaveLoadManager.Instance.SaveLoadWindow.transform;
    }




    public void SaveAction() {
        SaveTest();
    }
    public void LoadAction() {
        LoadTest();
    }
    public void CopyAction() {
        //�ε��� �ΰ� �������� �ͺ��� 
        //SaveLoadManager.Instance.Json_FileCopy();
    }
    public void DeleteAction() { 
        
    }
    public void OptionsAction() {
        FileInfos();
    }
    public void TitleAction() {
        LoadingScean.SceanLoading(EnumList.SceanName.Title ,EnumList.ProgressType.Bar);
        WindowList.Instance.OptionsWindow.SetActive(false);
    }





}
//long money;
//Match match = Regex.Match(saveJsonData, @"""money""\s*:\s*(\d+)");
//if (match.Success)
//{
//    string moneyValue = match.Groups[1].Value;
//    money = long.Parse(moneyValue);
//}
