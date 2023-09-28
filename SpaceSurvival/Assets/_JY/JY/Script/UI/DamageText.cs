using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public Color NormalColor;
    public Color Critical_Color;
    TextMeshPro damageText;
    float timeElapse = 0;
    float moveSpeed = 2.0f;
    private void Awake()
    {
        damageText = GetComponent<TextMeshPro>();
    }
    private void OnEnable()
    {
        timeElapse = 0;
        damageText.alpha = 1.0f;
        transform.rotation = Camera.main.transform.rotation;
    }
    public void SetText(float damage, bool isCritical)//활성화 하는 곳에서 호출
    {
        if (isCritical)
        {
            damageText.fontStyle = FontStyles.Bold;
            damageText.fontSize = 10;
            damageText.color = Critical_Color;
        }
        else
        {
            damageText.fontStyle = FontStyles.Normal;
            damageText.fontSize = 7;
            damageText.color = NormalColor;
        }
        damageText.text = $"{damage}";
    }

    void Update()
    {
        timeElapse += Time.deltaTime;
        damageText.alpha -= (Time.deltaTime * 1.25f);
        if (timeElapse > 0.8f)
        {
            gameObject.SetActive(false);
        }
        transform.Translate(0, moveSpeed * Time.deltaTime, 0, Space.Self);
    }
}
