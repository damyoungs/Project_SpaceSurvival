using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    GameObject item_Description;
    [SerializeField]
    Text item_Discription_Txt;
    private void Awake()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("enter");
        SetDiscription();
        item_Description.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("exit");
        item_Description.SetActive(false);
    }
    void SetDiscription()
    {
        item_Discription_Txt.text = "맛있어 보인다!";
    }
}
