using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField]
    Text item_Discription;
    private void Awake()
    {
        item_Discription = transform.GetChild(2).GetComponent<Text>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("enter");
        SetDiscription();

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("exit");

    }
    void SetDiscription()
    {
        item_Discription.text = "맛있어 보인다!";
    }
}
