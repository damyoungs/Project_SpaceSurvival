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
    public enum SceneName 
	{
		NONE = -1,//���þȉ������ǰ� �̰����õǸ�ȵȴ� �⺻������.
		TITLE,
		LOADING,
		OPENING, 
		ENDING,
        //Item_Test,//�κ�â�����γѾ��Ȯ��
        //CreateCharcter, //�����ȸ���
        TestBattleMap,	//���� ���������� ����ؿ� �߰�
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
		SAVE_LOAD_POPUP,
	}
	/// <summary>
	/// ���� ��ü
	/// </summary>
	public enum MultipleFactoryObjectList
	{
		SAVE_DATA_POOL = 0, //����ȭ�鿡 ������ ������Ʈ����� Ǯ
        SAVE_PAGE_BUTTON_POOL,
        TURN_GAUGE_UNIT_POOL,
        TRACKING_BATTLE_UI_POOL,
		STATE_POOL,
		BATTLEMAP_UNIT_POOL,

    }


	public enum UniqueFactoryObjectList 
	{
		OPTIONS_WINDOW = 0, //�ɼ�â ESC�� O Ű�������� ������ �Ϸ��� ������
		PLAYER_WINDOW,     //�÷��̾� ����â Ư������Ű�� �����ؼ� ����Ϸ��� ������
		NON_PLAYER_WINDOW,  //NPC ���� �����ɰ� �����̳� �޽� â���� �ſ� ���ɿ���
		PROGRESS_LIST,     //���α׷����� ������ �þ�� ���� ���� �־��
		DEFAULT_BGM,		  //������� ó���� �̱���������� ������
		SYSTEM_EFFECT_SOUND //����Ʈ���� ���� �̱��� ���� ���۾���.
	}

	/// <summary>
	/// ȭ�� ���� ���� �ߵ� ���� 
	/// </summary>
	public enum StateType 
	{
		None = 0, //��
        ElectricShock, //����
        Freeze, //����
        Poison, //�ߵ�
        Fear, //����
        Burns //ȭ��
    }

	/// <summary>
	/// ĳ���� ī�޶� 
	/// </summary>
	public enum CameraTargetType
	{
		None = -1,
		MiniMap ,
		Player = 0,
		Npc,
	}

}