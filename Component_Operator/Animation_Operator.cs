using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public class Animation_Operator : MonoBehaviour
{
    private Animator animator;
    [field: NonEditable] public int currentAnimationState = -1;
    private ValueChecker<int> animationStateChecker = new ValueChecker<int>();
    public static string AnimationStateIdx = nameof(AnimationStateIdx);

    private void Awake()
    {
        animationStateChecker.changedAction += ChangeAnimation_SetInteger;
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
        AssignConditions_Enum();
    }

    private void Update()
    {
        animationStateChecker.Update(currentAnimationState);
    }

    /// <summary>
    /// currentAnimationStateÇ©ÇÁAnimationÇçƒê∂Ç∑ÇÈ
    /// </summary>
    public void ChangeAnimation_SetInteger()
    {
        animator.SetInteger(AnimationStateIdx, currentAnimationState);
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
                UnityEditor.Animations.AnimatorState _state = new AnimatorState();

                foreach (AnimatorStateTransition _tra in _childStates.anyStateTransitions)
                {
                    _tra.conditions = Convert_MotionState.AssignCondition_Enum<GeneralMotion>(_tra.destinationState, AnimationStateIdx);
                    _tra.hasExitTime = false;
                }
            }
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
