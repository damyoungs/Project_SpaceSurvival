using System;
using System.IO;
using UnityEngine;



/// <summary>
/// ������ ������ �����͸� ���� �ϴ� Ŭ������ �����Ѵ� .
/// �ۼ���� 
/// ��
/// 1. ������ �⺻������ private �� ������ ������Ƽ�� �����Ѵ�. [�ʼ������� ĸ��ȭ]
///   1-1. ����ȭ�Ҷ� private �ϰ� �Ӽ� [SerializeField] �����°ų� public ���� �����ϴ°ų� �۾����� ���ٰ��Ѵ�. 
///         public�� ���������� ����ȭ �۾��� ���ļ� �����Ϳ��Ѹ��ٰ��Ѵ�. private �ϰ� [SerializeField] �ϴ°Ͱ� ����������� �̷������.
///         
/// 2. private �� ������ ������ �Ӽ��� [SerializeField] �� �����Ѵ�  - ����Ƽ �⺻ JsonUtility ���� ������ ����½� �����ϱ����� ���  
/// 
/// 3. ���߹迭(2�����迭�̻�)�� 1�����迭�� ����ü�� �ְ� ����ü�ȿ� �ٽ� 1�����迭�� ������ ¥�½����� ����ü�� �̿��Ͽ� ���߹迭�������� ����� ������ �ִ´�.
/// 3-1. ����ü�迭�� �ȵȴ�? �ƴ� �ȴ� ������ ������о�´�. 
/// 
/// 4. ��Ӱ����� �Ľ��� �ȵȴ�. �⺻������ �θ�A �� Ŭ������  jsonUtility �� �̿��ϸ� A ��ü�� �ڽ��ǳ������ �����Ѵ� ������ 
///         �ҷ��ö� jsonUtility�� �����͸� �޴� ����  Ŭ���������� �Ľ��ϱ⶧���� ������(�ڽ�Ŭ����������) ������ �߻��Ѵ�.
///    ��� : �������� �ϳ��� ó���Ϸ��� Ŭ�����ϳ����� ó���ؾ��Ѵ� ( ��� X )
///    - �ش系���� ���������� �����͸� ���������� �����̴�  , �޸𸮻󿡴� �ڽ��� �����ͱ��� ���� ���ִ�.
/// ex) struct A{B[] b; int i;}; struct B{C[] c; int a;};  struct C{int a;};  
/// [SerializeField]
/// A a ; 
/// public A A => a; 
/// 
/// 4. �����Ҷ� �ٲ����ʴ� ������ �����Ϸ��� SaveLoadManager.setDefaultInfo �Լ��� ���� �⺻������ ������Ƽ�� public ���� �����Ѵ�.
/// �׽�Ʈ ���
/// 1. �ش�Ŭ������ ��ӹ޾Ƽ� ����غ������� ��ӹ��� Ŭ������ �ɹ������� ������ �ǳ� �о�ö� ���� ����� �������� �ʴ´�. 
///  1-1. �ذ�: �Լ�ȣ��� ��ӹ��� Ŭ������ �ѱ�� ����� �Ľ��� �ȴ�. �ε��Ҷ��� ���� ��ü�� ����Ͽ����Ѵ�.
/// 2. JsonGameData �� ���̽��� ��ӹ��� Ŭ������   ����Ƽ �̱��� ��ü�� �����ϱ����� ���׷������� �־�f���� ���۳�Ʈ����� �̱����������������. 
///  2-1. �ش� ������ �ٸ�������� ������ �����ϰ��ְ� �׽�Ʈ���̴�.
///  
/// 3. ���� ť�� ����Ʈ ������ �׽�Ʈ�� ���غ��Ѵ�. => ť ����Ʈ ���� ���� �ڷᱸ���� ����ȭ �ϱ� ��Ʊ⶧���� �����ͷ� �ִ°� ����õ�̴�.
///  3-1 . ����Ƽ������ ����ȭ�� �������̸� ������ 10�ܰ�� �������ξ��µ� �̴� ��ȯ������ ������ �Ǽ� �׷����̴� 
///        ť�� �����ϴ� �ڷᱸ���� ���������ʰ� , ����Ʈ�ǰ�� ��ȯ������ �����ؼ� ������ ������ ����ε� ����ȭ�� �̷������.
///        ���ö��� �����ϴ� �ڷᱸ���δ� ���������ʴ�.
///        
///  ************* ĳ���� ���������ϰ� �ش�Ŭ������ ����Ͽ� �Ľ��Լ��� �����Ѵ�. 
///                 - �̶� ��ȯ������ �����ϰ� ����ؾ��ϰ� ��������(��ӵ�����)�� 10�ܰ�� �����ϱ⶧���� �����ؾ��Ѵ�.***************
///  
/// MonoBehaviour ��  [Serializable] �� ���������ʴ´�.
/// </summary>
///  
///����ȭ : ���������� �Ľ��۾��� �ʿ��ϴٰ� �Ѵ�. 
///
///PlayerPrefs �� ������ ������Ʈ���� ����ȴ�. �����Ͱ� ���µ��־ ����õ�̴�. �����ιٲܼ�������. ���ȿ� ��� 
///JsonGameData  a = new(); ���ĵ�����
///
[Serializable]
public class JsonGameData 
{
    //���嵥���� ĳ���ϱ����� �ε��� ��ȣ
    [SerializeField]
    int dataIndex;
    public int DataIndex {
        get => dataIndex;
        set{ 
            dataIndex = value;
        
        }
    }
    /// <summary>
    /// ����ð� �־�α� 
    /// </summary>
    [SerializeField]
    string saveTime;
    public string SaveTime { 
        get => saveTime;
        set { 
            saveTime = value;
        }
    }
    
    /// <summary>
    /// �ҷ������ ���� ������ 
    /// </summary>
    [SerializeField]
    EnumList.SceanName sceanName;
    public EnumList.SceanName SceanName
    {
        get => sceanName;
        set
        {
            sceanName = value;
        }
    }

    /// <summary>
    /// ĳ���� ������ ����
    /// </summary>
    [SerializeField]
    StructList.CharcterInfo[] charcterInfo;
    public StructList.CharcterInfo[] CharcterInfo
    {
        get => charcterInfo;
        protected set
        {
            charcterInfo = value;
        }
    }

    /// <summary>
    /// ĳ���� ���������� ����Ʈ
    /// </summary>
    [SerializeField]
    StructList.CharcterItems[] itemList;
    public StructList.CharcterItems[] ItemList { 
        get => itemList;
        protected set
        {
            itemList = value;
        }
    }

    /// <summary>
    /// ĳ���ͽ��� ��� ��������Ʈ
    /// </summary>
    [SerializeField]
    StructList.CharcterSkills[] skillList;
    public StructList.CharcterSkills[] SkillList
    {
        get => skillList;
        protected set
        {
            skillList = value;
        }
    }

    /// <summary>
    /// ĳ���� ����Ʈ���� ����Ʈ
    /// </summary>
    [SerializeField]
    StructList.CharcterQuest[] questList;
    public StructList.CharcterQuest[] QuestList
    {
        get => questList;
        protected set
        {
            questList = value;
        }
    }

  
}
