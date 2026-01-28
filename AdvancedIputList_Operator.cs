using AddUnityClass;
using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class AdvancedIputList_Operator <T> where T : Enum
{
    [field: SerializeField, NonEditable] public SerializedDictionary<T, AdvancedInput> inputDic { get; set; } = new SerializedDictionary<T, AdvancedInput>();
    public void Initialize()
    {
        inputDic.Clear();
    }

    public void Add(Interval interval, Action action, T _enum)
    {
        AdvancedInput newAdIn = new AdvancedInput();
        newAdIn.Initialize(interval);
        newAdIn.action += action;

        inputDic.Add(_enum, newAdIn);
    }

    public AdvancedInput Add(T _enum)
    {
        AdvancedInput _add = new AdvancedInput();
        inputDic.Add(_enum, _add);

        return inputDic[_enum];
    }

    public void Update()
    {
        List<AdvancedInput> newAdInList = new List<AdvancedInput>();
        List<float> inputTiming = new List<float>();
        AdvancedInput youngestAdIn = new AdvancedInput();

        foreach (var ad in inputDic)
        {
            ad.Value.Update();
            if(ad.Value.Executable == true)
            {
                newAdInList.Add(ad.Value);
                inputTiming.Add(ad.Value.inputTiming.time.value);
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
