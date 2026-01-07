using AddUnityClass;
using Fusion;
using UnityEngine;

public class CameraController_SeeSaw : NetworkBehaviour
{
    [field: SerializeField] public Camera cam { get; set; }
    /// <summary>
    /// 操作感度
    /// <br/>推奨3
    /// </summary>
    [SerializeField] private float sensitivity;
    [field: SerializeField, NonEditable] public EntityAndPlan<Vector2> inputViewPoint { get; set; }
    /// <summary>
    /// カメラが注視するオブジェクト
    /// </summary>
    [SerializeField] private Transform viewPointObject;
    [SerializeField] private VecRangeOperator angleLimit;
    [SerializeField] private Transform seesaw;

    private void Awake()
    {
        Initialize();
    }
    public void Initialize()
    {
        angleLimit.AssignProfile();
        Reset();
    }
    public void AssignFusion()
    {
        NetworkRunner _runner = GameObject.FindWithTag(Fusion_Connect.networkRunnerTag).GetComponent<NetworkRunner>();
        Debug.Log($"HasInputAutority {HasInputAuthority}\nHasStateAuthority {HasStateAuthority}");
        if (_runner.GameMode == GameMode.Shared)
        {
            if (HasStateAuthority == false)
            {
                cam.enabled = false;
            }
        }
        else
        {


            if (HasInputAuthority == false)
            {
                cam.enabled = false;
            }
        }
    }

    public void Reset()
    {
        cam.transform.eulerAngles = transform.eulerAngles;
    }
    private void Update()
    {
        if (sensitivity <= 0.0f)
        {
            Debug.LogError($"{this.GetType()}のsensitivityが0以下です");
        }
        UpdateAngleLimit();
        cam.transform.LookAt(viewPointObject);
    }
    public override void FixedUpdateNetwork()
    {
    }

    public void UpdateAngleLimit()
    {
        Vector3 newSeesawEuler = seesaw.localEulerAngles;
        newSeesawEuler.y += inputViewPoint.plan.x * sensitivity;
        newSeesawEuler.x -= inputViewPoint.plan.y * sensitivity;
        newSeesawEuler.x = AddFunction.GetNormalizedAngle(newSeesawEuler.x, -180, 180);


        newSeesawEuler = angleLimit.Update(newSeesawEuler);
        seesaw.localEulerAngles = newSeesawEuler;

    }

    public void InputZeroAssign()
    {
        inputViewPoint.PlanDefault();
    }

    /// <summary>
    /// 引数の角度にsensitivityのスピードで回転<br/>
    /// 通常はxとyのみの回転
    /// </summary>
    /// <param name="dirrection"></param>
    public void ChangeSeeSawAngle(Vector3 dirrection)
    {
        Vector3 newAngle;
        newAngle = seesaw.localEulerAngles;
        newAngle.x += dirrection.x * sensitivity;
        newAngle.y += dirrection.y * sensitivity;
        newAngle.z += dirrection.z * sensitivity;

        seesaw.localEulerAngles = newAngle;
    }

    /// <summary>
    /// Cameraの方を向かせる<br/>
    /// 引数は向かせるオブジェクト、向かせる方向
    /// </summary>
    /// <param name="objTransform"></param>
    public void AssignCamEulerAngle(Transform objTransform, bool x = true, bool y = true, bool z = true)
    {
        Vector3 newRotation = objTransform.eulerAngles;

        if (x == true) { newRotation.x = cam.transform.eulerAngles.x; }
        if (y == true) { newRotation.y = cam.transform.eulerAngles.y; }
        if (z == true) { newRotation.z = cam.transform.eulerAngles.z; }

        objTransform.eulerAngles = newRotation;
    }
}
