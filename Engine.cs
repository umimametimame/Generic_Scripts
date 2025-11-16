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
    /// <summary>
    /// velocityPlan���v�Z����֐���o�^����
    /// </summary>
    public Action velocityPlanAction { get; set; }
    public override void Spawned()
    {
        rb = GetComponent<Rigidbody>();
        gravityOperator.Initialize();
        gravityActive = true;

        velocityPlan = Vector3.zero;
        PlanReset();
    }

    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority == false)
        {
            //Debug.LogWarning("Engine");

        }
        PlanReset();
        VelocitySolution();
        if (HasStateAuthority == false)
        {
            //Debug.LogWarning("Engine Finish");

        }
    }

    public void SetGravity(GravityProfile gp)
    {
        gravityOperator.gravityProfile = gp;
    }

    public void PlanReset()
    {
        velocityPlan = Vector3.zero;
        rotatePlan = Quaternion.identity;
    }

    /// <summary>
    /// velocityPlan��M��֐���o�^���A���̌�ړ�������
    /// </summary>
    private Vector3 VelocitySolution()
    {
        velocityPlanAction?.Invoke();
        GravitySolution();
        //rb.rotation = rotatePlan;
        rb.linearVelocity = velocityPlan;
        //if (velocityPlan != Vector3.zero)
        //    Debug.LogWarning(velocityPlan);

        beforeVelocity = velocityPlan;
        return transform.position;
    }


    private void GravitySolution()
    {
        gravityOperator.Update();
        velocityPlan += gravityOperator.currentGravity.plan;
        if (HasInputAuthority == true)
        {
            //Debug.Log(gravityOperator.currentGravity.plan);
        }
    }

    public bool gravityActive
    {
        get { return gravityOperator.active; }
        set { gravityOperator.active = value; }
    }
}