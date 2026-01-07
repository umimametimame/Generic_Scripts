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
public class TransitionalMotionThreshold
{
    /// <summary>
    /// beforeMotionのどこから派生できるか
    /// </summary>
    [field: SerializeField] public GeneralMotion beforeMotion { get; set; }
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
public class MotionState_Base
{
    [field: SerializeField, NonEditable] public MotionType motionType { get; private set; }
    [field: SerializeField, NonEditable] public GeneralMotion state { get; protected set; }
    [field: SerializeField, NonEditable] public bool enable;
    /// <summary>
    /// モーション時間と現在地
    /// </summary>
    [field: SerializeField, NonEditable] public Interval motionTime { get; private set; }
    public CutInType cutInType;
    public Life exist { get; private set; } = new Life();
    [field: SerializeField, NonEditable] public List<TransitionalMotionThreshold> transitionalPeriod { get; set; }
    [field: SerializeField] public MotionStateProfile profile { get; set; }

    public void Initialize_Base()
    {
        AssignProfile();
        exist.Initialize();
        exist.start += Reset;
        exist.enable += motionTime.Update;
        motionTime.reachAction += exist.Finish;
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

    public void AssignProfile(MotionStateProfile newProfile = null)
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
                TransitionalMotionThreshold add = new TransitionalMotionThreshold();


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
                        motionTime.reachAction += exist.Finish;
                    }
                break;
            }



        }
    }
    private void ConvertTransitionalList()
    {
        List<GeneralMotion> states = ConvertEnums<GeneralMotion>.GetList();
        transitionalPeriod = new List<TransitionalMotionThreshold>();

        TransitionalMotionThreshold newTransitional = new TransitionalMotionThreshold();
        switch (cutInType)
        {
            case CutInType.ReverseHandy:

                for (int i = 0; i < states.Count; ++i)
                {
                    newTransitional = new TransitionalMotionThreshold();
                    newTransitional.thresholdRatio.Initialize(0, 1);
                    newTransitional.beforeMotion = states[i];
                    transitionalPeriod.Add(newTransitional);
                    for (int j = 0; j < profile.motionStateValueList.Count; j++)
                    {
                        if (states[i] == profile.motionStateValueList[j].beforeMotion)
                        {
                            transitionalPeriod.RemoveAt(transitionalPeriod.Count - 1);
                        }
                    }
                }
                break;
            case CutInType.IsAll:
                for (int i = 0; i < states.Count; ++i)
                {
                    newTransitional = new TransitionalMotionThreshold();
                    newTransitional.beforeMotion = states[i];
                    newTransitional.thresholdRatio.Initialize(0, 1);
                    transitionalPeriod.Add(newTransitional);
                }
                break;

            case CutInType.OtherMyself:
                for (int i = 0; i < states.Count; ++i)
                {
                    newTransitional = new TransitionalMotionThreshold();
                    if (states[i] != state)
                    {
                        newTransitional.beforeMotion = states[i];
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
            exist.Enable();
        }
        else
        {
            exist.Disable();
        }
        exist.Update();
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

public static class ConvertEnums<T1> where T1 : Enum
{
    public static Dictionary<T1, MotionState_Base> GetDic()
    {
        Dictionary<T1, MotionState_Base> newDic = new Dictionary<T1, MotionState_Base>();
        foreach (T1 s in Enum.GetValues(typeof(T1)))
        {
            newDic.Add(s, new MotionState_Base());
        }

        return newDic;
    }

    public static List<T1> GetList()
    {
        List<T1> newList = new List<T1>();
        foreach (T1 t in Enum.GetValues(typeof(T1)))
        {
            newList.Add(t);
        }

        return newList;

    }
}