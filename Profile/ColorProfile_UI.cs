using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AddUnityClass;

[CreateAssetMenu(fileName = "ColorProfile", menuName = "ScriptableObject/ColorProfile")]
public class ColorProfile_UI : ScriptableObject
{
    public SerializedDictionary<UITag,ColorWithUITag<UITag>> colors = new SerializedDictionary<UITag,ColorWithUITag<UITag>>();
    
}
