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

        //ITurnBaseData turnObj = TurnManager.Instance.CurrentTurn;   // �� ������Ʈã�Ƽ� 
        //ICharcterBase curruentUnit = turnObj.CurrentUnit;           // ���� �ൿ���� ���� ã�� 
        //if (curruentUnit == null)
        //{
        //    Debug.LogWarning("������ �����̾����ϴ�");
        //    return;
        //}
        //if (!curruentUnit.IsMoveCheck) //�̵����� �ƴѰ�츸  
        //{
        //    if (isMoveButtonClick) 
        //    {
                //turnObj.CurrentUnit.MoveSize -= turnObj.CurrentUnit.CurrentTile.MoveCheckG;
                //turnObj.TurnActionValue -= turnObj.CurrentUnit.CurrentTile.MoveCheckG;
        //    }
        //    SpaceSurvival_GameManager.Instance.MoveRange.ClearLineRenderer(curruentUnit.CurrentTile);
        //    SpaceSurvival_GameManager.Instance.MoveRange.MoveSizeView(curruentUnit.CurrentTile, curruentUnit.MoveSize);//�̵�����ǥ�����ֱ� 
        //    isMoveButtonClick = true;
        //}
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
