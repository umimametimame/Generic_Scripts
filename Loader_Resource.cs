using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Loader_Resource
{
    public static List<T> GetProfileListFromResource<T>(string _path) where T : ScriptableObject
    {
        List<T> _ret = Resources.LoadAll<T>(_path).ToList();

        return _ret;
    }

    public static SerializedDictionary<TKey, TProfile> GetProfileDicFromResource<TKey, TProfile>(string _path) where TKey : Enum where TProfile : ScriptableObject
    {
        SerializedDictionary<TKey, TProfile> _ret = new SerializedDictionary<TKey, TProfile>();
        List<TProfile> _resultList = GetProfileListFromResource<TProfile>(_path);

        for (int i = 0; i < _resultList.Count; i++)
        {
            _ret.Add((TKey)(object)i,_resultList[i]);
        }

        return _ret;
    }
}
