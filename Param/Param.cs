using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class Param_LeftRightSticks
{
    public Param_GamePadStick rightStick = new Param_GamePadStick();
    public Param_GamePadStick leftStick = new Param_GamePadStick();

    public List<Param_GamePadStick> GetSticks
    {
        get
        {
            List<Param_GamePadStick> _ret = new List<Param_GamePadStick> { rightStick, leftStick };
            return _ret;
        }
    }

    public void Initialize()
    {
        rightStick.Initialize();
        leftStick.Initialize();
    }

    public void Update()
    {
        rightStick.Update();
        leftStick.Update();
    }

    /// <summary>
    /// コントローラーの右スティックの入力
    /// </summary>
    public void AssignRightInput_GamePad()
    {
        rightStick.AssignStickValue(GetRightStick);
    }

    /// <summary>
    /// コントローラーの左スティックの入力
    /// </summary>
    public void AssignLeftInput_GamePad()
    {
        leftStick.AssignStickValue(GetLeftStick);
    }

    public void AssignInput_GamePad()
    {
        AssignRightInput_GamePad();
        AssignLeftInput_GamePad();
    }



    private Vector2 GetRightStick
    {
        get
        {
            Vector2 _ret = Vector2.zero;
            Gamepad _gamepad = Gamepad.current;

            _ret = _gamepad.rightStick.ReadValue();
            return _ret;
        }
    }


    private Vector2 GetLeftStick
    {
        get
        {
            Vector2 _ret = Vector2.zero;
            Gamepad _gamepad = Gamepad.current;

            _ret = _gamepad.leftStick.ReadValue();
            return _ret;
        }
    }


}

[Serializable]
public class Param_GamePadStick
{
    /// <summary>
    /// stickValueのmagnitudeがdeadZone以上なら入力と判定する
    /// </summary>
    [Range(0.0f,1.0f)]
    public float deadZone;

    [SerializeField, NonEditable] private Vector2 stickValue = Vector2.zero;
    public ValueChecker<bool> isInputtingStick { get; private set; } = new ValueChecker<bool>();

    public void Initialize()
    {
        stickValue = Vector2.zero;
        isInputtingStick.Initialize(IsInputting);
        isInputtingStick.changedAction += ()=>Debug.LogWarning("ChangeStick");
    }
    public void Update()
    {
        Debug.LogWarning($"StickUpdate {isInputtingStick.value} {isInputtingStick.beforeValue}");
        isInputtingStick.Update(IsInputting);
    }

    public void AssignStickValue(Vector2 _value)
    {
        stickValue = _value;
    }

    /// <summary>
    /// deadZoneを適用した値
    /// </summary>
    public Vector2 GetStickValue
    {
        get
        {
            Vector2 _ret = Vector2.zero;
            if (stickValue.magnitude >= deadZone)
            {
                _ret = stickValue;
            }
            else
            {
            }

                return _ret;
        }
    }

    public bool IsInputting
    {
        get
        {
            bool _ret = false;

            if (GetStickValue.magnitude > 0)
            {
                _ret = true;
            }

            return _ret;
        }
    }
    public Vector2 GetNormalizedStick
    {
        get
        {
            Vector2 _ret = GetStickValue;
            float _magnitude = _ret.magnitude;

            if (_magnitude > 0.0f)
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