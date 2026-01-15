using System;
using UnityEngine;



[Serializable]
public class GravityParam : MonoBehaviour
{
    [field: SerializeField] public GravityProfile gravityProfile { get; set; }
    [field: SerializeField] public Life gravityLife { get; set; }
    [field: SerializeField] public VariedTime time { get; private set; } = new VariedTime();
    /// <summary>
    /// 重力無効後、再度有効になるまでのインターバル
    /// </summary>
    [field: SerializeField] public Interval gravityActiveInterval { get; set; } = new Interval();
    private void Awake()
    {
        Initialize();
        gravityActiveInterval.Initialize(true, false);
    }

    private void Initialize()
    {
        if (gravityProfile == null) { return; }
        gravityLife.Initialize();
        gravityLife.enable += EnableAction;
        gravityLife.disable += DisableAction;
        Reset();
    }

    private void Reset()
    {
        time.Initialize();
    }

    private void Update()
    {
        if (gravityProfile == null)
        {
            //Debug.Log("Gravity");
            return;
        }
        gravityLife.Update();
    }
    public Vector3 CurrentGravityVelocity
    {
        get
        {
            Vector3 returnVec = Vector3.zero;
            if (gravityLife.IsEnable == true)
            {
                returnVec.x = gravityProfile.gravityScale.x * gravityProfile.easing.Evaluate(time.value);
                returnVec.y = gravityProfile.gravityScale.y * gravityProfile.easing.Evaluate(time.value);
                returnVec.z = gravityProfile.gravityScale.z * gravityProfile.easing.Evaluate(time.value);

            }
            return returnVec;
        }
    }
    public void Enable()
    {
        if (gravityActiveInterval.Reaching == true)
        {
            gravityLife.Enable();
        }
    }

    public void Disable()
    {
        gravityLife.Disable();
    }

    private void EnableAction()
    {
        time.Update();
        gravityActiveInterval.Reset();
    }
    private void DisableAction()
    {
        Reset();
        gravityActiveInterval.Update();
    }
}
