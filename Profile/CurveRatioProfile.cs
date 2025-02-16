using AddClass;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CurveProfile", menuName = "ScriptableObject/CurveProfile")]
public class CurveRatioProfile : ScriptableObject
{
    [field: SerializeField] public AnimationCurve curve { get; private set; }
    [field: SerializeField] public float multiplyValue { get; private set; }

}
