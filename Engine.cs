using AddUnityClass;
using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Engine : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [field: SerializeField] public SpriteRenderer img { get; private set; }
    [SerializeField] private GravityOperator gravityOperator = new GravityOperator();
    [field: SerializeField, NonEditable] public Vector3 velocityPlan {  get; set; }
    [field: SerializeField, NonEditable] public Vector3 beforeVelocity { get; private set; }
    [field: SerializeField, NonEditable] public Quaternion rotatePlan { get; set; }
    /// <summary>
    /// velocityPlanÇåvéZÇ∑ÇÈä÷êîÇìoò^Ç∑ÇÈ
    /// </summary>
    public Action velocityPlanAction { get; set; }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        gravityOperator.Initialize();
        gravityActive = true;

        velocityPlan = Vector3.zero;
        PlanReset();
    }

    private void Update()
    {
        PlanReset();
        VelocitySolution();
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
    /// velocityPlanÇòMÇÈä÷êîÇìoò^ÇµÅAÇªÇÃå„à⁄ìÆÇ≥ÇπÇÈ
    /// </summary>
    private Vector3 VelocitySolution()
    {
        velocityPlanAction?.Invoke();
        GravitySolution();
        //rb.rotation = rotatePlan;
        rb.velocity = velocityPlan;
        //Debug.LogWarning(velocityPlan);

        beforeVelocity = velocityPlan;
        return transform.position;
    }


    private void GravitySolution()
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