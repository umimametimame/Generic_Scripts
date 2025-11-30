using AddUnityClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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



public static class EnumOperator
{
    public static List<T> Get<T>() where T : Enum
    {
        List<T> _retList = new List<T>();
        foreach(T _value in Enum.GetValues(typeof(T)))
        {
            _retList.Add(_value);
        }

        return _retList;
    }

    /// <summary>
    /// 引数をintにキャストし、そこから連続しているenumのみList化して返す
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_startEnum"></param>
    /// <returns></returns>
    public static List<T> GetSerialEntity<T>(T _startEnum) where T : Enum
    {
        // すべて取得
        List<T> _enums = Get<T>();

        // 返すList
        List<T> _retList = new List<T>();

        int _entity = 0;
        int _beforeValue = Convert.ToInt32(_startEnum);

        bool _start = false;
        bool _isSerial = false;

        for (int i = 0; i < _enums.Count; i++)
        {
            if(_start == false)
            {
                if (_startEnum.Equals(_enums[i]) == true)
                {
                    _start = true;
                }
            }
            else
            {
                _entity = Convert.ToInt32(_enums[i]);
                if ((_beforeValue + 1) == _entity)
                {
                    _isSerial = true;
                    _beforeValue = _entity;
                }
                else
                {
                    _isSerial = false;
                }

                if (_isSerial == true)
                {
                    _retList.Add(_enums[i]);
                }
                else
                {

                    return _retList;
                }
            }
        }
        return _retList;
    }

    /// <summary>
    /// 引数の1桁目と同じEnumをList化して返す
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_start"></param>
    /// <returns></returns>
    public static List<T> GetOver<T>(T _start) where T : Enum
    {
        int _startOver = Convert.ToInt32(_start) % 10;
        while (_startOver > 0)
        {

        }

        List<T> _retList = new List<T>();
        foreach (T _value in Enum.GetValues(typeof(T)))
        {
            int _over = Convert.ToInt32(_value) % 10;

            if(_startOver == _over)
            {
                _retList.Add(_value);
            }

        }

        return _retList;
    }

    public static List<T> GetOver10<T>() where T : Enum
    {
        T _start = IntToEnum<T>(10);
        return GetOver(_start);
    }

    /// <summary>
    /// 引数をintに変換し、以上未満で取得する
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_moreT"></param>
    /// <param name="_lessT"></param>
    /// <returns></returns>
    public static List<T> GetBetween<T>(T _moreT, T _lessT) where T : Enum
    {
        int _more = Convert.ToInt32(_moreT);
        int _less = Convert.ToInt32(_lessT);
        List<T> retList = new List<T>();
        foreach (T _valueT in Enum.GetValues(typeof(T)))
        {
            int _value = Convert.ToInt32(_valueT);
            if(_value >= _more)
            {
                if(_value < _less)
                {
                    retList.Add(_valueT);
                }
            }
        }

        return retList;
    }

    /// <summary>
    /// enum型をすべてstringにキャストして列挙する
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static List<string> GetStringList<T>() where T : Enum
    {
        List<string> _stringList = new List<string>();
        List<T> _enums = Get<T>();

        for(int i = 0; i < _enums.Count; i++)
        {
            _stringList.Add(_enums[i].ToString());
        }

        return _stringList;
    }

    public static List<string> GetStringList<T>(List<T> _enums) where T : Enum
    {

        List<string> _stringList = new List<string>();

        for (int i = 0; i < _enums.Count; i++)
        {
            _stringList.Add(_enums[i].ToString());
        }

        return _stringList;
    }

    public static T IntToEnum<T>(int value) where T : Enum
    {
        if (Enum.IsDefined(typeof(T), value))
        {
            return (T)Enum.ToObject(typeof(T), value);
        }
        else
        {
            // valueがEnumで定義されていない場合の処理
            // 例：例外をスローする、デフォルト値を返すなど
            throw new ArgumentException($"指定された値 {value} はEnum {typeof(T).Name} に定義されていません。");
        }
    }

