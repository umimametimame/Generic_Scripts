using AddClass;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ViewPoint���厲�ɂ���FPS���_����<br/>
/// ���삷��I�u�W�F�N�g�ɃA�^�b�`����
/// </summary>
public class FPSViewPoint : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Engine engine;
    [field: SerializeField, NonEditable] public EntityAndPlan<Vector2> inputViewPoint { get; set; }
    [field: SerializeField, NonEditable] public Vector3 viewPointPosPlan { get; set; }
    [SerializeField, NonEditable] private Vector3 beforePlan;

    [SerializeField] private Transform viewPointObject;
    [SerializeField] private MoveCircleSurface viewCircleHorizontal;
    [SerializeField] private MoveCircleSurface viewCircleVertical;
    [SerializeField] private ThresholdRatio verticalLimitter;

    [SerializeField] private CircleClamp norHorizontalCircle;
    [SerializeField] private Transform centerPos;

    private void Start()
    {
        Initialize();
        Reset();
    }
    public void Initialize()
    {
        viewCircleHorizontal.Initialize(viewPointObject);
        viewCircleVertical.Initialize(viewPointObject);
        verticalLimitter.Initialize();

        norHorizontalCircle.Initialize(centerPos.gameObject, viewPointObject.gameObject);

    }

    public void Reset()
    {
        cam.transform.eulerAngles = transform.eulerAngles;
    }

    private void Update()
    {
        //DirrectionManager();
        CameraContorller();
    }
    public void DirrectionManager()
    {
        // �����𐧌�
        inputViewPoint.Assign();

        Vector3 newPlan = Vector3.zero;
        //newPlan = Vector3.Scale(transform.forward, new Vector3(5, 5, 5));

        if (inputViewPoint.entity == Vector2.zero)
        {
            viewPointPosPlan = Vector3.zero;
            VerticalOffset(centerPos);


            newPlan.x += beforePlan.x;
            newPlan.z += beforePlan.z;
            newPlan.x += viewCircleHorizontal.NewPosUpdate(inputViewPoint.plan.x).x;
            newPlan.z += viewCircleHorizontal.NewPosUpdate(inputViewPoint.plan.x).z;

            viewPointPosPlan += newPlan;
        }
        else
        {
            viewPointPosPlan = Vector3.zero;
            VerticalOffset(centerPos);

            //newPlan.x = centerPos.position.x;
            //newPlan.z = centerPos.position.z;

            if(inputViewPoint.plan.x != 0.0f)
            {
                newPlan.x += viewCircleHorizontal.NewPosUpdate(inputViewPoint.plan.x).x;
                newPlan.z += viewCircleHorizontal.NewPosUpdate(inputViewPoint.plan.x).z;

            }

            viewCircleVertical.axis = transform.right;
            newPlan.y += viewCircleVertical.NewPosUpdate(-inputViewPoint.plan.y).y;
            //viewCircleVertical.Update(-inputViewPoint.plan.y);

            

            viewPointPosPlan += newPlan;
            beforePlan = viewPointPosPlan;
        }



        Debug.Log(viewPointPosPlan);

        viewPointObject.position = viewPointPosPlan;
        norHorizontalCircle.Limit();
    }
    public void CameraContorller()
    {
        // Camera��Rotation��ύX
        Vector3 newCamEuler = cam.transform.eulerAngles;
        newCamEuler.y += inputViewPoint.plan.x;
        newCamEuler.x += -inputViewPoint.plan.y;
        cam.transform.eulerAngles = newCamEuler;

        // Y���̎��_����
        if (cam.transform.eulerAngles.x != 0.0f)
        {
            verticalLimitter.Update(cam.transform.eulerAngles.x);
            if (verticalLimitter.reaching == true)  // ���_�̊p�x���͈͊O�Ȃ�
            {
                newCamEuler.x -= -inputViewPoint.plan.y;    // �Ȃ��������Ƃɂ���
            }
        }
        cam.transform.eulerAngles = newCamEuler;



        // Player��Rotation��ύX(Y���̂�)
        Vector3 newPlayerEuler = transform.eulerAngles;
        newPlayerEuler.y = newCamEuler.y;
        transform.eulerAngles = newPlayerEuler;

        

    }

    /// <summary>
    /// Y����Ǐ]����
    /// </summary>
    /// <param name="t1"></param>
    public void VerticalOffset(Transform t1)
    {
        Vector3 newViewPointPos = Vector3.zero;
        newViewPointPos.y = t1.gameObject.transform.position.y;

        viewPointPosPlan += newViewPointPos;
    }

    /// <summary>
    /// viewPointObject�̕�����������<br/>
    /// �����͌�������I�u�W�F�N�g�A�����������
    /// </summary>
    /// <param name="objTransform"></param>
    public void LookAtViewPoint(Transform objTransform, bool x = true, bool y = true, bool z = true)
    {
        Vector3 newRotation = objTransform.position;

        if(x == true) { newRotation.x = viewPointObject.position.x; }
        if(y == true) { newRotation.y = viewPointObject.position.y; }
        if(z == true) { newRotation.z = viewPointObject.position.z; }

        objTransform.LookAt(newRotation);
    }
}
