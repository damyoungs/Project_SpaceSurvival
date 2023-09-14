using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SkillBox_Description : MonoBehaviour
{
    SkillData skillData;
    public SkillData SkillData
    {
        get => skillData;
        set
        {
            skillData = value;
        }
    }

    RectTransform rectTransform;
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
        nextLevel_LevelText = transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        nextLevel_Description = transform.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>();
        rectTransform = (RectTransform)transform;
    }

    public void MovePosition()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        float positionY = mousePos.y + (rectTransform.sizeDelta.y * 0.5f);
        float overY = positionY - Screen.height;


        if (positionY > Screen.height)
        {
            mousePos.y -= overY;
        }
        transform.position = mousePos;
    }
    public void Open(SkillData skillData, string currentLevel_Info, string nextLevel_Info)
    {
        SkillData = skillData;
        MovePosition();
        Refresh(skillData, currentLevel_Info, nextLevel_Info);
        canvasGroup.alpha = 1.0f;

    }
    public void Close()
    {
        canvasGroup.alpha = 0;
    }
    void Refresh(SkillData skillData, string current_Info, string next_Info)
    {
        skillName_Text.text = skillData.SkillName;
        currentLevel_Skill_Icon.sprite = skillData.skill_sprite;
        currentLevel_LevelText.text = skillData.SkillLevel.ToString();
        nextLevel_LevelText.text = $"{skillData.SkillLevel + 1}";
        currentLevel_Description.text = current_Info;
        nextLevel_Description.text = next_Info;
    }
}