    /// <summary>
    /// 引数以下の文字をカットしてList化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static List<string> GetStringExceptIdentifier<T>(string _exceptKeyword, List<T> _enums)where T : Enum
    {
        List<string> _strings = GetStringList(_enums);
        for (int i = 0; i < _strings.Count; i++)
        {
            // 見つからない場合、0
            int _num = _strings[i].IndexOf(_exceptKeyword) + 1;
            _strings[i] = GetStringExceptIdentifier_More(_exceptKeyword, _enums[i]);
        }

        return _strings;
    }

    public static List<string> GetStringExceptIdentifier<T>(string _exveptKeyword) where T : Enum
    {
        List<T> _enums = Get<T>();
        return GetStringExceptIdentifier(_exveptKeyword, _enums);
    }

    /// <summary>
    /// キーワード箇所より後を返す
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_exceptKeyword"></param>
    /// <param name="_enum"></param>
    /// <returns></returns>
    public static string GetStringExceptIdentifier_More<T>(string _exceptKeyword, T _enum) where T : Enum
    {
        string _string = _enum.ToString();
        // 見つからない場合、0
        int _num = _string.IndexOf(_exceptKeyword) + 1;
        _string = _string.Remove(0, _num);

        return _string;
    }

    /// <summary>
    /// キーワード箇所より前を返す
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_exceptKeyword"></param>
    /// <param name="_enum"></param>
    /// <returns></returns>
    public static string GetStringExceptIdentifier_Less<T>(string _exceptKeyword, T _enum) where T : Enum
    {
        string _string = _enum.ToString();
        // 見つからない場合、-1
        int _num = _string.IndexOf(_exceptKeyword);
        _string = _string.Remove(_num, _string.Length - _num);

        return _string;
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
                Debug.Log(_filePath);
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
                Debug.Log(_filePath);

                return JsonUtility.FromJson<T>(json);
            }

        }
    }

}




public class JsonFileName
{
    public static string UserData
    {
        get
        {
            return nameof(UserData) + FileExtension;
        }
    }

    public static string FileExtension
    {
        get
        {
            return ".json";
        }
    }
}
/// <summary>
/// スクリプト内のTをListにする
/// </summary>
public static class GetTypeInList 
{
    public static List<T> GetListClass<T> (object targetObject) where T : class
    {
        if (targetObject == null)
        {
            return new List<T>();
        }

        Type targetType = targetObject.GetType();
        List<T> fieldList = new List<T>();

        // すべてのインスタンスフィールドを取得 (パブリックとプライベート)
        FieldInfo[] fields = targetType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (FieldInfo field in fields)
        {
            if (field.FieldType == typeof(T))
            {
                // フィールドの値を指定された型 T として取得し、リストに追加
                if (field.GetValue(targetObject) is T fieldValue)
                {
                    fieldList.Add(fieldValue);
                }
            }
        }

        // すべての静的フィールドを取得 (パブリックとプライベート)
        FieldInfo[] staticFields = targetType.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (FieldInfo field in staticFields)
        {
            if (field.FieldType == typeof(T))
            {
                // 静的フィールドの値を取得するには、GetValueにnullを渡す
                if (field.GetValue(null) is T fieldValue)
                {
                    fieldList.Add(fieldValue);
                }
            }
        }

        return fieldList;
    }

    // ValueType (構造体) のジェネリック型に対応したオーバーロード
    public static List<T> GetListStruct<T>(object targetObject) where T : struct
    {
        if (targetObject == null)
        {
            return new List<T>();
        }

        Type targetType = targetObject.GetType();
        List<T> fieldList = new List<T>();

        // すべてのインスタンスフィールドを取得 (パブリックとプライベート)
        FieldInfo[] fields = targetType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (FieldInfo field in fields)
        {
            if (field.FieldType == typeof(T))
            {
                // フィールドの値を指定された型 T として取得し、リストに追加
                object value = field.GetValue(targetObject);
                if (value != null && value.GetType() == typeof(T))
                {
                    fieldList.Add((T)value);
                }
            }
        }

        // すべての静的フィールドを取得 (パブリックとプライベート)
        FieldInfo[] staticFields = targetType.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (FieldInfo field in staticFields)
        {
            if (field.FieldType == typeof(T))
            {
                // 静的フィールドの値を取得するには、GetValueにnullを渡す
                object value = field.GetValue(null);
                if (value != null && value.GetType() == typeof(T))
                {
                    fieldList.Add((T)value);
                }
            }
        }

        return fieldList;
    }
}

