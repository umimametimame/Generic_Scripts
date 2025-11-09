using AddUnityClass;
using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorProfile_Assign : MonoBehaviour
{
    protected Image image;
    protected TextMeshProUGUI text;
    
    public virtual void Assign()
    {

    }

    protected T IsMatchEnum<T>(T _enum) where T : Enum
    {
        List<T> enums = GetEnumList<T>();
        for(int i = 0; i < enums.Count; i++)
        {
            if(_enum.Equals(enums[i]))
            {
                return (T)Enum.ToObject(typeof(T), i);
            }
        }


        return _enum;
    }

    protected List<T> GetEnumList<T>() where T : Enum
    {
        List<T> enums = ConvertEnums<T>.GetList();
        return enums;
    }

    private void OnValidate()
    {
        if(TryGetComponent(out Image _image))
        {
            image = _image;
        }

        if(TryGetComponent(out TextMeshProUGUI _text))
        {
            text = _text;
        }

        Assign();
    }
}

public class ColorProfile : ScriptableObject
{

}