using AddUnityClass;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public enum CutInType
{
    /// <summary>
    /// 特定のモーションを指定し、そこに割り込みを追加する場合
    /// </summary>
    Handy,
    /// <summary>
    /// 特定のモーションを指定し、そこからのみ割り込める場合
    /// </summary>
    ReverseHandy,
    /// <summary>
    /// 自身を含め全てから割り込める場合
    /// <br/>被弾時など
    /// </summary>
    IsAll,
    /// <summary>
    /// 自身以外の全てから割り込める場合
    /// </summary>
    OtherMyself,
}

public static class Convert_MotionState
{

    public static AnimatorCondition[] AssignCondition_Enum<T>(UnityEditor.Animations.AnimatorState _state, string _paramName) where T : Enum
    {
        List<string> _enums = EnumOperator.GetStringList<T>();
        List<AnimatorCondition> _retList = new List<AnimatorCondition>();
        for (int i = 0; i < _enums.Count; ++i)
        {
            if (_state.name == _enums[i])
            {
                AnimatorCondition _addCondition = new AnimatorCondition();
                _addCondition.mode = AnimatorConditionMode.Equals;
                _addCondition.parameter = _paramName;
                _addCondition.threshold = i;

                _retList.Add(_addCondition);
            }
        }

        return _retList.ToArray();
    }
}


[Serializable]
public class TransitionalMotionThreshold<T> where T : Enum
{
    /// <summary>
    /// beforeMotionのどこから派生できるか
    /// </summary>
    [field: SerializeField] public T beforeMotion { get; set; }
    [field: SerializeField] public Range thresholdRatio { get; set; } = new Range();

    public bool IsReaching
    {
        get
        {
            return thresholdRatio.IsReaching;
        }
    }
}

/// <summary>
/// モーション単位
/// <br/>継承先で処理記載
/// </summary>
[Serializable]
public class MotionState_Base<T> where T : Enum
{
    [field: SerializeField, NonEditable] public MotionType motionType { get; private set; }
    [field: SerializeField, NonEditable] public T state { get; protected set; }
    [field: SerializeField, NonEditable] public bool enable;
    /// <summary>
    /// モーション時間と現在地
    /// </summary>
    [field: SerializeField, NonEditable] public Interval motionTime { get; private set; } = new Interval();
    public CutInType cutInType;
    public Life life { get; private set; } = new Life();
    [field: SerializeField, NonEditable] public Vector3 velocity { get; protected set; }
    [field: SerializeField, NonEditable] public List<TransitionalMotionThreshold<T>> transitionalPeriod { get; set; }
    [field: SerializeField] public MotionStateProfile<T> profile { get; set; }

    public virtual void Initialize_Base()
    {
        life.Initialize();
        life.start += Reset;
        life.enable += motionTime.Update;
        motionTime.reachAction += life.Finish;
        velocity = Vector3.zero;
    }

    private void UpdateTransitionList()
    {
        for (int i = 0; i < transitionalPeriod.Count; ++i)
        {
            transitionalPeriod[i].thresholdRatio.Update(motionTime.Ratio);
        }
    }

    private void Reset()
    {
        motionTime.Reset();

    }

    /// <summary>
    /// 経過時間毎の処理
    /// </summary>
    [field: SerializeField, NonEditable] public List<Range> actionByTimeRange { get; set; } = new List<Range>();

    public void AssignProfile(MotionStateProfile<T> newProfile = null)
    {
        if (newProfile != null)
        {
            profile = newProfile;
        }

        if (profile != null)
        {
            state = profile.state;
            cutInType = profile.cutInType;

            // 手動で指定したカットイン以外なら
            if (cutInType != CutInType.Handy)
            {
                ConvertTransitionalList();
            }
            else
            {
                TransitionalMotionThreshold<T> add = new TransitionalMotionThreshold<T>();


                transitionalPeriod = profile.GetTransitionalList;
                actionByTimeRange = new List<Range>();
                for (int i = 0; i < profile.rangeBool.Count; ++i)
                {
                    Range newRange = new Range();
                    newRange.Initialize(profile.rangeBool[i]);
                    actionByTimeRange.Add(newRange);
                }
            }


            switch (profile.motionType)
            {
                case MotionType.Duration:
                    {
                        motionTime.Initialize(false, false, -1.0f);
                    }
                    break;

                case MotionType.Rigor:
                    {
                        motionTime.Initialize(false, true, profile.motionTime);
                        motionTime.reachAction += life.Finish;
                    }
                break;
            }



        }
    }
    private void ConvertTransitionalList()
    {
        List<PlayerMotion> states = ConvertEnums<PlayerMotion>.GetList();
        transitionalPeriod = new List<TransitionalMotionThreshold<T>>();

        TransitionalMotionThreshold<T> newTransitional = new TransitionalMotionThreshold<T>();
        switch (cutInType)
        {
            case CutInType.ReverseHandy:

                for (int i = 0; i < states.Count; ++i)
                {
                    newTransitional = new TransitionalMotionThreshold<T>();
                    newTransitional.thresholdRatio.Initialize(0, 1);
                    newTransitional.beforeMotion = (T)(object)states[i];
                    transitionalPeriod.Add(newTransitional);
                    for (int j = 0; j < profile.motionStateValueList.Count; j++)
                    {
                        if (states[i].Equals(profile.motionStateValueList[j].beforeMotion) == true)
                        {
                            transitionalPeriod.RemoveAt(transitionalPeriod.Count - 1);
                        }
                    }
                }
                break;
            case CutInType.IsAll:
                for (int i = 0; i < states.Count; ++i)
                {
                    newTransitional = new TransitionalMotionThreshold<T>();
                    newTransitional.beforeMotion = (T)(object)states[i];
                    newTransitional.thresholdRatio.Initialize(0, 1);
                    transitionalPeriod.Add(newTransitional);
                }
                break;

            case CutInType.OtherMyself:
                for (int i = 0; i < states.Count; ++i)
                {
                    newTransitional = new TransitionalMotionThreshold<T>();
                    if (states[i].Equals(state) == false)
                    {
                        newTransitional.beforeMotion = (T)(object)states[i];
                        newTransitional.thresholdRatio.Initialize(0, 1);
                        transitionalPeriod.Add(newTransitional);
                    }
                }
                break;
        }

    }

    public void Update()
    {
        if (enable == true)
        {
            life.Enable();
        }
        else
        {
            life.Disable();
        }
        life.Update();
        UpdateTransitionList();
    }
}


public enum MotionType
{
    /// <summary>
    /// ループ可能モーション
    /// </summary>
    Duration,
    /// <summary>
    /// 時限モーション
    /// </summary>
    Rigor,
}

public static class ConvertEnums<T> where T : Enum
{
    public static Dictionary<T, MotionState_Base<T>> GetDic()
    {
        Dictionary<T, MotionState_Base<T>> newDic = new Dictionary<T, MotionState_Base<T>>();
        foreach (T s in Enum.GetValues(typeof(T)))
        {
            newDic.Add(s, new MotionState_Base<T>());
        }

        return newDic;
    }

    public static List<T> GetList()
    {
        List<T> newList = new List<T>();
        foreach (T t in Enum.GetValues(typeof(T)))
        {
            newList.Add(t);
        }

        return newList;

    }
}