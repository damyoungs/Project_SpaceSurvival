using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestNPCTest : MonoBehaviour
{
    [SerializeField]
    GameObject questNPCPrefab;
    private void Start()
    {
        Tile[] battleMap = SpaceSurvival_GameManager.Instance.BattleMap;
        int maxX = SpaceSurvival_GameManager.Instance.MapSizeX;
        int maxY = SpaceSurvival_GameManager.Instance.MapSizeY;
        Instantiate(questNPCPrefab, Vector3.zero, Quaternion.identity);
        //go.SetTile(SpaceSurvival_GameManager.Instance.MoveRange.GetRandomTile(Tile.TileExistType.Monster));
    }
}
