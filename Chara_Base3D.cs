using UnityEngine;
using GenericChara;
using UnityEngine.InputSystem;

public class Chara_Base3D : Chara
{
    [SerializeField] Param_Base inputParam;
    public Animator animator;

    private void Start()
    {
        Initialize_BaseChara();
    }

    private void Update()
    {
        Update_Parameter();
        assignSpeed = speed.entity;
        AddAssignedMoveVelocity(MoveInputVelocity);
    }




    public Vector3 MoveInputVelocity
    {
        get
        {
            Vector3 _ret = ConvertStickInputTo3D.GetMoveVelocity(inputParam.sticks.GetLeftStick);
            return _ret;
        }
    }

    public Vector3 LookInputVelocity
    {
        get
        {
            Vector3 _ret = ConvertStickInputTo3D.GetLookVelocity(inputParam.sticks.GetRightStick);
            return _ret;
        }
    }

    protected void ChangeAnimation_SetInteger(string _paramName, int _value)
    {
        animator.SetInteger(_paramName, _value);
    }

}
