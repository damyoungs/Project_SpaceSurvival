
public interface EnumList 
{
	/// <summary>
	/// ���� ��ü
	/// </summary>
	enum ObjectGenelateList
	{
		Monster
	}
	/// <summary>
	/// ������Ʈ 
	/// </summary>
	enum SceanName 
	{
		Loding,
		Opening, //�����ȸ���
		Title,
		Ending,
		CreateCharcter, //�����ȸ���
		World,	//���� ���������� ����ؿ� �߰�
	}
	/// <summary>
	/// Ÿ��Ʋ���� ����� �޴����� ����Ʈ
	/// </summary>
	enum TitleMenu { 
		NewGame,
		Continue,
		Options,
		Exit
	}
	/// <summary>
	/// BGM ����Ʈ
	/// </summary>
	enum BGM_List { 
	
	}
	/// <summary>
	/// ȿ���� ����Ʈ
	/// </summary>
	enum EffectSound {
        Explosion1,
        Explosion2, 
		Explosion3
	}

	/// <summary>
	/// �ε�ȭ�鿡 ������ �̹��� ��������Ʈ
	/// </summary>
	enum ProgressType { 
		Bar = 0,

	}
}
