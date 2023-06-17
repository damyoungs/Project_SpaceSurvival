using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TMPro;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

//����ȭ �ȵǴ�����
//1. MonoBehaviour �� ��ӹ����� ����ȭ �� �Ҽ����� 
//2. [Serializable] �Ӽ�(Attribute) �� Ŭ������ ��������� 
//
// -  Ŭ���� ������ private �� �����Ͽ����� [SerializeField] �� ������� JsonUtil���� ������ �����ϴ�.
/// <summary>
/// ���� �ε� ���� ���̽� ���� �ۼ��� Ŭ���� 
/// try{}cahch(Exeption e){} �� ����������� ���������� ���ӻ󿡼� �ܵ����� �̷������ ����̰� 
/// ���⼭ ���������ٰ� ������ ���߸� �ȵǱ⶧���� �����߻��ϴ��� �������ʰ� �÷��� �ǵ��� �߰��Ͽ���.
/// </summary>
public class SaveLoadManager : Singleton<SaveLoadManager> {

    /// <summary>
    /// �������� ��ġ 
    /// project ���� �ٷιؿ����� Assets������ ������ġ
    /// </summary>
    const string saveFileDirectoryPath = "SaveFile/";

    /// <summary>
    /// �⺻ ������ �����̸�
    /// </summary>
    const string saveFileDefaultName = "SpacePirateSave";

    /// <summary>
    /// ������ ���� Ȯ���
    /// </summary>
    const string fileExpansionName = ".json ";


    /// <summary>
    /// ���̺� ���ϸ� �о�������� ���� ���� 
    /// ? �� ���ڿ� �ϳ��� ���ϴ°��ε� �̰����ȴ�
    /// </summary>
    //const string searchPattern = "SpacePirateSave[0-9][0-9][0-9].json"; //���ȵ�     
    const string searchPattern = "SpacePirateSave???.json";


    /// <summary>
    /// �������� ��ġ�� �����̸��� Ȯ���� ������ �����´�.
    /// </summary>
    /// <param name="index">�������� ��ȣ</param>
    /// <returns>������ ���� ��ġ�� ���ϸ�,Ȯ���ڸ���ȯ�Ѵ�</returns>
    private string GetFileInfo(int index) {
        //�����̸� �ڿ� ������ ����("D3")�� 001 �̷��������� �ٲ㼭 �����Ѵ�. 3�� ����ǥ���ڸ����̴�
        return saveFileDirectoryPath + saveFileDefaultName + index.ToString("D3") + fileExpansionName; 
    }

    /// <summary>
    /// ���嵥���� ���Ϸ� ���� �۾�
    /// </summary>
    /// <param name="saveData">���� ������ Ŭ����</param>
    /// <param name="index">�������� ��ȣ</param>
    /// <returns>�������� ��������</returns>
    public bool Json_Save(JsonGameData saveData, int index)
    {

        try
        {
            saveData.SaveTime = DateTime.Now.ToString(); //����ð� �����صα� 
            //ReflashSaveFileOne();
            string toJsonData = JsonUtility.ToJson(saveData, true); //���嵥���͸� Json�������� ����ȭ �ϴ� �۾� ����Ƽ �⺻����  
            
            File.WriteAllText(GetFileInfo(index), toJsonData); //�����̾����� �������ش�.

            return true;
        }
        catch (Exception e)
        {
            Debug.LogException(e); 
                                   
            return false;
        }
    }
   /// <summary>
   /// �������� �о���� �Լ� 
   /// </summary>
   /// <param name="saveData">���嵥���� Ŭ����</param>
   /// <param name="index">�������� ��ȣ</param>
   /// <returns>�о���� ��������</returns>
    public bool Json_Load(JsonGameData saveData, int index ) 
    {
        try 
        {
            if (File.Exists(GetFileInfo(index)))//������������ ������ �ִ��� üũ 
            {
                string saveJsonData = File.ReadAllText(GetFileInfo(index)); //���������� ��Ʈ������ �о���� 

                //������ ����. ���߹迭 �����ȵǰ�, private �ɹ������� �����Ϸ���  [SerializeField] �����ؾ���. Ŭ������ [Serializable] ����Ǿ���.
                saveData = JsonUtility.FromJson<JsonGameData>(saveJsonData); //��ȯ�� ��Ʈ�� �����͸� JsonUtility ���������� ����ȭó���� Ŭ������ �ڵ��Ľ� 
                return true;
            }
            else
            {
                
                
                Debug.LogWarning($"{GetFileInfo(index)} �ش���ġ�� ������ �������� �ʽ��ϴ�.");
            }
            return false;
        }
        catch(Exception e)
        {
            Debug.LogException(e); // ����ġ ���� ���� �߻��� �����߰߸���. Ŭ���� ������ �߸��߸� ��ü�� �ߵȴ�.
            return false;
        }
    }

