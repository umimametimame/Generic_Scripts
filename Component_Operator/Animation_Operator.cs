using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

[Serializable]
public class Animation_Operator<T> where T:Enum
{
    private Animator animator;
    [field: NonEditable] public int currentAnimationState = -1;
    private ValueChecker<int> animationStateChecker = new ValueChecker<int>();
    private string animationTriggerName = "";
    public static string AnimationStateIdx = nameof(AnimationStateIdx);

    public void Initialize(Animator _animator)
    {
        animator = _animator;
        AssignConditions_Enum();
        animationStateChecker.Initialize(currentAnimationState);
        animationStateChecker.changedAction += OnChangeAnimationState_SetInteger;
        animationTriggerName = "";
    }

    public void Update()
    {
        if (IsAnimationTrigger == true)
        {
            animator.SetTrigger(animationTriggerName);
            animationTriggerName = "";
        }
        animationStateChecker.Update(currentAnimationState);
    }

    /// <summary>
    /// currentAnimationStateÇ©ÇÁAnimationÇçƒê∂Ç∑ÇÈ
    /// </summary>
    public void OnChangeAnimationState_SetInteger()
    {
        animator.SetInteger(AnimationStateIdx, currentAnimationState);
    }

    public void OnTrigger(string _triggerName)
    {
        Debug.Log($"{_triggerName} Trigger");
        animationTriggerName = _triggerName;
    }
    [ContextMenu(nameof(AssignConditions_Enum))]
    public void AssignConditions_Enum()
    {
        AnimatorController controller = (AnimatorController)animator.runtimeAnimatorController;
        List<AnimatorControllerLayer> _layers = controller.layers.ToList();


        if(CheckExistenceStateParam() == true)
        {
            for (int i = 0; i < _layers.Count; ++i)
            {
                AnimatorStateMachine _childStates = _layers[i].stateMachine;

                // anyStateÇ©ÇÁÇÃTransition
                foreach (AnimatorStateTransition _tra in _childStates.anyStateTransitions)
                {
                    _tra.conditions = Convert_MotionState.AssignCondition_Enum<T>(_tra.destinationState, AnimationStateIdx);
                    _tra.hasExitTime = false;
                    _tra.canTransitionToSelf = false;
                }
            }
        }
    }

    public bool IsAnimationTrigger
    {
        get
        {
            bool _ret = false;

            if (animationTriggerName != "")
            {
                _ret = true;
            }

            return _ret;
        }
    }

    /// <summary>
    /// ä˘íËÇÃParametersÇ™AnimatorÇ…ë∂ç›Ç∑ÇÈÇ©
    /// </summary>
    /// <returns></returns>
    private bool CheckExistenceStateParam()
    {
        List<AnimatorControllerParameter> _parameters = animator.parameters.ToList();
        bool _existence = false;

        for (int i = 0; i < _parameters.Count; ++i)
        {
            if (_parameters[i].name == AnimationStateIdx)
            {
                _existence = true;
                break;
            }

        }

        if(_existence == false)
        {
            Debug.LogError($"AnimatorÇ…ä˘íËÇÃParameters {AnimationStateIdx} Ç™ë∂ç›ÇµÇ‹ÇπÇÒÅB");
        }

        return _existence;
    }


}
