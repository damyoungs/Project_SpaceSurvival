using System;
using UnityEngine;
using UnityEngine.UI;
public class MoveActionButton : BattleActionButtonBase
{
    /// <summary>
    /// �ϴ� ���ư��Ը���� ���߿� �ð�������� 
    /// </summary>
    static bool isMoveButtonClick= false;
    public static bool IsMoveButtonClick 
    {
        get => isMoveButtonClick;
        set 
        {
            if (isMoveButtonClick != value) 
            {
                isMoveButtonClick = value;
                //��ư Ŭ������ �����̿���
                isButtonClick?.Invoke(value);
            }
        }
    }
    public static Action<bool> isButtonClick;

    Image backgroundImg;
    Color backColorOrigin;
    protected override void Awake()
    {
        base.Awake();
        backgroundImg = transform.GetChild(0).GetComponent<Image>();
        isButtonClick = OnClickCheckImageView;
        backColorOrigin = backgroundImg.color;
    }

    protected override void OnClick()
    {

        ITurnBaseData turnObj = TurnManager.Instance.CurrentTurn;   // �� ������Ʈã�Ƽ� 
        BattleMapPlayerBase curruentUnit = turnObj.CurrentUnit as BattleMapPlayerBase;           // ���� �ൿ���� ���� ã�� 
        if (curruentUnit == null)
        {
            Debug.LogWarning("������ �����̾����ϴ�");
            return;
        }
        if (!curruentUnit.IsMoveCheck) //�̵����� �ƴѰ�츸  
        {
            float moveSize = curruentUnit.CharcterData.Stamina > curruentUnit.MoveSize ? curruentUnit.MoveSize : curruentUnit.CharcterData.Stamina;
            SpaceSurvival_GameManager.Instance.MoveRange.ClearLineRenderer(curruentUnit.CurrentTile);
            SpaceSurvival_GameManager.Instance.MoveRange.MoveSizeView(curruentUnit.CurrentTile, moveSize);//�̵�����ǥ�����ֱ� 
            isMoveButtonClick = true;
        }
    }

    protected override void OnMouseEnter()
    {
        uiController.ViewButtons();
    }
    protected override void OnMouseExit()
    {
        uiController.ResetButtons();
    }

    private void OnClickCheckImageView(bool isClick)
    {
        if (isClick)
        {
            backgroundImg.color = Color.black;
        }
        else 
        {
            backgroundImg.color = backColorOrigin;
        }

    }
}
