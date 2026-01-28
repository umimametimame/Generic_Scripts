using AddUnityClass;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOperator_BaseTPS : MonoBehaviour
{
    [field: SerializeField] public Chara_Base3D chara { get; private set; }
    [field: SerializeField] public InputParam_BaseFrontier inputParam { get; private set; }
    public CameraController_SeeSaw cameraController;
    public Chara_LookAtBar lookAtBar;

    protected virtual void Start()
    {

    }
    protected virtual void Update()
    {
        Update_Camera();
    }


    public void Update_CharaAngle()
    {
        Vector2 _lookAngle = new Vector2(GetMoveVelocity_ConvertCamera.x, GetMoveVelocity_ConvertCamera.z);
        lookAtBar.AssignAngle(AddFunction.GetAngleFromVector2(_lookAngle));

    }

    protected void Update_Camera()
    {
        cameraController.ChangeSeeSawAngle(LookInputVelocity);
    }

    public Param_GamePadStick MoveStick
    {
        get
        {
            Param_GamePadStick _ret = inputParam.sticks.leftStick;

            return _ret;
        }
    }

    public Param_GamePadStick ViewStick
    {
        get
        {
            Param_GamePadStick _ret = inputParam.sticks.rightStick;

            return _ret;
        }
    }


    public Vector3 MoveInputVelocity
    {
        get
        {
            Vector3 _ret = ConvertStickInputTo3D.GetMoveVelocity(MoveStick.GetNormalizedStick);

            return _ret;
        }
    }

    public Vector3 GetMoveVelocity_ConvertCamera
    {
        get
        {
            Transform camera = cameraController.cam.transform;
            Vector3 _ret = Vector3.zero;
            _ret = ConvertStickInputTo3D.GetNormalizedMoveVelocity(camera.right, camera.forward, MoveInputVelocity);

            return _ret;
        }
    }
    public Vector3 LookInputVelocity
    {
        get
        {
            Vector3 _ret = ConvertStickInputTo3D.GetLookVelocity(ViewStick.GetNormalizedStick);
            return _ret;
        }
    }

}
