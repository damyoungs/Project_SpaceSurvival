using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Buttons : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI scoreTxt;
    int score = 0;

  

    Dictionary<Type, UnityEngine.Object[]> _Objects = new Dictionary<Type, UnityEngine.Object[]>();
    private void Start()
    {
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
    }
    enum Texts
    {
        ScoreText,
        PointText
    }
    enum Buttons
    {  
        PointButton
    }
    void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);

        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _Objects.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; i++)
        {
            objects[i] = Util.FindChild<T>(gameObject, names[i], true);
        }
    }

    public void OnBtnClick()
    {
        score ++;
        scoreTxt.text = $"score : {score}";
    }
}
