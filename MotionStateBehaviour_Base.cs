using AddUnityClass;
using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;


public class MotionStateBehaviour_Base<T> : MonoBehaviour where T:Enum
{
    [SerializeField, NonEditable] private Animator animator;
    [SerializeField, NonEditable] private Animation_Operator<T> animatorOperator = new Animation_Operator<T>();
    [field: SerializeField] public Chara_Base3D chara { get; protected set; }
    [field: SerializeField, NonEditable] public T currentMotionState { get; private set; }
    [field: SerializeField, NonEditable] public SerializedDictionary<PlayerMotion, MotionState_Base<PlayerMotion>> motionList = new SerializedDictionary<PlayerMotion, MotionState_Base<PlayerMotion>>();

    public SerializedDictionary<T, MotionStateFromToTriggerProfile<T>> triggerProfileDic = new SerializedDictionary<T, MotionStateFromToTriggerProfile<T>>();
    private void Start()
    {
    }
    private void Update()
    {
        UpdateMotionList();
        animatorOperator.Update();
    }

    protected void Initialize()
    {
        animator = GetComponent<Animator>();
        animatorOperator.Initialize(animator);

        currentMotionState = (T)(object)0;
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
    }
    void UpdateMotionList()
    {

        foreach (var _m in motionList)
        {
            if (_m.Key.Equals(currentMotionState))
            {
                _m.Value.enable = true;
                chara.AddAssignedMoveVelocity(_m.Value.velocity);
            }
            else
            {
                _m.Value.enable = false;
            }

            _m.Value.Update();
        }
    }

    public void UpdateCurrentState(T _newState)
    {
        bool _isTrigger = false;
        string _triggerName = "";
        foreach (var _trigger in triggerProfileDic)
        {
            _isTrigger = _trigger.Value.IsMatch(currentMotionState, _newState);
            _triggerName = _trigger.Value.triggerValue;
        }



        currentMotionState = _newState;
        if (_isTrigger == true)
        {
            animatorOperator.OnTrigger(_triggerName);
        }
        else if (_isTrigger == false)
        {

            animatorOperator.currentAnimationState = (int)(object)currentMotionState;
        }
    
    }

    public List<MotionState_Base<T>> MotionList
    {
        get
        {
            List<MotionState_Base<T>> _ret = new List<MotionState_Base<T>>();
            TypeFinder _finder = new TypeFinder();
            _ret = _finder.GetAndInList<MotionState_Base<T>>(typeof(MotionState_Base<T>));

            return _ret;
        }
    }

    public SerializedDictionary<T, MotionStateProfile<T>> ConvertProfileListToDic(List<MotionStateProfile<T>> _list)
    {
        SerializedDictionary<T, MotionStateProfile<T>> _ret = new SerializedDictionary<T, MotionStateProfile<T>>();

        for (int i = 0; i < _list.Count; ++i)
        {
            _ret.Add(_list[i].state, _list[i]);
        }

        return _ret;
    }

    private void OnAnimatorIK(int layerIndex)
    {
        float _weight = 1;
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, _weight);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, _weight);
        animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, _weight);
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, _weight);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, _weight);
        animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, _weight);

        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, _weight);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, _weight);
        animator.SetIKHintPositionWeight(AvatarIKHint.LeftKnee, _weight);
        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, _weight);
        animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, _weight);
        animator.SetIKHintPositionWeight(AvatarIKHint.RightKnee, _weight);
    }
}
