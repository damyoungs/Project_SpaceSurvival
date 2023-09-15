using System;
using UnityEngine;
using UnityEngine.UI;
public class MoveActionButton : BattleActionButtonBase
{
    /// <summary>
    /// 일단 돌아가게만들고 나중에 시간나면수정 
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
                //버튼 클릭여부 유아이연결
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

        ITurnBaseData turnObj = TurnManager.Instance.CurrentTurn;   // 턴 오브젝트찾아서 
        BattleMapPlayerBase curruentUnit = turnObj.CurrentUnit as BattleMapPlayerBase;           // 현재 행동중인 유닛 찾고 
        if (curruentUnit == null)
        {
            Debug.LogWarning("선택한 유닛이없습니다");
            return;
        }
        if (!curruentUnit.IsMoveCheck) //이동중이 아닌경우만  
        {
            float moveSize = curruentUnit.CharcterData.Stamina > curruentUnit.MoveSize ? curruentUnit.MoveSize : curruentUnit.CharcterData.Stamina;
            SpaceSurvival_GameManager.Instance.MoveRange.ClearLineRenderer(curruentUnit.CurrentTile);
            SpaceSurvival_GameManager.Instance.MoveRange.MoveSizeView(curruentUnit.CurrentTile, moveSize);//이동범위표시해주기 
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
