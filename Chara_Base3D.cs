using UnityEngine;
using GenericChara;
using UnityEngine.InputSystem;

public class Chara_Base3D : Chara
{
    [SerializeField] private InputParam_Base inputParam;
    public CameraController_SeeSaw cameraController;


    private void Start()
    {
    }

    protected void Initialize_Base3D()
    {
        Initialize_BaseChara();
    }

    private void Update()
    {
    }

    protected void Update_Base3D()
    {
        Update_Parameter();

        cameraController.ChangeSeeSawAngle(LookInputVelocity);
        assignSpeed = speed.entity;
        AddAssignedMoveVelocity(GetMoveVelocity_ConvertCamera);

    }

    


    public Vector3 MoveInputVelocity
    {
        get
        {
            Vector3 _ret = ConvertStickInputTo3D.GetMoveVelocity(inputParam.sticks.GetLeftStick);
            
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
            Vector3 _ret = ConvertStickInputTo3D.GetLookVelocity(inputParam.sticks.GetRightStick);
            return _ret;
        }
    }

}
