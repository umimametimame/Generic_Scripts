using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Gauge_SceneLoad : MonoBehaviour
{
    private Slider gauge;
    [field: SerializeField, NonEditable] public Func<float> guageValueAction { get; private set; } 
    private void Start()
    {
        gauge = GetComponent<Slider>();
    }

    private void Update()
    {
        gauge.value = guageValueAction.Invoke();
    }

    public void Assign(Func<float> _guageValueAction)
    {
        guageValueAction += _guageValueAction;
    }
}
