using AddClass;
using UnityEngine;

public class TPSViewPoint : MonoBehaviour
{
    [field: SerializeField] public Camera cam { get; set; }
    [SerializeField] private Engine engine;
    [SerializeField] private float sensitivity;
    [field: SerializeField, NonEditable] public EntityAndPlan<Vector2> inputViewPoint { get; set; }
    [field: SerializeField, NonEditable] public Vector3 viewPointPosPlan { get; set; }
    [SerializeField, NonEditable] private Vector3 beforePlan;

    [SerializeField] private Transform viewPointObject;
    [SerializeField] private VecRangeOperator vertical;
    [SerializeField] private Transform seesaw;

    private void Start()
    {
        Initialize();
        vertical.AssignProfile();
        Reset();
    }
    public void Initialize()
    {
    }

    public void Reset()
    {
        cam.transform.eulerAngles = transform.eulerAngles;
    }

    public void Update()
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
    /// �����̊p�x��sensitivity�̃X�s�[�h�ŉ�]<br/>
    /// �ʏ��x��y�݂̂̉�]
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
    /// Camera�̕�����������<br/>
    /// �����͌�������I�u�W�F�N�g�A�����������
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
