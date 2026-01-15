using AddUnityClass;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 入力を指定時間維持し、条件達成で発動
/// </summary>
[Serializable] public class AdvancedInput
{
    public BoolFuncs funcs { get; set; } = new BoolFuncs();
    public Action action { get; set; }
    
    public Interval input { get; private set; } = new Interval();
    public bool enable;


    public void Initialize(Interval interval)
    {
        input = interval;
    }
    /// <summary>
    /// 発動可能かを返す
    /// </summary>
    public bool Executable
    {
        get
        {
            if(!input.Reaching == true)
            {
                if (funcs.Invoke() == true)
                {
                    return true;
                }

            }

            return false;
        }
    }

    public void FuncReset()
    {
        funcs = null;
    }
}
