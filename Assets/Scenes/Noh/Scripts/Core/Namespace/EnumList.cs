
using UnityEngine;
/*
 enum�� �����Ҷ� ���� Ŭ���� �ۿ��� �����ϱ⶧���� �׳� ���ӽ����̽��� ��� ���
 enum �� �ʿ��Ѱ�� ���⿡ �߰�.
 */
namespace EnumList 
{
    /// <summary>
    /// ������Ʈ 
    /// �ɹ��������� ���̸����θ�����Ѵ�. �׸��� �����ʿ� ������ ������Ѵ�.
    /// </summary>
    public enum SceanName 
	{
		NONE = -1,//���þȉ������ǰ� �̰����õǸ�ȵȴ� �⺻������.
		TITLE,
		LOADING,
		OPENING, 
		ENDING,
        //Item_Test,//�κ�â�����γѾ��Ȯ��
        //CreateCharcter, //�����ȸ���
		SAVELOADTEST,	//���� ���������� ����ؿ� �߰�
	}
    /// <summary>
    /// �ε�ȭ�鿡 ������ �̹��� ��������Ʈ
    /// </summary>
    public enum ProgressType
    {
        BAR = 0,

    }
    /// <summary>
    /// Ÿ��Ʋ���� ����� �޴����� ����Ʈ
    /// </summary>
    public enum TitleMenu { 
		NEWGAME =0,
		CONTINUE,
		OPTIONS,
		EXIT
	}
	/// <summary>
	/// BGM ����Ʈ
	/// </summary>
	public enum BGM_List { 
	
	}
	/// <summary>
	/// ȿ���� ����Ʈ
	/// </summary>
	public enum EffectSound {
        EXPLOSION1,
        EXPLOSION2, 
		EXPLOSION3
	}
	/// <summary>
	/// ����ȭ�� ��ư����
	/// </summary>
	public enum SaveLoadButtonList { 
		NONE = 0,
		SAVE,
		LOAD,
		COPY,
		DELETE,
	}
	public enum PopupList { 
		NONE = -1,
		SAVELOADPOPUP,
	}
	/// <summary>
	/// ���� ��ü
	/// </summary>
	public enum MultipleFactoryObjectList
	{
		SAVEDATAPOOL = 0, //����ȭ�鿡 ������ ������Ʈ����� Ǯ
	}


	public enum UniqueFactoryObjectList 
	{
		OPTIONWINDOW = 0, //�ɼ�â ESC�� O Ű�������� ������ �Ϸ��� ������
		PLAYERWINDOW,     //�÷��̾� ����â Ư������Ű�� �����ؼ� ����Ϸ��� ������
		NONPLAYERWINDOW,  //NPC ���� �����ɰ� �����̳� �޽� â���� �ſ� ���ɿ���
		PROGRESSLIST,     //���α׷����� ������ �þ�� ���� ���� �־��
		DEFAULTBGM,		  //������� ó���� �̱���������� ������
		SYSTEMEFFECTSOUND //����Ʈ���� ���� �̱��� ���� ���۾���.
	}
}
