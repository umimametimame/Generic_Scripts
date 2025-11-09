using AddUnityClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// ============ C#系 =============
[Serializable]
public class RectNeo
{
    [field: SerializeField] public float x { get; private set; }
    [field: SerializeField] public float y { get; private set; }
    [field: SerializeField] public float width { get; private set; }
    [field: SerializeField] public float height { get; private set; }
    public Rect entity { get; private set; }

    public RectNeo(Rect rect)
    {
        x = rect.x;
        y = rect.y;
        width = rect.width;
        height = rect.height;

        entity = rect;
    }

    public void Initialize(Rect rect)
    {
        x = rect.x;
        y = rect.y;
        width = rect.width;
        height = rect.height;

        entity = rect;
    }

    public void SetHorizontal(float x, float width)
    {
        this.x = x;
        this.width = width;

        entity = new Rect(this.x, this.y, this.width, this.height);
    }

    public void SetVertical(float y, float height)
    {
        this.y = y;
        this.height = height;

        entity = new Rect(this.x, this.y, this.width, this.height);
    }
    public float X
    {
        set
        {
            x = value;
            entity = new Rect(x, y, width, height);
        }
    }

    public float Width
    {
        set
        {
            width = value;
            entity = new Rect(x, y, width, height);
        }
    }
    public float Y
    {
        set
        {
            y = value;
            entity = new Rect(x, y, width, height);
        }
    }

    public float Height
    {
        set
        {
            height = value;
            entity = new Rect(x, y, width, height);
        }
    }

}

[Serializable]
public class EntityAndPlan<T>
{
    [field: SerializeField] public T entity { get; set; }
    [field: SerializeField] public T plan { get; set; }

    /// <summary>
    /// plan = entity
    /// </summary>
    public void Assign()
    {
        plan = entity;
    }

    public void Default()
    {
        plan = default;
        entity = default;
    }
    public void Reset(T t1)
    {
        plan = t1;
        entity = t1;
    }
    public void PlanDefault()
    {
        plan = default;
    }
}


/// <summary>
/// 複数のFuncを全て判断する
/// </summary>
public class BoolFuncs
{
    List<Func<bool>> funcs = new List<Func<bool>>();
    public void Initialize()
    {
        funcs.Clear();
    }

    public void Add(Func<bool> _func)
    {
        funcs.Add(_func);
    }

    public bool Invoke()
    {
        int i;
        for (i = 0; i < funcs.Count; ++i)
        {
            if (funcs[i].Invoke() == false)
            {
                return false;
            }
        }

        if (i == 0)
        {
            Debug.Log("Funcがありません");
        }
        return true;

    }

    public static BoolFuncs operator +(BoolFuncs a, Func<bool> b)
    {
        a.Add(b);
        return a;
    }
}


[Serializable]
public class VecT<T>
{
    public T x;
    public T y;
    public T z;

    private List<T> list = new List<T>();

    public VecT()
    {
        GetList();
    }

    private List<T> GetList()
    {
        list.Clear();
        list = new List<T>() { x, y, z };

        return list;
    }

    public void Assign()
    {
        x = list[0];
        y = list[1];
        z = list[2];
    }
    public void Assign(int index)
    {
        switch (index)
        {
            case 0:
                x = list[0];
                break;
            case 1:
                y = list[1];
                break;
            case 2:
                z = list[2];
                break;
        }
    }

    public void Assign(int index, T t)
    {
        switch (index)
        {
            case 0:
                x = t;
                break;
            case 1:
                y = t;
                break;
            case 2:
                z = t;
                break;
        }
    }

    public T IndexToEntity(int index)
    {
        switch (index)
        {
            case 0:
                return x;
            case 1:
                return y;
            case 2:
                return z;
        }

        Debug.Log("Indexが違います");
        return default;
    }

    public static int count
    {
        get { return 3; }
    }

    public List<T> List
    {
        get { return GetList(); }
    }
}
[Serializable]
public class Vec3Bool
{
    public bool x;
    public bool y;
    public bool z;

    public VecT<bool> ConvertToVecT()
    {
        VecT<bool> returnVecT = new VecT<bool>();
        returnVecT.x = x;
        returnVecT.y = y;
        returnVecT.z = z;

        return returnVecT;
    }
}

/// <summary>
/// Rangeとそれを評価する値を代入する値を含む
/// </summary>

[Serializable]
public class MinMax
{
    public void Initialize(float min, float max)
    {
        this.min = min;
        this.max = max;
    }
    public void Initialize(MinMax _minmax)
    {
        this.min = _minmax.min;
        this.max = _minmax.max;

    }
    public float min;
    public float max;

    public void Clear()
    {
        min = 0.0f;
        max = 0.0f;
    }

    public void SymmetryMax()
    {
        min = -max;
    }
    public void SymmetryMin()
    {
        max = -min;
    }

    public float half
    {
        get
        {
            return max / 2.0f;
        }
    }
}



public static class GetEnumList
{
    public static List<T> Get<T>() where T : Enum
    {
        List<T> retList = new List<T>();
        foreach(T value in Enum.GetValues(typeof(T)))
        {
            retList.Add(value);
        }

        return retList;
    }
}

public class JsonOperator
{
    public static string jsonDirectoryName = "Json";
    public static string jsonDirectoryPath
    {
        get
        {
            string _currentDir = Directory.GetCurrentDirectory();
            string _relativeJsonDirPath = Path.Combine(_currentDir, jsonDirectoryName);
            
            Directory.CreateDirectory(_relativeJsonDirPath);

            return _relativeJsonDirPath;
        }
    }

    /// <summary>
    /// 引数のファイル名のパスを返す
    /// </summary>
    /// <param name="_fileName"></param>
    /// <returns></returns>
    public static string JsonPath(string _fileName)
    {
        return Path.Combine(jsonDirectoryPath, _fileName);
    }

    /// <summary>
    /// 引数のファイル名が存在するか
    /// </summary>
    /// <param name="_fileName"></param>
    /// <returns></returns>
    public static bool ExistJson(string _fileName)
    {
        bool _result = File.Exists(JsonPath(_fileName));
        return _result;
    }


    public static void Write(object _object, string _fileName)
    {
        string _json = JsonUtility.ToJson(_object);
        string _filePath = Path.Combine(jsonDirectoryPath, _fileName);

        File.Delete(_filePath);

        using (FileStream _file = new FileStream(_filePath, FileMode.OpenOrCreate, FileAccess.Write))
        {
            using (StreamWriter _writer = new StreamWriter(_file))
            {
                _writer.Write(_json);
            }

        }
    }

    public static T Read<T>(string _fileName)
    {
        string _filePath = Path.Combine(jsonDirectoryPath, _fileName);
        using (FileStream _file = new FileStream(_filePath, FileMode.Open, FileAccess.Read))
        {
            using (StreamReader _reader = new StreamReader(_file))
            {
                string json = _reader.ReadToEnd();

                return JsonUtility.FromJson<T>(json);
            }

        }
    }

}