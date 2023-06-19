using System;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System.Collections.Generic;

//����ȭ �ȵǴ�����
//1. MonoBehaviour �� ��ӹ����� ����ȭ �� �Ҽ����� 
//2. [Serializable] �Ӽ�(Attribute) �� Ŭ������ ��������� 
//
// -  Ŭ���� ������ private �� �����Ͽ����� [SerializeField] �� ������� JsonUtil���� ������ �����ϴ�.
//    �̱��濡 ���� Ŭ����Ÿ���� ���׸����� �����Ҽ�����.  gameObject.AddComponent<T>(); addComponent ���� ���׸�Ŭ������ ó���Ҽ�������
/// <summary>
/// 
/// ���� �ε� ���� ���̽� ���� �ۼ��� Ŭ���� 
/// try{}cahch(Exeption e){} �� ����������� ���������� ���ӻ󿡼� �ܵ����� �̷������ ����̰� 
/// ���⼭ ���������ٰ� ������ ���߸� �ȵǱ⶧���� �����߻��ϴ��� �������ʰ� �÷��� �ǵ��� �߰��Ͽ���.
/// </summary>
public class SaveLoadManager : Singleton<SaveLoadManager> {

    /// <summary>
    /// �������� ��ġ 
    /// project ���� �ٷιؿ����� Assets������ ������ġ
    /// </summary>
    string saveFileDirectoryPath;
    public String SaveFileDirectoryPath => saveFileDirectoryPath;


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
    /// Ǯ���� ����������� SaveData������Ʈ�� �ѱ������ ����ȭ�������� 
    /// </summary>
    GameObject saveLoadWindow;
    public GameObject SaveLoadWindow => saveLoadWindow;


    /// <summary>
    /// ��������� ����ȭ�鵵 ��ȯ�� �Ǿ������� ��������������صξ���.
    /// </summary>
    public Action<JsonGameData,int> readAgain;

    /// <summary>
    /// ������ƮǮ �Ⱦ��� �׳� �����ͷθ� ������ ����.
    /// </summary>
    JsonGameData[] saveDataList;
    public JsonGameData[] SaveDataList => saveDataList;


    /// <summary>
    /// �������� �ִ밹�� 
    /// </summary>
    int maxSaveDataLength = 100;
    public int MaxSaveDataLength => maxSaveDataLength;


    /// <summary>
    /// ���丮 �ִ��� Ȯ���ϴ� ����
    /// </summary>
    public bool isDirectory = false; 

    /// <summary>
    /// ���ӽ��۽� �������� �ε��� ����� �̷�������� Ȯ���ϴ� ����
    /// </summary>
    public bool isFilesLoading = false;
    
