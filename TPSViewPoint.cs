using AddUnityClass;
using Fusion;
using UnityEngine;

public class TPSViewPoint : NetworkBehaviour
{
    [field: SerializeField] public Camera cam { get; set; }
    [SerializeField] private float sensitivity;
    [field: SerializeField, NonEditable] public EntityAndPlan<Vector2> inputViewPoint { get; set; }
    [field: SerializeField, NonEditable] public Vector3 viewPointPosPlan { get; set; }
    [SerializeField, NonEditable] private Vector3 beforePlan;

    [SerializeField] private Transform viewPointObject;
    [SerializeField] private VecRangeOperator vertical;
    [SerializeField] private Transform seesaw;

    public override void Spawned()
    {
        AssignFusion();
        vertical.AssignProfile();
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
    }
    public override void FixedUpdateNetwork()
    {
        SeeSawController();
        cam.transform.LookAt(viewPointObject);
    }

    public void SeeSawController()
    {
        Vector3 newSeesawEuler = seesaw.localEulerAngles;
        newSeesawEuler.y += inputViewPoint.plan.x * sensitivity;
        newSeesawEuler.x -= inputViewPoint.plan.y * sensitivity;
        newSeesawEuler.x = AddFunction.GetNormalizedAngle(newSeesawEuler.x, -180, 180);


        newSeesawEuler = vertical.Update(newSeesawEuler);
        seesaw.localEulerAngles = newSeesawEuler;

    }

    /// <summary>
    /// 
    /// </summary>
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
