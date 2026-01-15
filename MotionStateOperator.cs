using AYellowpaper.SerializedCollections;
using System;
using UnityEngine;
using UnityEngine.XR;


public class MotionStateOperator : MonoBehaviour
{
    private Animation_Operator animator;
    [field: SerializeField] public Chara_Base3D chara { get; protected set; }
    [field: SerializeField, NonEditable] public GeneralMotion currentMotionState { get; private set; }
    [field: SerializeField, NonEditable] public SerializedDictionary<GeneralMotion, MotionState_Base> motionList = new SerializedDictionary<GeneralMotion, MotionState_Base>();
    private void Awake()
    {
    }

    private void Start()
    {

    }
    private void Update()
    {
        UpdateMotionList();
    }

    protected void Initialize()
    {

        currentMotionState = 0;
        foreach (var _m in motionList)
        {
            _m.Value.Initialize_Base();
            switch (_m.Value.motionType)
            {
                case MotionType.Duration:
                    {
                    }
                    break;

                case MotionType.Rigor:
                    {
                    }
                    break;
            }
        }
        animator = GetComponent<Animation_Operator>();
    }
    void UpdateMotionList()
    {

        foreach (var _m in motionList)
        {
            if (_m.Key == currentMotionState)
            {
                _m.Value.enable = true;
                //Debug.Log($"{_m.Key} Active");
            }
            else
            {
                _m.Value.enable = false;
            }

            _m.Value.Update();
            chara.AddAssignedMoveVelocity(_m.Value.velocity);
        }
    }

    public void UpdateCurrentState(GeneralMotion _newState)
    {
        currentMotionState = _newState;
        animator.currentAnimationState = (int)currentMotionState;
    }

    public void AddMotion(MotionState_Base _motion)
    {

    }
}
