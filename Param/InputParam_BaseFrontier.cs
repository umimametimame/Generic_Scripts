using UnityEngine;
using UnityEngine.InputSystem;

public class InputParam_BaseFrontier : InputParam_Base
{
    private PlayerInput playerInput;
    [field: SerializeField, NonEditable] public float sprint;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnSprint(InputValue _value)
    {
        sprint = _value.Get<float>();
    }
}
