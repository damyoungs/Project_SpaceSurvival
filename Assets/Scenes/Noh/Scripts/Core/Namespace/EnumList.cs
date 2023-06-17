
using UnityEngine;
/*
 enum�� �����Ҷ� ���� Ŭ���� �ۿ��� �����ϱ⶧���� �׳� ���ӽ����̽��� ��� ���
 enum �� �ʿ��Ѱ�� ���⿡ �߰�.
 */
namespace EnumList 
{
    /// <summary>
    /// ������Ʈ 
    /// �ɹ��������� ���̸����θ�����Ѵ�.
    /// </summary>
    public enum SceanName 
	{
		Loading =0,
		Opening, 
		Title,
		Ending,
		CreateCharcter, //�����ȸ���
        Item_Test,
        World,	//���� ���������� ����ؿ� �߰�
	}
    /// <summary>
    /// �ε�ȭ�鿡 ������ �̹��� ��������Ʈ
    /// </summary>
    public enum ProgressType
    {
        Bar = 0,

    }
    /// <summary>
    /// Ÿ��Ʋ���� ����� �޴����� ����Ʈ
    /// </summary>
    public enum TitleMenu { 
		NewGame =0,
		Continue,
		Options,
		Exit
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
        Explosion1,
        Explosion2, 
		Explosion3
	}

	
	/// <summary>
	/// ���� ��ü
	/// </summary>
	public enum MultipleFactoryObjectList
	{
		SaveDataPool = 0, //����ȭ�鿡 ������ ������Ʈ����� Ǯ
	}


	public enum UniqueFactoryObjectList 
	{
		OptionWindow = 0, //�ɼ�â esc�� o Ű�������� ������ �Ϸ��� ������
		PlayerWindow,     //�÷��̾� ����â Ư������Ű�� �����ؼ� ����Ϸ��� ������
		NonPlayerWindow,  //Npc ���� �����ɰ� �����̳� �޽� â���� �ſ� ���ɿ���
		ProgressList,     //���α׷����� ������ �þ�� ���� ���� �־��
		DefaultBGM,		  //������� ó���� �̱���������� ������
		SystemEffectSound //����Ʈ���� ���� �̱��� ���� ���۾���.
	}
}
