using UnityEngine;
using UnityEngine.InputSystem;

public class Param_Base : MonoBehaviour
{
    private PlayerInput playerInput;
    [NonEditable] public Param_GamePadSticks sticks = new Param_GamePadSticks();


    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        sticks.AssignInput_GamePad();
    }


    private void OnMove(InputValue _value)
    {
        
    }
    private void OnLook(InputValue _value)
    {

    }
}
