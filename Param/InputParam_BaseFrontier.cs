using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputParam_BaseFrontier : InputParam_Base
{
    private PlayerInput playerInput;
    [field: SerializeField, NonEditable] public AdvancedIputList_Operator<PlayerMotion> inputValues { get; private set; } = new AdvancedIputList_Operator<PlayerMotion>();

    protected override void Awake()
    {
        base.Awake();
        List<PlayerMotion> _enums = EnumOperator.Get<PlayerMotion>();
        inputValues = GetAdvancedInput_ValueInputMode(_enums);
    }
    protected override void Start()
    {
        base.Start();
        playerInput = GetComponent<PlayerInput>();
    }
    protected override void Update()
    {
        base.Update();
        inputValues.Update();
    }

    private void OnSprint(InputValue _value)
    {
        PlayerMotion _motionIdx = PlayerMotion.Sprint_Value;
        inputValues.inputDic[_motionIdx].OnInput(_value.Get<float>());
    }
}