    /// <summary>
    /// ����� ���� ���� 
    /// </summary>
    /// <param name="index">�������� ��ȣ</param>
    /// <returns>���ϻ��� ��������</returns>
    public bool Json_FileDelete(int index) 
    {
        try {
        
            if (File.Exists(GetFileInfo(index)))//�����ִ��� Ȯ�� 
            {
                File.Delete(GetFileInfo(index));//������ ���� 
                return true;
            }
            else
            {
                Debug.LogWarning($"{GetFileInfo(index)} �ش���ġ�� ������ �������� �ʽ��ϴ�.");
                return false;           
            }
        }catch(Exception e)
        {
            //���� IO������ �˼����� ������ �߻��Ҽ� ������ �ϴ� �ɾ�д�.
            Debug.LogException (e);
            return false;
        }
    }

    /// <summary>
    /// ù��° ���ڰ��� �ش��ϴ� ���Ͽ� �ι�° ���ڰ��� �ش��ϴ� ������������ �����ִ±��
    /// �������� ���� �Ҷ� ����� ����
    /// </summary>
    /// <param name="oldIndex">������ ���Ϲ�ȣ</param>
    /// <param name="newIndex">����� ���Ϲ�ȣ</param>
    /// <returns>�����ϱ� ��������</returns>
    public bool Json_FileCopy(int oldIndex , int newIndex) 
    {
        try {
            string oldFullFilePathAndName = GetFileInfo(oldIndex);// ������ ������ġ
            string newFullFilePathAndName = GetFileInfo(newIndex);// ����� ������ġ
            if (File.Exists(oldFullFilePathAndName))//������ ������ġ�� �������ִ��� Ȯ��
            {
                if (!File.Exists(newFullFilePathAndName))//����� ���� ��ġ�� �������ִ��� Ȯ��
                {
                    File.Create(newFullFilePathAndName);//������ ����  �������ҽ� ���Ͽ����߻�
                }
                string tempa = File.ReadAllText(oldFullFilePathAndName);//������ ������ �о����
                File.WriteAllText(newFullFilePathAndName,tempa);//����� ���Ͽ� �����߰�
                return true;
            }
            else
            {
               
                Debug.LogWarning($"{GetFileInfo(oldIndex)} �ش���ġ�� ������ �������� �ʽ��ϴ�.");
                return false;
            }
        }
        catch (Exception e) {
            Debug.LogException (e);
            return false;
        }


    }
   
    /// <summary>
    /// �������� �������� Ȯ���ϰ� ������ �����ϴ·���
    /// </summary>
    /// <returns>���� ���� ���� ��ȯ</returns>
    public bool ExistsSaveDirectory() {
        try {
            if (!Directory.Exists(saveFileDirectoryPath))//���� ��ġ�� ������ ������ 
            {
                Directory.CreateDirectory(saveFileDirectoryPath);//������ �����.
            }
            
            return true; //�⺻������ ����θ� �´�.
        }
        catch (Exception e) { 
            //��������� �������� �����𸣴� ��찡 ������������ �ϴ� �ɾ�д�.
            Debug.LogException(e);
            return false;
        }
    }


    /// <summary>
    /// ���������� �������ϸ���Ʈ�� �⺻������ �ҷ��´�
    /// ����ȭ�� �����ٶ� ����ҿ���
    /// </summary>
    /// <returns>�������� ����ƮFileInfo[]�� ��ȯ</returns>
    public FileInfo[] GetSaveFileList()
    {
        //pool�̿��Ͽ� ������ ����� ����
        try
        {
            DirectoryInfo di = new DirectoryInfo(saveFileDirectoryPath); //������ �������� �о�´�.

            FileInfo[] fi = di.GetFiles(searchPattern, SearchOption.TopDirectoryOnly); //�����ȿ� ������ �̿��Ͽ� Ư�����ϸ� ��� �迭�� �о�´�.
                                                                                       //SearchOption.TopDirectoryOnly �ɼǰ� SearchOption.AllDirectories
                                                                                       //�ΰ����ִµ� Top�� ���� �ٷξƷ����븸 �о���°��̰� 
                                                                                       //All�� �������� �������� ������ �о���°��̴�.
            return fi;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return null;
        }
    }

    /// <summary>
    /// ���� ����� ȭ�鿡 �����͸� �����ϱ����� �Լ�
    /// </summary>
    /// <param name="index">���� �ѹ�</param>
    public bool ReflashSaveFileOne(int index) {
        //������
        return false;
    }



    /// <summary>
    /// index ����ŭ ���ϸ����. �׽�Ʈ��
    /// </summary>
    /// <param name="index">�ִ� 999��������</param>
    public void TestcreateFiles(int index) {

        for (int i = 0; i < index; i++) {
            if (!File.Exists(GetFileInfo(i))) { 
                File.Create(GetFileInfo(i));
            }
        }
    }

    public string TestFileDataLoad(int i) {
        
        if (File.Exists(GetFileInfo(i))) {
            return File.ReadAllText(GetFileInfo(i));
            
        }
        return null;
    }
   
   
}
