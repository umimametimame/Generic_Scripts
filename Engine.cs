using AddUnityClass;
using Fusion;
using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Engine : NetworkBehaviour
{
    [SerializeField] private Rigidbody rb;
    [field: SerializeField] public SpriteRenderer img { get; private set; }
    public GravityParam gravityParam;
    [field: SerializeField, NonEditable] public Vector3 velocityPlan {  get; private set; }
    [field: SerializeField, NonEditable] public Vector3 beforeVelocity { get; private set; }
    [field: SerializeField, NonEditable] public Quaternion rotatePlan { get; private set; }
    public Func<Vector3> velocityPlanAction { get; set; }
    public Func<Vector3> rbPositionFunc { get; set; }
    public virtual void Start()
    {
        Initialize();
    }

    public virtual void Update()
    {
        if (HasStateAuthority == false)
        {

        }
        Reset_Plan();
        Update_Velocity();
        if (HasStateAuthority == false)
        {

        }
    }

    public void FixedUpdate()
    {

        if (rbPositionFunc != null)
        {
            rb.position += rbPositionFunc.Invoke();
        }
    }

    public void Initialize()
    {
        rb = GetComponent<Rigidbody>();


        velocityPlan = Vector3.zero;
        Reset_Plan();

    }

    public void AssignGravity(GravityProfile gp)
    {
        gravityParam.gravityProfile = gp;
    }

    public void Reset_Plan()
    {
        velocityPlan = Vector3.zero;
        rotatePlan = Quaternion.identity;
    }

    /// <summary>
    /// velocityPlanでRigidBodyを動かす
    /// </summary>
    /// <returns></returns>
    private Vector3 Update_Velocity()
    {
        velocityPlan += velocityPlanAction.Invoke();
        velocityPlan += gravityParam.CurrentGravityVelocity;
        rb.linearVelocity = velocityPlan;


        beforeVelocity = velocityPlan;
        return transform.position;
    }

    public void EnableGravity()
    {
        gravityParam.Enable();
    }
    public void DisableGravity()
    {
        gravityParam.Disable();
    }

}