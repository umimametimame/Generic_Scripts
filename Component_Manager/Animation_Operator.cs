using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public class Animation_Operator : StateMachineBehaviour
{
    public AnimatorController controller;
    private AnimatorStateMachine stateMachine = new AnimatorStateMachine();
    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        Debug.Log("OnStateMachineEnter");   
    }
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        Debug.Log("OnStateEnter");
    }

    private void Awake()
    {
    }

    public static List<AnimatorStateInfo> GetAnimationClips(Animator _animator)
    {
        List<AnimatorStateInfo> _ret = new List<AnimatorStateInfo>();
        List<AnimatorClipInfo> _info = _animator.GetCurrentAnimatorClipInfo(0).ToList();
        List<AnimationClip> _clips = _animator.runtimeAnimatorController.animationClips.ToList();
        List<AnimatorControllerParameter> _parameters = _animator.parameters.ToList();
        List<AnimatorTransition> _transitions = new List<AnimatorTransition>();

        AnimatorStateMachine _stateMachine = new AnimatorStateMachine();








        for (int i = 0; i < _animator.layerCount; ++i)
        {
            _ret.Add(_animator.GetCurrentAnimatorStateInfo(i));
            Debug.Log(_ret[i].shortNameHash);
        }

        Debug.Log(_info.Count);
        for(int i = 0; i < _info.Count; ++i)
        {
            Debug.Log(_info[i].clip.name);
        }

        Debug.Log(_clips.Count);
        for (int i = 0; i < _clips.Count; ++i)
        {
            Debug.Log(_clips[i].name);
            
        }


        Debug.Log(_parameters.Count);
        for (int i = 0; i < _parameters.Count; ++i)
        {
            Debug.Log(_parameters[i].name);
        }


        return _ret;
    }

    public void AssignCondition_Enum<T>() where T : Enum
    {
        List<string> _enums = EnumOperator.GetStringList<T>();

    }
}