public static class ListOperator
{

    public static List<T2> UpdateListByEnum<T1, T2>(List<T2> _ref) where T1 : Enum where T2 : class
    {
        List<T1> _enum = EnumOperator.Get<T1>();
        List<T2> _newList = new List<T2>();

        for (int i = 0; i < _enum.Count; i++)
        {
            bool _isAddNew = false;

            if (i >= _ref.Count)
            {
                _isAddNew = true;
            }

            if (_isAddNew == true)
            {
                _newList.Add(null);
            }
            else
            {
                _newList.Add(_ref[i]);
            }
        }

        return _newList;
    }
    public static List<T> UpdateListByEnum<T>(StringEnum _enum, List<T> _ref) where T : class
    {
        List<T> _newList = new List<T>();

        for (int i = 0; i < _enum.Count; i++)
        {
            bool _isAddNew = false;

            if (i >= _ref.Count)
            {
                _isAddNew = true;
            }

            if (_isAddNew == true)
            {
                _newList.Add(null);
            }
            else
            {
                _newList.Add(_ref[i]);
            }
        }

        return _newList;
    }
}

/// <summary>
/// 比較演算子
/// </summary>
public enum Comparison
{
    More,
    MoreOrEqual,
    Less,
    LessOrEqual,
}

/// <summary>
/// Action型拡張メソッド
/// </summary>
public static class MulticastDelegateExtensions
{
    public static int GetLength(this MulticastDelegate self)
    {
        if (self == null || self.GetInvocationList() == null)
        {
            return 0;
        }
        return self.GetInvocationList().Length;
    }
}

public class ProgressList
{
    [field: SerializeField, NonEditable] public List<Func<float>> progressList { get; private set; } = new List<Func<float>>();

    public void Initialize()
    {
        progressList.Clear();
    }

    public void AddProgress(Func<float> _func)
    {
        progressList.Add(_func);
    }

    public float value
    {
        get
        {
            if (progressList.Count == 0)
            {
                Debug.Log("Progress List 0");
                return 0.0f;
            }

            float _sum = 0.0f;
            float _ave = 0.0f;

            for (int i = 0; i < progressList.Count; i++)
            {
                _sum += progressList[i].Invoke();
            }

            _ave = _sum / progressList.Count;

            return _ave;
        }
    }
}

/// <summary>
/// 現在地と目標値のギャップ
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable]
public class EnumGap<T> where T : Enum
{
    [field: SerializeField, NonEditable] public T current { get; private set; }
    [field: SerializeField, NonEditable] public T target { get; private set; }
    [field: SerializeField, NonEditable] public T none { get; set; }

    public Action onChangeTarget;
    public Action onChangeCurrent;

    public void Initialize()
    {
        Assign(default, default, default);
        onChangeTarget = null;
        onChangeCurrent = null;
    }

    public void Assign(T _current, T _target, T _none)
    {
        current = _current;
        target = _target;
        none = _none;
    }

    /// <summary>
    /// 変更後にonTargetChange実行
    /// </summary>
    /// <param name="_value"></param>
    public void ChangeTarget(T _value)
    {
        target = _value;

        if (target.Equals(none) == true)
        {

        }
        else
        {
            onChangeTarget?.Invoke();
        }
    }

    /// <summary>
    /// 変更後にtargetと同じならonCurrentReachTarget実行
    /// </summary>
    /// <param name="_value"></param>
    public void ChangeCurrent(T _value)
    {
        current = _value;
        onChangeCurrent?.Invoke();
    }

    public bool isEqual
    {
        get
        {
            if (current.Equals(target) == true)
            {
                return true;
            }

            return false;
        }
    }
}