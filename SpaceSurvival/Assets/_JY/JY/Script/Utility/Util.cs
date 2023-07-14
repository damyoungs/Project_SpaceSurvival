using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util 
{
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;
        
        if (recursive == false)
        { 
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name) //비어있거나 파라미터로 받은 이름과 같다면 해당 오브젝트(Transform)의 컴포넌트를 리턴해라
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                    {
                        return component;
                    }
                }
            }
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) ||component.name == name)
                {
                    return component;
                }
            }
        }
        return null;
    }
}
