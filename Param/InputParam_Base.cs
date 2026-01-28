using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputParam_Base : MonoBehaviour
{
    private PlayerInput playerInput;
    public Param_LeftRightSticks sticks = new Param_LeftRightSticks();

    protected virtual void Awake()
    {
        Initialize_Base();
    }
    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        Update_Base();
    }

    protected void Initialize_Base()
    {

        playerInput = GetComponent<PlayerInput>();
        sticks.Initialize();
    }
    protected void Update_Base()
    {
        sticks.AssignInput_GamePad();
        sticks.Update();
    }


    private void OnMove(InputValue _value)
    {
        
    }
    private void OnLook(InputValue _value)
    {

    }

    public AdvancedIputList_Operator<T> GetAdvancedInput_ValueInputMode<T>(List<T> _enums) where T : Enum
    {
        AdvancedIputList_Operator<T> _ret = new AdvancedIputList_Operator<T>();
        _ret.Initialize();
        SerializedDictionary<T,InputMode> _inputModes = GetInputModeList(_enums);

        for (int i = 0; i < _enums.Count; ++i)
        {
            T _key = _enums[i];
            switch (_inputModes[_key])
            {
                case InputMode.Button:
                    {
                        _ret.Add(_enums[i]);
                    }
                break;

                case InputMode.Value:
                    {
                        Interval _newInterval = new Interval();
                        _newInterval.Initialize(false, false);
                        _newInterval.interval = -1.0f;
                        _ret.Add(_enums[i]).Initialize(_newInterval);
                    }
                break;

                default:
                    {

                    }
                break;
            }
        }

        return _ret;
    }

    /// <summary>
    /// Enum‚ÌList‚©‚ç“ü—Í•û–@‚ð”»•Ê‚·‚éList‚ð•Ô‚·
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_enums"></param>
    /// <returns></returns>
    public SerializedDictionary<T,InputMode> GetInputModeList<T>(List<T> _enums) where T : Enum
    {
        SerializedDictionary<T,InputMode> _ret = new SerializedDictionary<T,InputMode>();
        List<string> _enumstrings = EnumOperator.GetStringList<T>();

        for (int i = 0; i < _enumstrings.Count; ++i)
        {
            InputMode _add = InputMode.None;
            for (int j = 0; j < InputModeList.Count; ++j)
            {
                if (_enumstrings[i].Contains(InputModeList[j].ToString()) == true)
                {
                    _add = InputModeList[j];
                }
            }

            _ret.Add(_enums[i],_add);
        }

        return _ret;
    }

    public List<InputMode> InputModeList
    {
        get
        {
            List<InputMode> _ret = new List<InputMode> ();
            _ret = EnumOperator.Get<InputMode>();

            return _ret;
        }
    }
}

public enum InputMode
{
    None,
    Stick,
    Button,
    Value,
}