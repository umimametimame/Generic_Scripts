using System;
using System.Collections.Generic;
using UnityEngine;

public class SerializeDic : MonoBehaviour
{
}

[Serializable]
public class TableBase<TKey, TValue, Type> where Type : KeyAndValue<TKey, TValue>
{
    [SerializeField]
    private List<Type> list;
    private Dictionary<TKey, TValue> table;


    public Dictionary<TKey, TValue> GetTable()
    {
        if (table == null)
        {
            table = ConvertListToDictionary(list);
        }
        return table;
    }

    /// <summary>
    /// Editor Only
    /// </summary>
    public List<Type> GetList()
    {
        return list;
    }

    static Dictionary<TKey, TValue> ConvertListToDictionary(List<Type> list)
    {
        Dictionary<TKey, TValue> dic = new Dictionary<TKey, TValue>();
        foreach (KeyAndValue<TKey, TValue> pair in list)
        {
            dic.Add(pair.Key, pair.Value);
        }
        return dic;
    }
}

/// <summary>
/// シリアル化できる、KeyValuePair
/// </summary>
[Serializable]
public class KeyAndValue<TKey, TValue>
{
    public TKey Key;
    public TValue Value;

    public KeyAndValue(TKey key, TValue value)
    {
        Key = key;
        Value = value;
    }
    public KeyAndValue(KeyValuePair<TKey, TValue> pair)
    {
        Key = pair.Key;
        Value = pair.Value;
    }
}

//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;


///// <summary>
///// ジェネリックを隠すために継承してしまう
///// [System.Serializable]を書くのを忘れない
///// </summary>
//[System.Serializable]
//public class SampleTable : Serialize.TableBase<string, Vector3, SamplePair>
//{


//}

///// <summary>
///// ジェネリックを隠すために継承してしまう
///// [System.Serializable]を書くのを忘れない
///// </summary>
//[System.Serializable]
//public class SamplePair : Serialize.KeyAndValue<string, Vector3>
//{

//    public SamplePair(string key, Vector3 value) : base(key, value)
//    {

//    }
//}

//public class SerializableTest : MonoBehaviour
//{

//    //Inspectorに表示できるデータテーブル
//    public SampleTable sample;

//    void Awake()
//    {
//        //内容を列挙
//        foreach (KeyValuePair<string, Vector3> pair in sample.GetTable())
//        {
//            Debug.Log("Key : " + pair.Key + "  Value : " + pair.Value);
//        }
//    }
//}
