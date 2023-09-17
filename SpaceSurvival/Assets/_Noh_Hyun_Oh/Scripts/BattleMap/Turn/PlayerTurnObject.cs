using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/// <summary>
/// �÷��̾� �� 
/// </summary>
public class PlayerTurnObject : TurnBaseObject 
{
   
    /// <summary>
    /// �׽�Ʈ�� ���� 
    /// </summary>
    [SerializeField]
    int testPlayerLength = 1;

    /// <summary>
    /// ��Ʋ�ʿ��� �̺�Ʈ�ڵ鷯�� ������ ������Ʈ
    /// </summary>
    [SerializeField]
    BattleMap_Player_Controller bpc;

    [SerializeField]
    CameraOriginTarget cot;
    
    /// <summary>
    /// ĳ���� �����ʹ� �ܺο��� �����ϱ⶧���� �ش� ��������Ʈ �����������
    /// </summary>
    public Func<ICharcterBase[]> initPlayer;

    MiniMapCamera miniMapCam;
    
    /// <summary>
    /// ������ �ʱ�ȭ �Լ�
    /// </summary>
    public override void InitData()
    {
        //�ش������Ʈ�� ���丮���� ���������� 
        bpc = FindObjectOfType<BattleMap_Player_Controller>();   // ��Ʈ�ѷ��� ��Ʋ�ʿ����� �ִ� ������Ʈ�� �ʱ�ȭ �Ҷ� ã�ƿ´�
        cot = FindObjectOfType<CameraOriginTarget>(true);        // ��Ʈ�ѷ��� ��Ʋ�ʿ����� �ִ� ������Ʈ�� �ʱ�ȭ �Ҷ� ã�ƿ´�
        miniMapCam = FindObjectOfType<MiniMapCamera>(true);      // ��Ʈ�ѷ��� ��Ʋ�ʿ����� �ִ� ������Ʈ�� �ʱ�ȭ �Ҷ� ã�ƿ´�
        bpc.onClickPlayer = OnClickPlayer;                       // Ÿ���� Ŭ�������� �÷��̾� ���ִ�Ÿ��(Ÿ�ϼӼ��� ����) �̸� ����� �Լ��� �����Ѵ�. 
        bpc.onMoveActive = OnUnitMove;                           // Ÿ���� Ŭ�������� �÷��̾ �����̵��� ��������
        bpc.GetPlayerTurnObject = () => this;                    // �ʱⰪ ������ ���� 

        if (initPlayer != null) //�ܺ� �Լ��� ����� ������
        {
            ICharcterBase[] playerList = initPlayer(); //������ ��û�� �ϰ� 
            if (playerList != null && playerList.Length > 0) //�����Ͱ� �����ϸ�  
            {
                foreach (ICharcterBase player in playerList) //������ ������ŭ 
                {
                    charcterList.Add(player); //�ϰ����� ĳ���ͷ� ����
                    player.GetCurrentTile = () => SpaceSurvival_GameManager.Instance.MoveRange.GetRandomTile(Tile.TileExistType.Charcter); //Ÿ�� ���ÿ���
                    player.transform.position = player.CurrentTile.transform.position;//���õ� Ÿ����ġ�� �̵���Ų��.
                }
                WindowList.Instance.TeamBorderManager.ViewTeamInfo(playerList.Length);//�� ��� ������ �����ֱ� 
            }
            else 
            {
                Debug.LogWarning($"{name} ������Ʈȣ��  \n �ܺ� �÷��̾� �����Ͱ� ������ �ȵ��ֽ��ϴ�.");
            }
        }
        else //�ܺ��Լ��� ����ȵ��ִ°��  
        {
            BattleMapPlayerBase go;
            //�׽�Ʈ ������ ����
            for (int i = 0; i < testPlayerLength; i++)//ĳ���͵� �����ؼ� ���� 
            {
                go = (BattleMapPlayerBase)Multiple_Factory.Instance.GetObject(EnumList.MultipleFactoryObjectList.CHARCTER_PLAYER_POOL);
                charcterList.Add(go);
                go.name = $"Player_{i}";
                go.SetTile(SpaceSurvival_GameManager.Instance.MoveRange.GetRandomTile(Tile.TileExistType.Charcter));
                go.transform.position = go.CurrentTile.transform.position; //���õ� Ÿ����ġ�� �̵���Ų��.
            }
            WindowList.Instance.TeamBorderManager.ViewTeamInfo(testPlayerLength); //�� ��� ������ �����ֱ� 

        }

   
    }

