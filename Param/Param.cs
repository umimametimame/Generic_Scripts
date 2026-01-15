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

    public Vector2 GetNormalizedRightStick
    {
        get
        {
            Vector2 _ret = GetRightStick;
            float _velocitySum = _ret.magnitude;

            if (_velocitySum > 0.0f)
            {
                _ret = _ret.normalized;
            }
            else
            {
                _ret = Vector2.zero;
            }

            return _ret;
        }
    }

    public Vector2 GetNormalizedLeftStick
    {
        get
        {
            Vector2 _ret = GetLeftStick;
            float _velocitySum = _ret.magnitude;

            if (_velocitySum > 0.0f)
            {
                _ret = _ret.normalized;
            }
            else
            {
                _ret = Vector2.zero;
            }

            return _ret;
        }
    }
}