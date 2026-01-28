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
    [field: SerializeField] public Interval inputTiming { get; private set; } = new Interval();
    [field: SerializeField] public ValueChecker<float> value { get; private set; } = new ValueChecker<float>();
    public void Initialize(Interval interval)
    {
        inputTiming = interval;
        value.Initialize(0.0f);
    }

    public void OnInput(float _value)
    {
        value.Update(_value);
        inputTiming.Reset();
    }

    public void Update()
    {
        inputTiming.Update();
    }

    /// <summary>
    /// 発動可能かを返す
    /// </summary>
    public bool Executable
    {
        get
        {
            bool _ret = false;
            if (inputTiming.interval < 0)
            {
                if (value.value >= 1.0f)
                {
                    if (funcs.Invoke() == true)
                    {
                        _ret = true;
                    }
                }
            }
            else if(inputTiming.Reaching == false)
            {
                if (funcs.Invoke() == true)
                {
                    _ret = true;
                }

            }

            return _ret;
        }
    }


    public void FuncReset()
    {
        funcs = null;
    }
}