    /// <summary>
    /// �ϸ޴������� �ڽ��� ���϶� �������ִ� �Լ� 
    /// </summary>
    public override void TurnStartAction()
    {
        isTurn = true; // �ڽ��� ������ üũ�Ѵ�. ������ ��������Ʈ�� �����صξ��� ������ �������ư ���� ����ȴ�.
        Debug.Log($"{name} ������Ʈ�� ���� ���۵Ǿ��� �ൿ�� : {TurnActionValue}");
        currentUnit = charcterList[0]; //�÷��̾� �������ϰ� 
        currentUnit.IsControll = true; //��Ʈ�� �Ҽ��ְ� �����Ѵ�.
        cot.Target = currentUnit.transform; //ī�޶� ��Ŀ�� ���߱� 
 
        //ĳ���������� ���׹̳� ������ �ѱ��
        BattleMapPlayerBase currentCharcter = (BattleMapPlayerBase)currentUnit;
        Player_ currentPlayer = currentCharcter.CharcterData;
        currentPlayer.Stamina = TurnActionValue;
        float moveSize = currentUnit.MoveSize < TurnActionValue ? currentUnit.MoveSize : TurnActionValue;//�̵����� �ִ� ũ����Ƴ�����ŭ�� ǥ���ϱ����� ��
        Debug.Log(TurnActionValue);
        //��������� ����

        TeamBorderStateUI uiComp = WindowList.Instance.TeamBorderManager.TeamStateUIs[0];
        uiComp.SetHpGaugeAndText(currentPlayer.HP,currentPlayer.MaxHp);
        uiComp.SetStmGaugeAndText(currentPlayer.Stamina, currentPlayer.Max_Stamina);


        SelectControllUnit(); //���� ���÷��� ����

        // ù�ε��� ����Ÿ�̹־ȸ��� 
        if (currentUnit.BattleUI != null)
        {
            
            currentUnit.BattleUI.stmGaugeSetting(TurnActionValue, currentPlayer.Max_Stamina);
            currentUnit.BattleUI.hpGaugeSetting(currentPlayer.HP, currentPlayer.MaxHp);
        }
        SpaceSurvival_GameManager.Instance.MoveRange.ClearLineRenderer(currentUnit.CurrentTile);
        SpaceSurvival_GameManager.Instance.MoveRange.MoveSizeView(currentUnit.CurrentTile, moveSize);//�̵�����ǥ�����ֱ� 

        SpaceSurvival_GameManager.Instance.AttackRange.ClearLineRenderer();     //���ݹ��� �ʱ�ȭ 
        SpaceSurvival_GameManager.Instance.AttackRange.InitDataSet(currentCharcter); //�Ͻ��۵ɶ� ���� ���� ���� �� �������� ����
    }

    /// <summary>
    /// ���� ��Ʈ�� ���� ������ ������ ��Ʈ���������� �̵����� �����ϱ� 
    /// </summary>
    /// <param name="seletedTile">���õ� Ÿ��</param>
    private void OnUnitMove(Tile seletedTile) 
    {
        if (EventSystem.current.IsPointerOverGameObject())//�����Ͱ� UI ���� Mouse Over�� ��� return;
        {
            Debug.Log("UI ����");
            return;
        }
        if (currentUnit != null && currentUnit.IsControll) //���� ��Ʈ���ΰ�츸 
        {

            currentUnit.CharcterMove(seletedTile);//�̵����� ����
           
        }
    }
   
