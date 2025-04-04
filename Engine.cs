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
    [field: SerializeField, NonEditable] public Quaternion rotatePlan { get; set; }
    public Action velocityPlanAction { get; set; }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        gravityOperator.Initialize();
        gravityActive = true;
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
    /// velocityPlanを弄る関数を登録し、その後移動させる
    /// </summary>
    private Vector3 VelocitySolution()
    {
        velocityPlanAction?.Invoke();
        GravitySolution();
        //rb.rotation = rotatePlan;
        rb.velocity = velocityPlan;

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
