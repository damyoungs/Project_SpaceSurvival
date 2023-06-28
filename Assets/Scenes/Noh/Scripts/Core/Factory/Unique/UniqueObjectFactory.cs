using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// �Ѱ��� �����Ǵ� ��ü���� ���������� ����� Ŭ���� 
/// ���������� ó���ؾߵ� �����ۼ�
/// �̸��� ���丮....Prefab �̶����̴°Գ���������..
/// </summary>
public class UniqueObjectFactory : Singleton<UniqueObjectFactory>
{

    /// <summary>
    /// �ɼ�â ���� ����
    /// ��Ÿ�ɼ� ��  ���� �ҷ����� ���� ��
    /// </summary>
    [SerializeField]
    private GameObject optionWindow;

    /// <summary>
    /// �÷��̾� ����â ��������
    /// �κ�, �������ͽ� , ��ų ��
    /// </summary>
    [SerializeField]
    private GameObject playerWindow;

    /// <summary>
    /// ���÷��̾� ����â �������� 
    /// ���� , ũ������ , ��ȭâ? �� 
    /// </summary>
    [SerializeField]
    private GameObject nonPlayerWindow;

    /// <summary>
    /// �ε�ȭ�鿡 ���� �����
    /// </summary>
    [SerializeField]
    private GameObject progressList;

    /// <summary>
    /// �⺻ ������� 
    /// </summary>
    [SerializeField]
    private GameObject defaultBGM;

    /// <summary>
    /// â���� Ŭ���̳� ���ý� �Ҹ� 
    /// </summary>
    [SerializeField]
    private GameObject SystemEffectSound;

    /// <summary>
    /// �ʿ��ѿ�����Ʈ �������� ���. 
    /// prefab ������Ʈ�� �Ѱ��ֱ⶧���� 
    /// ����������Ʈ �������� �����̾ȵȴ�.
    /// </summary>
    /// <param name="type">������Ʈ ����</param>
    /// <returns>prefab ������Ʈ �ٷο������ֱ�</returns>
    public GameObject GetObject(EnumList.UniqueFactoryObjectList type) {

        switch (type) {
            case EnumList.UniqueFactoryObjectList.OPTIONWINDOW:
                return optionWindow;
            case EnumList.UniqueFactoryObjectList.PLAYERWINDOW:
                return playerWindow;
            case EnumList.UniqueFactoryObjectList.NONPLAYERWINDOW:
                return nonPlayerWindow;
            case EnumList.UniqueFactoryObjectList.PROGRESSLIST:
                return progressList;
            case EnumList.UniqueFactoryObjectList.DEFAULTBGM:
                return defaultBGM;
            default:
                return null;
        }
    }

  
}