    /// <summary>
    /// ��������ȭ�� ������Ʈ ��ġ�� ã�ƿ��� 
    /// ��������� ������ġ�� �����Ѵ�.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        //��������ȭ����ġ
        saveLoadWindow = GameObject.FindGameObjectWithTag("WindowList").transform. 
                                GetChild(0).            //OptionsWindow
                                GetChild(0).            //OptionSettingWindow
                                GetChild(1).            //SaveLoadWindow
                                GetChild(1).            //SaveFileList
                                GetChild(0).            //Scroll View
                                GetChild(0).            //Viewport
                                GetChild(0).gameObject; //Content

    }
    //ȭ���̵��� awake�� ���ο� �̱��水ä�� ���� ����Ǳ⶧���� ���ſ� �۾��� Start �λ���.
    void Start (){
        setDirectoryPath(); // ���� �����������ּҰ�����
        isDirectory = ExistsSaveDirectory();//����üũ�� ������ ���� 
        Debug.Log($"�񵿱� �ε��׽�Ʈ ���� {saveDataList}");
        if (!isFilesLoading)
        {
            FileListLoadind();//�񵿱�� ���Ϸε�
            //isFilesLoading = SetSaveFileList();
        }
        Debug.Log($"�񵿱� �ε��׽�Ʈ Ȯ��2�� {saveDataList}"); // �������̸� �̸޼����� ��Ʒ��� �����Ѵ� ������ �񵿱�� �ٸ���.
    
    }
  
    /// <summary>
    /// �񵿱� �Լ� �׽�Ʈ�� �����غ��Ѵ�
    /// �񵿱�μ����� async �Լ��� ��밡���� Thread �ϳ��� �� �Լ��������Ѵ�.
    /// ���α׷� �ȿ��� ������⶧���� ���α׷��� �����ֱ⸦ �����Ѵ� �Լ�����ó�����ȉ����� ���α׷�������Ǹ� ���� �������.
    /// </summary>
    async void FileListLoadind() //�񵿱� �׽�Ʈ
    {

        Debug.Log($"�񵿱� �ε��׽�Ʈ Ȯ��1�� {saveDataList}");

        await TestAsyncFunction(); //���Լ��� ���������� ��ٸ���.
        //await Task.Run(() => { isFilesLoading =  SetSaveFileList(); }); //���Լ��� ���������� ��� 

    }

    /// <summary>
    /// �񵿱� �Լ� �׽�Ʈ 
    /// ���������� �񵿱�� �ϱ����� �׽�Ʈ �ڵ��ۼ�
    /// �ڷ�ƾ�� ��������� �⺻������ ������ �ٸ���.
    /// </summary>
    async Task TestAsyncFunction() {
        Debug.Log("�񵿱� �׽�Ʈ�Լ� ����");
        await Task.Run(() => { isFilesLoading = SetSaveFileList(); }); //���Լ��� ���������� ��� 
        Debug.Log("�񵿱� �׽�Ʈ�Լ� ����");
        await Task.Delay( 3000 ); //3�� ��ٸ���

    }

    /// <summary>
    /// �������� ��ġ�� �����̸��� Ȯ���� ������ �����´�.
    /// </summary>
    /// <param name="index">�������� ��ȣ</param>
    /// <returns>������ ���� ��ġ�� ���ϸ�,Ȯ���ڸ���ȯ�Ѵ�</returns>
    private string GetFileInfo(int index) {
        //�����̸� �ڿ� ������ ����("D3")�� 001 �̷��������� �ٲ㼭 �����Ѵ�. 3�� ����ǥ���ڸ����̴�
        return $"{saveFileDirectoryPath}{saveFileDefaultName}{index.ToString("D3")}{fileExpansionName}"; 
    }
   

    /// <summary>
    /// ����� ������ġ�� ����
    /// </summary>
    private void setDirectoryPath()
    {
        //����Ƽ �����Ͱ��ƴ� ����ȯ���� Applicaion���� �ڵ����� ������������ش�. 
    #if UNITY_EDITOR //����Ƽ �����Ϳ������� ����
        saveFileDirectoryPath = $"{Application.dataPath}/SaveFile/";
        //Application.dataPath ��Ÿ�Ӷ� �����ȴ�.
    #else //����Ƽ�����Ͱ� �ƴҶ��� ���� 
        saveFileDirectoryPath = Application.persistentDataPath + "/SaveFile/"; //����Ƽ �����Ͱ��ƴ� ����ȯ���� Applicaion���� �ڵ����� ������������ش�. 
    #endif

    }


    /// <summary>
    /// ��������� �⺻������ �Է��Ѵ�.
    /// </summary>
    /// <param name="saveData">���� ����</param>
    /// <param name="index">���� ���Ϲ�ȣ</param>
    private void setDefaultInfo(JsonGameData saveData, int index) {
        saveData.DataIndex = index;  ///�����ε��� ����
        saveData.SaveTime = DateTime.Now.ToString();// ����Ǵ� �ð��� �����Ѵ�
        saveData.SceanName = SceneManager.GetActiveScene().name; //���̸��� �����Ѵ�.
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
            setDefaultInfo(saveData, index);// ��������� �⺻������ �����Ѵ�.
            string toJsonData = JsonUtility.ToJson(saveData, true); //���嵥���͸� Json�������� ����ȭ �ϴ� �۾� ����Ƽ �⺻����  
            File.WriteAllText(GetFileInfo(index), toJsonData); //�����̾����� �������ش�.
            readAgain?.Invoke(saveData,index); //������ ȭ�鿡 ����� �����ͷ� ǥ���ϱ����� ����
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

    /// <summary> �׽�Ʈ�����ȵ�
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
                readAgain?.Invoke(null, index); //������ ȭ�鿡 ������ �����ͷ� ǥ���ϱ����� ����
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

    /// <summary> �׽�Ʈ�����ȵ�
    /// ù��° �ε������� �ش��ϴ� ������ �ι�° �ε������� �ش��ϴ� �������������� �����ִ±��
    /// ���� ���� ���� 
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

                readAgain?.Invoke(JsonUtility.FromJson<JsonGameData>(tempa), newIndex); //���ϳ����̹ٲ�� �ٽ�ȭ�鿡 �ٲﳻ�뺸����������� �߰�
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
    /// ���ӽ��۽� ����ȭ�� �����ٶ� ����� �����͵� ã�ƿ��� 
    /// List �����̾ƴ� �迭�������� �ۼ��Ͽ��� 
    /// ������ �迭�������� ������ ���缭 �˻��� ������ �̷������� �ϱ����ؼ���.
    /// </summary>
    public bool SetSaveFileList()
    {
        try
        {
            string[] jsonDataList = Directory.GetFiles(saveFileDirectoryPath,searchPattern); // �����ȿ� ���ϵ� ������ �о 
            if (jsonDataList.Length == 0) //�о�������̾����� ����~
            {
                Debug.LogWarning($"������ ���� ������ �����ϴ� ");
                return false;
            }
            // ����Ʈ ���� ����ȭ�鿡�� ���̺�â �ٲ𶧸��� �ð��� �ɸ����ɼ����ִ� ���������� �����ǵ��Եȴ�.  
            JsonGameData[] tempSaveDataList = new JsonGameData[maxSaveDataLength]; //�迭�� ó���� �����ѹ������ذ�ȴ�.

            int checkDumyCount = 0;
            
            for (int i = 0; i < tempSaveDataList.Length; i++) { //�������� ã�ƿ� ���ϰ�����ŭ �ݺ���������
                if (i > jsonDataList.Length) { // ������ ���̻� ���������ʴ°�� ����������.
                    break ;
                }
                int tempIndex = GetIndexString(jsonDataList[i - checkDumyCount]);//�߰��� ������� üũ�ϱ����� checkDumyCount�� �����Ѵ�.
                if (tempIndex == i) // ���� �ε����� ���������� ������ �����Ѵ� 
                {
                    string jsonData = File.ReadAllText(jsonDataList[i - checkDumyCount]); // ���Ͽ� �����ؼ� �����ͻ̾ƺ��� 
                    if (jsonData.Length == 0 || jsonData == null)
                    {
                        //�⺻������ ����Ǹ�ȵȴ�. �̰��� ����Ǹ� ������������ ���ϳ����̾��ٴ°��̴�   
                        Debug.LogWarning($"{i - checkDumyCount} ��° ���� ������ �����ϴ� ���ϳ��� : {jsonDataList[i - checkDumyCount]} ");
                    }
                    else
                    {
                        //���������� �ƴѰ�� �۾�����
                        JsonGameData temp = JsonUtility.FromJson<JsonGameData>(jsonData); // ��ƿ����ؼ� �Ľ��۾��� ���� 

                        if (temp == null) //�Ľ̾ȉ�����  
                        {
                            //�⺻������ ����Ǹ�ȵȴ�.  ��������� Ȯ���غ��ų� ���嵥���Ͱ� ���������� ����������ʾƼ��߻��Ѵ�.
                            Debug.LogWarning($"{i - checkDumyCount} ��° ���� ������ Json���� ��ȯ�Ҽ� �����ϴ�  ���ϳ��� : {jsonDataList[i - checkDumyCount]} ");
                        }
                        else
                        {
                            tempSaveDataList[i] = temp;//�����ó���Ǹ� �����Ѵ�.
                            
                        }
                    }
                }
                else
                {
                    checkDumyCount++;// �����ε�����ȣ���߱����� ��ã���� ���̰��߰�
                }
            }

            //�ʱ�ε�(���ӽ���)�ÿ� ó���ǰ��س��� ���ݽð��̰ɸ����۾��̶� �ش������� �����Ͽ���.
            saveDataList = tempSaveDataList;
            
            return true;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return false;
        }
    }

    /// <summary>
    /// ���ϸ������� ī������ ���� 3�ڸ� ��������
    /// </summary>
    /// <param name="findText">�������Ͻ����ּҰ��� �̸� Ȯ���ڸ����</param>
    /// <returns>���ϸ��� �ε�������� ���ڸ� �̾Ƽ���ȯ -1�̸� ���Ͼ���</returns>
    public int GetIndexString(string findText)
    {
        if (findText != null)
        {
            int findDotPoint = findText.IndexOf('.'); //������ ***.json �̰� .�� �ϳ��ۿ������������� �ϴ� .��ġã�´�
            int temp = int.Parse(findText.Substring(findDotPoint-3, 3));//����ġ���� 3ĭ������ �̵��� 3���� ���� �������� ���� 3�ڸ������ü��ִ�.
            return temp;
        }
        else {
            return -1;
        }
    }


    /// <summary>
    /// �׽���Ŭ������ �׽�Ʈ�����ͻ���
    /// </summary>
    public void TestCreateFile() {

        for (int i = 0; i < maxSaveDataLength; i++)
        {
            TestSaveData<string> jb = new TestSaveData<string>();
            jb.TestFunc();
            setDefaultInfo(jb, i);// ��������� �⺻������ �����Ѵ�.
            string temp = JsonUtility.ToJson(jb);

            File.WriteAllText(GetFileInfo(i),temp);
            
        }
    }

    public void TestReLoad(int i) {
        readAgain?.Invoke(saveDataList[i],i);
    }
}
