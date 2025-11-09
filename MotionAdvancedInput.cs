using AddUnityClass;
using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class MotionAdvancedInput <T> where T : Enum
{
    public SerializedDictionary<T, AdvancedInput> dicAdvanced { get; private set; } = new SerializedDictionary<T, AdvancedInput>();
    public void Initialize()
    {
        dicAdvanced.Clear();
    }

    public void Add(Interval interval, Action action, T _enum)
    {
        AdvancedInput newAdIn = new AdvancedInput();
        newAdIn.Initialize(interval);
        newAdIn.action += action;

        dicAdvanced.Add(_enum, newAdIn);
    }

    public void Update()
    {
        List<AdvancedInput> newAdInList = new List<AdvancedInput>();
        AdvancedInput youngestAdIn = new AdvancedInput();
        List<float> inputTiming = new List<float>();

        foreach (var ad in dicAdvanced)
        {
            if(ad.Value.Executable == true)
            {
                newAdInList.Add(ad.Value);
                inputTiming.Add(ad.Value.input.time.value);
            }
        }

        for(int i = 0; i < newAdInList.Count; ++i)
        {
            if (i == 0)
            {
                youngestAdIn = newAdInList[0];
            }
            // 2つ目以降のAdvancedInput
            else if (i > 0)
            {
                Debug.LogWarning("同時入力");
                // 入力のタイミングが若いなら
                if (inputTiming[0] >= inputTiming[i])
                {
                    // 代入する
                    youngestAdIn = newAdInList[i];
                    inputTiming[0] = inputTiming[i];
                }
            }
        }

        if(youngestAdIn.action != null)
        {
            youngestAdIn.action?.Invoke();
        }

    }
}
