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
/// �V���A�����ł���AKeyValuePair
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
///// �W�F�l���b�N���B�����߂Ɍp�����Ă��܂�
///// [System.Serializable]�������̂�Y��Ȃ�
///// </summary>
//[System.Serializable]
//public class SampleTable : Serialize.TableBase<string, Vector3, SamplePair>
//{


//}

///// <summary>
///// �W�F�l���b�N���B�����߂Ɍp�����Ă��܂�
///// [System.Serializable]�������̂�Y��Ȃ�
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

//    //Inspector�ɕ\���ł���f�[�^�e�[�u��
//    public SampleTable sample;

//    void Awake()
//    {
//        //���e���
//        foreach (KeyValuePair<string, Vector3> pair in sample.GetTable())
//        {
//            Debug.Log("Key : " + pair.Key + "  Value : " + pair.Value);
//        }
//    }
//}
