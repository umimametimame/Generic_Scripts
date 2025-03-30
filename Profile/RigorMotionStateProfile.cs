using AddClass;
using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MotionStateProfile", menuName = "ScriptableObject/RigorMotionStateProfile")]
public class RigorMotionStateProfile : ScriptableObject
{
    [field: SerializeField] public GeneralMotion state { get; private set; }
    /// <summary>
    /// ‚±‚Ìƒ‚[ƒVƒ‡ƒ“‚ª‚Ç‚¤‚â‚Á‚ÄŠ„‚è‚Ş‚©
    /// </summary>
    [field: SerializeField] public CutInType cutInType { get; private set; }

    [field: SerializeField] public List<TransitionalMotionThreshold> transitionalPeriod { get; set; }
    [field: SerializeField] public List<MinMax> rangeBool { get; private set; }
    [field: SerializeField] public List<Instancer> effectInstancers { get; private set; }
    [field: SerializeField] public float motionTime { get; private set; }
}
