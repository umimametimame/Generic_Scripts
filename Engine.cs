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
    [SerializeField] private GravityOperator gravityOperator = new GravityOperator();
    [field: SerializeField, NonEditable] public Vector3 velocityPlan {  get; set; }
    [field: SerializeField, NonEditable] public Vector3 beforeVelocity { get; private set; }
    [field: SerializeField, NonEditable] public Quaternion rotatePlan { get; set; }
    public Action velocityPlanAction { get; set; }
    public virtual void Start()
    {
        Initialize();
    }

    public virtual void Update()
    {
        if (HasStateAuthority == false)
        {
            //Debug.LogWarning("Engine");

        }
        Reset_Plan();
        Update_Velocity();
        if (HasStateAuthority == false)
        {
            //Debug.LogWarning("Engine Finish");

        }
    }

    public void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        gravityOperator.Initialize();
        gravityActive = true;

        velocityPlan = Vector3.zero;
        Reset_Plan();

    }

    public void SetGravity(GravityProfile gp)
    {
        gravityOperator.gravityProfile = gp;
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
        velocityPlanAction?.Invoke();
        Update_Gravity();
        rb.linearVelocity = velocityPlan;

        beforeVelocity = velocityPlan;
        return transform.position;
    }


    private void Update_Gravity()
    {
        gravityOperator.Update();
        velocityPlan += gravityOperator.currentGravity.plan;
    }

    public bool gravityActive
    {
        get { return gravityOperator.active; }
        set { gravityOperator.active = value; }
    }
}