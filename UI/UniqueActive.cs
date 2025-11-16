using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 子要素のいずれか1つのみActiveにする
/// </summary>
public class UniqueActive : MonoBehaviour
{
    public GameObject initialGameObject;
    public GameObject newestGameObject
    {
        get
        {
            if (objHistory.Count == 0)
            {
                return initialGameObject;
            }

            return objHistory[objHistory.Count - 1];
        }
    }
    [field: SerializeField, NonEditable] public List<GameObject> objHistory {  get; private set; } = new List<GameObject>();
    /// <summary>
    /// initialGameObjectとcanvasHistoryを含む
    /// </summary>
    public List<GameObject> allGameObject
    {
        get
        {
            List<GameObject> _newGameObjectList = new List<GameObject>();
            _newGameObjectList.Add(initialGameObject);
            _newGameObjectList.AddRange(objHistory);

            return _newGameObjectList;
        }
    }

    private void Start()
    {
        SwitchGameObject(initialGameObject);
    }

    /// <summary>
    /// canvasHistoryが存在するか
    /// </summary>
    public bool returnable
    {
        get
        {
            if (objHistory.Count <= 0)
            {
                return false;
            }

            return true;
        }
    }

    /// <summary>
    /// 表示中するオブジェクトを切り替える
    /// </summary>
    /// <param name="_obj"></param>
    public void SwitchGameObject(GameObject _obj)
    {
        Debug.Log(_obj.name);
        if (newestGameObject != _obj)
        {
            // 履歴に追加
            objHistory.Add(_obj);
        }


        // 引数のGameObject以外を非表示
        for (int i = 0; i < allGameObject.Count; i++)
        {
            if (allGameObject[i] == newestGameObject)
            {
                allGameObject[i].SetActive(true);
            }
            else
            {
                allGameObject[i].SetActive(false);
            }
        }

    }


    public void Return()
    {
        if (returnable == false)
        {
            return;
        }

        newestGameObject.SetActive(false);
        objHistory.RemoveAt(objHistory.Count - 1);
        SwitchGameObject(newestGameObject);
    }

    public void ToHome()
    {
        SwitchGameObject(initialGameObject);
        objHistory.Clear();
    }

}

[Serializable]
public class UniqueActive_Enum<T> where T : Enum
{
    public UniqueActive reference;
    [field: SerializeField, NonEditable] public SerializedDictionary<T, GameObject> objList { get; private set; } = new SerializedDictionary<T, GameObject>();

    [ContextMenu(nameof(AssignList))] 
    public void AssignList()
    {
        List<T> _enums = EnumOperator.Get<T>();
        AssignList(_enums);
    }

    public void AssignList(List<T> _enums)
    {
        objList = new SerializedDictionary<T, GameObject>();

        for (int i = 0; i < reference.transform.childCount; i++)
        {
            GameObject _newObj = reference.transform.GetChild(i).gameObject;
            objList.Add(_enums[i], _newObj);

            if (i == 0)
            {
                reference.initialGameObject = _newObj;
            }
        }
    }

    public void Switch(T _enum)
    {
        reference.SwitchGameObject(objList[_enum]);
    }

    public void Return()
    {
        reference.Return(); ;
    }
}