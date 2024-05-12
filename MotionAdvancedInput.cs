using AddClass;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MotionAdvancedInput
{
    [SerializeField] public List<AdvancedInput> advancedInput { get; set; } = new List<AdvancedInput>();
    public List<Motion> motion { get; set; } = new List<Motion>();

    public void Initialize(List<Interval> inputs)
    {
        for(int i = 0; i < inputs.Count; ++i)
        {
            advancedInput.Add(new AdvancedInput());
            advancedInput[i].Initialize(inputs[i]);
        }
    }

    public void Update()
    {

    }
}
