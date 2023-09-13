using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SkillBox_Description : MonoBehaviour
{
    SkillData skillData;
    SkillData SkillData
    {
        get => skillData;
        set
        {
            skillData = value;
        }
    }

    CanvasGroup canvasGroup;
    //아이콘, 레벨, 네임, 설명
    TextMeshProUGUI skillName_Text;
    Image currentLevel_Skill_Icon;
    Image nextLevel_Skill_Icon;
    TextMeshProUGUI currentLevel_LevelText;
    TextMeshProUGUI nextLevel_LevelText;
    TextMeshProUGUI currentLevel_Description;
    TextMeshProUGUI nextLevel_Description;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        skillName_Text = transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        currentLevel_Skill_Icon = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
        currentLevel_LevelText = transform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>();
        currentLevel_Description = transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>();
        nextLevel_Skill_Icon = transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>();
        nextLevel_LevelText = transform.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>();
        nextLevel_Description = transform.GetChild(1).GetChild(3).GetComponent<TextMeshProUGUI>();
    }

    public void MovePosition()
    {
        transform.position = Mouse.current.position.ReadValue();
    }
    public void Open(SkillData skillData)
    {
        MovePosition();
        canvasGroup.alpha = 1.0f;

    }
    public void Close()
    {
        canvasGroup.alpha = 0;
    }
    void Refresh()
    {

    }
}