    /// <summary>
    /// �Ʊ��� Ŭ�������� ó���� ���� 
    /// </summary>
    /// <param name="clickedTile">Ŭ���� Ÿ��</param>
    public void OnClickPlayer(Tile clickedTile)
    {
        //if (currentUnit == null) //�÷��̾ �����ȵ������� 
        //{
        //    foreach (ICharcterBase playerUnit in charcterList) //�÷��̾� ������ġ���� üũ�ϱ����� �÷��̾ ������.
        //    {
        //        if (clickedTile.width == playerUnit.CurrentTile.width &&
        //            clickedTile.length == playerUnit.CurrentTile.length) //Ŭ���� Ÿ���� �÷��̾� ���� ��ġ�� 
        //        {
        //            currentUnit = playerUnit; //�÷��̾� �������ϰ� 
        //            currentUnit.IsControll = true; //��Ʈ�� �Ҽ��ְ� �����Ѵ�.
        //            cot.Target = currentUnit.transform; //ī�޶� ��Ŀ�� ���߱� 
        //            SelectControllUnit();
        //            return;
        //        }
        //    }
        //}
        currentUnit.IsControll = true; //��Ʈ�� �Ҽ��ְ� �����Ѵ�.
        cot.Target = currentUnit.transform; //ī�޶� ��Ŀ�� ���߱� 
        if (!currentUnit.IsMoveCheck)// ĳ���� �̵������� üũ�ؼ� �̵��������� ���� ���� 
        {
            if (currentUnit == null || //��Ʈ������ ������ ���ų� 
                clickedTile.width != currentUnit.CurrentTile.width ||
                clickedTile.length != currentUnit.CurrentTile.length
                )//��Ʈ�� ���� ������ ��ġ�� �ٸ���� 
            {
                foreach (ICharcterBase playerUnit in charcterList) //�÷��̾� ������ġ���� üũ�ϱ����� �÷��̾ ������.
                {
                    if (clickedTile.width == playerUnit.CurrentTile.width &&
                        clickedTile.length == playerUnit.CurrentTile.length) //Ŭ���� Ÿ���� �÷��̾� ���� ��ġ�� 
                    {
                        if (currentUnit != null) //������ ��Ʈ�� ���� ������ ������  
                        {
                            currentUnit.IsControll = false; //�������� ��Ʈ�� �����ϰ� 
                            //SpaceSurvival_GameManager.Instance.MoveRange.ClearLineRenderer(currentUnit.CurrentTile); //�̵����� ���½�Ų��.
                        }
                        TurnActionValue -= currentUnit.CurrentTile.MoveCheckG;  //�̵��Ѱ���ŭ ���ҽ�Ű��
                        currentUnit = playerUnit; //�ٸ� �Ʊ��� ���
                        currentUnit.IsControll = true; //��Ʈ�� �Ҽ��ְ� �����Ѵ�.
                        cot.Target = currentUnit.transform; //ī�޶� ��Ŀ�� ���߱� 
                        miniMapCam.player = currentUnit.transform;
                        SelectControllUnit();
                        return;
                    }
                }
            }
            else //���� ��Ʈ�� ���� ������ �ִ� Ÿ���� Ŭ���������  
            {
                PlayerSelect();
            }
        }
        else 
        {
            Debug.LogWarning("ĳ���Ͱ� �̵����Դϴ�.");
        }
    }

    /// <summary>
    /// ĳ���Ͱ� ���õ� ���¿��� �ٽ� ���õɶ� ó���ҷ��� 
    /// </summary>
    private void PlayerSelect()
    {
        Debug.Log($"��Ʈ������ {currentUnit.transform.name} �� �ٽ� �����ߴ�.");
    }

    /// <summary>
    /// ��Ʈ�� �������� ���� �ɶ� ó���ҷ��� 
    /// </summary>
    private void SelectControllUnit()
    {
        //currentUnit.MoveSize = TurnActionValue; //���ο�ĳ���� �̵����ɹ��� ����
        MoveActionButton.IsMoveButtonClick = false; //�����Ƽ� ����ƽ
        //Debug.Log($"��Ʈ������ : {currentUnit.transform.name} �����ߴ�.");
    }

    public override void ResetData()
    {
        if (currentUnit != null) //���� �������� ������ �ִ°�� 
        {
            currentUnit.IsControll = false; //��Ʈ�� ���� �Ѵ�.
            currentUnit = null;
        }
        base.ResetData();//�׸��� ������ �ʱ�ȭ �Ѵ�.
    }

}
