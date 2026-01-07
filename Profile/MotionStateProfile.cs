using AddUnityClass;
using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MotionStateProfile", menuName = "ScriptableObject/MotionStateProfile")]
public class MotionStateProfile : ScriptableObject
{

    [field: SerializeField] public MotionType motionType { get; private set; }
    [field: SerializeField] public GeneralMotion state { get; private set; }
    /// <summary>
    /// Ç±ÇÃÉÇÅ[ÉVÉáÉìÇ™Ç«Ç§Ç‚Ç¡ÇƒäÑÇËçûÇﬁÇ©
    /// </summary>
    [field: SerializeField] public CutInType cutInType { get; private set; }
    [field: SerializeField] public Inertia inertia_ { get; private set; } = new Inertia();
    [field: SerializeField] public MotionStateProfile_Inertia inertia { get; private set; } = new MotionStateProfile_Inertia();
    [field: SerializeField] public List<MinMax> rangeBool { get; private set; }
    [field: SerializeField] public List<TransitionalProfile_List> motionStateValueList { get; private set; } = new List<TransitionalProfile_List>();
    [field: SerializeField] public float motionTime { get; private set; }

    public List<TransitionalMotionThreshold> GetTransitionalList
    {
        get
        {
            List<TransitionalMotionThreshold> returnList = new List<TransitionalMotionThreshold>();
            for (int i = 0; i < motionStateValueList.Count; i++)
            {
                TransitionalMotionThreshold addTransitional = new TransitionalMotionThreshold();
                addTransitional.beforeMotion = motionStateValueList[i].beforeMotion;
                addTransitional.thresholdRatio.Initialize(motionStateValueList[i].thresholdRange);
                returnList.Add(addTransitional);
            }

            return returnList;
        }
    }

    public Inertia Inertia
    {
        get
        {
            Inertia returnInertia = new Inertia();
            returnInertia.durationTime.interval = inertia.durationTime;
            returnInertia.ratioCurve = inertia.ratioCurve;
            returnInertia.Initialize();

            return returnInertia;
        }
    }

    [Serializable]
    public class TransitionalProfile_List
    {
        public GeneralMotion beforeMotion;
        public MinMax thresholdRange;
    }

    [Serializable]
    public class MotionStateProfile_Inertia
    {
        public float durationTime;
        public RatioCurve ratioCurve = new RatioCurve();
    }

}
