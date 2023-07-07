using System;
/// <summary>
/// ������ �ʿ��� Ŭ������ ���������̽��� ��ӹ޾Ƽ� SortValue ���� �����Ѵ�.
/// </summary>
public interface ITurnBaseData
{
    /// <summary>
    /// ������� �߰��� �ൿ�� 
    /// ���� �����϶� ���̸� �α����� �ʿ��Ѱ����� ���� �������� ���� 1000������ ���� ��Ű��ȴ�.
    /// </summary>
    public float TurnEndActionValue { get; }
    /// <summary>
    /// �ൿ�� ���ĵ� ���ذ�
    /// </summary>
    public float TurnActionValue { get; set; }

    /// <summary>
    /// �ϿϷ�� �˷��� ��������Ʈ
    /// </summary>
    public Action<ITurnBaseData> TurnEndAction { get; set; }

    /// <summary>
    /// �Ͻ��۽� ������ �Լ�
    /// </summary>
    public void TurnStartAction();
}
