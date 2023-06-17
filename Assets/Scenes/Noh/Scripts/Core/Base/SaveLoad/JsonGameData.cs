using System;
using System.IO;
using UnityEngine;

/// <summary>
/// ������ ������ �����͸� ���� �ϴ� Ŭ������ �����Ѵ� .
/// �ۼ���� 
/// 1. ������ �⺻������ private �� ������ ������Ƽ�� �����Ѵ�. [�ʼ������� ĸ��ȭ]
/// 2. private �� ������ ������ �Ӽ��� [SerializeField] �� �����Ѵ�  - ����Ƽ �⺻ JsonUtility ���� ������ ����½� �����ϱ����� ���  
/// 3. ���߹迭(2�����迭�̻�)�� 1�����迭�� ����ü�� �ְ� ����ü�ȿ� �ٽ� 1�����迭�� ������ ¥�½����� ����ü�� �̿��Ͽ� ���߹迭�������� ����� ������ �ִ´�.
/// ex) struct A{B[] b; int i;}; struct B{C[] c; int a;};  struct C{int a;};  
/// [SerializeField]
/// A a ; 
/// public A A => a; 
/// �׽�Ʈ ���
/// 1. �ش�Ŭ������ ��ӹ޾Ƽ� ����غ������� ��ӹ��� Ŭ������ �ɹ������� ������ �ǳ� �о�ö� ���� ����� �������� �ʴ´�. 
///  1-1. �ذ�: �Լ�ȣ��� ��ӹ��� Ŭ������ �ѱ�� ����� �Ľ��� �ȴ�. �ε��Ҷ��� ���� ��ü�� ����Ͽ����Ѵ�.
/// 2. JsonGameData �� ���̽��� ��ӹ��� Ŭ������   ����Ƽ �̱��� ��ü�� �����ϱ����� ���׷������� �־�f���� ���۳�Ʈ����� �̱����������������. 
///  2-1. �ش� ������ �ٸ�������� ������ �����ϰ��ְ� �׽�Ʈ���̴�.
///  
/// 3. ���� ť�� ����Ʈ ������ �׽�Ʈ�� ���غ��Ѵ�.
/// 4. 
///  ************* ĳ���� ���������ϰ� �ش�Ŭ������ ����Ͽ� �Ľ��Լ��� �����Ѵ�.***************
///  
/// MonoBehaviour ��  [Serializable] �� ���������ʴ´�.
/// </summary>

//����ȭ : ���������� �Ľ��۾��� �ʿ��ϴٰ� �Ѵ�.
[Serializable]
public class JsonGameData 
{
    
    /// <summary>
    /// ����ð� �־�α� 
    /// </summary>
    [SerializeField]
    protected string saveTime;
    public string SaveTime { 
        get => saveTime;
        set { 
            saveTime = value;
        }
    }
    /// <summary>
    /// ĳ�����̸�
    /// </summary>
    [SerializeField]
    protected string charcterName;
    public String CharcterName { 
        get => charcterName;
        protected set 
        { 
            charcterName = value;
        }
    }
    /// <summary>
    /// ĳ������ ����
    /// </summary>
    [SerializeField]
    protected int level;
    public int Level { 
        get => level;
        protected set
        {
            level = value;
        }
    }
    /// <summary>
    /// ĳ���� �����ݾ�
    /// </summary>
    [SerializeField]
    protected long money;
    public long Money { 
        get => money;
        protected set { 
            money = value;
        }
    }
    /// <summary>
    /// �ҷ������ ���� ������ 
    /// </summary>
    [SerializeField]
    protected string sceanName;
    public string SceanName
    {
        get => sceanName;
        protected set
        {
            sceanName = value;
        }
    }
    /// <summary>
    /// ĳ���� ��������ġ
    /// </summary>
    [SerializeField]
    protected Vector3 characterPosition;
    public Vector3 CharcterPosition { 
        get => characterPosition;
        protected set
        {
            characterPosition = value;
        }
    }

    /// <summary>
    /// ĳ���� ���������� ����Ʈ
    /// </summary>
    [SerializeField]
    protected int[] itamIndexList;
    public int[] ItemIndexList { 
        get => itamIndexList;
        protected set
        {
            ItemIndexList = value;
        }
    }
   
    /// <summary>
    /// ĳ���ͽ��� �����ȣ �ε�������Ʈ  
    /// </summary>
    [SerializeField]
    protected int[] skill_IndexList;
    public int[] Skill_IndexList { 
        get => skill_IndexList;
        protected set 
        { 
            skill_IndexList = value;
        }
    }
}
