using AYellowpaper.SerializedCollections;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 表示したい大きさに調整したオブジェクトにアタッチする
/// </summary>
public class UI_ListContent : MonoBehaviour
{
    [field: SerializeField] public Instancer instancer { get; private set; } = new Instancer();
    /// <summary>
    /// RectTransformはstrech推奨
    /// </summary>
    public Instancer prefab = new Instancer();
    /// <summary>
    /// Valueは表示するテキスト
    /// </summary>
    public SerializedDictionary<string, string> contentTitle;
    public List<string> contentValues
    {
        get
        {
            return contentTitle.Values.ToList();
        }
    }
    public List<string> contentKeys
    {
        get
        {
            return contentTitle.Keys.ToList();
        }
    }
    public ListType listType;
    public bool isChangeTextToContentTitle;

    /// <summary>
    /// このRectTransformを基に列挙する
    /// </summary>
    public GameObject rectSample;

    public void SetActiveSample(bool _active)
    {
        instancer.obj.SetActive( _active );
    }

    /// <summary>
    /// contentTitleで列挙生成する
    /// </summary>
    public void InstanceList()
    {
        RectTransform _rectReference = rectSample.GetComponent<RectTransform>();
        for (int i = 0; i < contentTitle.Count; i++)
        {
            instancer.Instance(gameObject);

            if (isChangeTextToContentTitle == true)
            {
                TextMeshProUGUI _instancedText = instancer.lastObj.GetComponentInChildren<TextMeshProUGUI>();
                _instancedText.text = contentValues[i];

            }

            // offsetMax.x = Right
            // offsetMin.x = Left
            // offsetMax.y = PosY
            // offsetMin.y = Height

            switch (listType)
            {
                case ListType.Vertical:
                    RectTransform _instancedRect = instancer.lastObj.GetComponent<RectTransform>();
                    RectOperator.SetAnchor(_instancedRect, AnchorPresets.HorStretchTop);
                    RectOperator.SetPivot(_instancedRect, PivotPresets.TopCenter);
                    Vector2 _offsetMin = new Vector2(_rectReference.offsetMin.x, _rectReference.offsetMin.y * (i + 1));
                    Vector2 _offsetMax = new Vector2(_rectReference.offsetMax.x, _rectReference.offsetMin.y * i);

                    _instancedRect.offsetMin = _offsetMin;
                    _instancedRect.offsetMax = _offsetMax;



                    break;

                case ListType.Horizontal:



                    break;
            }
        }
        rectSample.SetActive(false);
    }

    public List<T> GetComponentsInInstancedList<T>()
    {
        List<T> _retList = new List<T>();
        for (int i = 0; i < Clones.Count; i++)
        {
            _retList.Add(Clones[i].GetComponentInChildren<T>());
        }

        return _retList;
    }

    public List<T> AddComponentsInInstancedList<T>() where T : MonoBehaviour
    {
        List<T> _retList = new List<T>();
        for (int i = 0; i < Clones.Count; i++)
        {
            _retList.Add(Clones[i].AddComponent<T>());
        }

        return _retList;
    }

    public void Assign_ContentTitles(List<string> _titles)
    {
        contentTitle.Clear();
        for (int i = 0; i < _titles.Count; i++)
        {
            contentTitle.Add(_titles[i], _titles[i]);
        }
    }

    /// <summary>
    /// Enumから項目を列挙生成
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void Assign_ContentTitles<T>() where T : Enum
    {
        List<T> _enum = EnumOperator.Get<T>();
        List<string> _convertList = new List<string>();
        for(int i = 0; i < _enum.Count; i++)
        {
            _convertList.Add(_enum[i].ToString());
        }

        Assign_ContentTitles(_convertList);
    }

    public void Assign_ContentTitles<T>(List<T> _enum) where T : Enum
    {
        List<string> _convertList = new List<string>();
        for (int i = 0; i < _enum.Count; i++)
        {
            _convertList.Add(_enum[i].ToString());
        }

        Assign_ContentTitles(_convertList);
    }

    /// <summary>
    /// 引数のenumに沿ってClonesを取得
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public SerializedDictionary<T, GameObject> GetInstancedListWithEnum<T>() where T : Enum
    {
        List<T> _enums = EnumOperator.Get<T>();
        var _retList = new SerializedDictionary<T, GameObject>();
        for (int i = 0; i < _enums.Count; i++)
        {
            _retList.Add(_enums[i], Clones[i]); 
        }

        return _retList;
    }
    public SerializedDictionary<T, GameObject> GetInstancedListWithEnum<T>(List<T> _enums) where T : Enum
    {
        var _retList = new SerializedDictionary<T, GameObject>();
        for (int i = 0; i < _enums.Count; i++)
        {
            _retList.Add(_enums[i], Clones[i]);
        }

        return _retList;
    }

    public void SetActiveClones(bool _active)
    {
        for (int i = 0; i < instancer.clones.Count; i++)
        {
            instancer.clones[i].SetActive(_active);
        }
    }

    public List<GameObject> Clones
    {
        get
        {
            return instancer.clones;
        }
    }

    public bool AssignedContentTitle
    {
        get
        {
            if (contentTitle.Count < 1)
            {
                return false;
            }
            else if (contentTitle == null)
            {
                return false;
            }

            return true;
        }
    }
}

public enum ListType
{
    Vertical,
    Horizontal,

}

