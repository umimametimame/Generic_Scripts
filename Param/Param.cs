using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class Param_GamePadSticks
{
    public Vector2 rightStick = Vector2.zero;
    public Vector2 leftStick = Vector2.zero;

    public Param_GamePadSticks()
    {
        rightStick = Vector2.zero;
        leftStick = Vector2.zero;
    }


    /// <summary>
    /// コントローラーの右スティックの入力
    /// </summary>
    public void AssignRightInput_GamePad()
    {
        rightStick = GetRightStick;
    }

    /// <summary>
    /// コントローラーの左スティックの入力
    /// </summary>
    public void AssignLeftInput_GamePad()
    {
        leftStick = GetLeftStick;
    }

    public void AssignInput_GamePad()
    {
        AssignRightInput_GamePad();
        AssignLeftInput_GamePad();
    }

    public Vector2 GetRightStick
    {
        get
        {
            Gamepad _gamepad = Gamepad.current;

            return _gamepad.rightStick.ReadValue();
        }
    }


    public Vector2 GetLeftStick
    {
        get
        {
            Gamepad _gamepad = Gamepad.current;

            return _gamepad.leftStick.ReadValue();
        }
    }
}