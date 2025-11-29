using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class Param_GamePadSticks
{
    public Vector2 leftStick = Vector2.zero;
    public Vector2 rightStick = Vector2.zero;

    public Param_GamePadSticks()
    {
        leftStick = Vector2.zero;
        rightStick = Vector2.zero;
    }

    /// <summary>
    /// コントローラーの左スティックの入力
    /// </summary>
    public void AssignLeftInput_GamePad()
    {
        Gamepad _gamepad = Gamepad.current;
        Vector3 _newVec = Vector3.zero;
        _newVec.x = _gamepad.leftStick.ReadValue().x;
        _newVec.z = _gamepad.leftStick.ReadValue().y;

        leftStick = _newVec;
    }

    /// <summary>
    /// コントローラーの右スティックの入力
    /// </summary>
    public void AssignRightInput_GamePad()
    {
        Gamepad _gamepad = Gamepad.current;
        Vector3 _newVec = Vector3.zero;
        _newVec.x = _gamepad.rightStick.ReadValue().y * -1;
        _newVec.y = _gamepad.rightStick.ReadValue().x;

        rightStick = _newVec;
    }

    public void AssignInput_GamePad()
    {
        AssignLeftInput_GamePad();
        AssignRightInput_GamePad();
    }

    public static Vector3 GetRightStick
    {
        get
        {
            Gamepad _gamepad = Gamepad.current;

            return _gamepad.leftStick.ReadValue();
        }
    }


    public static Vector3 GetLeftStick
    {
        get
        {
            Gamepad _gamepad = Gamepad.current;

            return _gamepad.rightStick.ReadValue();
        }
    }
}