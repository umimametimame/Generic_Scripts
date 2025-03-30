using AddClass;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// MoveActionをイベントとして持つ
/// </summary>
public class InputOperator : MonoBehaviour
{
    [field: SerializeField] public PlayerInput playerInput { get; private set; }
    [field: SerializeField] public List<InputVecOrFloat<Vector3>> vInputs = new List<InputVecOrFloat<Vector3>>();
    [field: SerializeField] public List<InputVecOrFloat<float>> fInputs = new List<InputVecOrFloat<float>>();
    [field: SerializeField, NonEditable] public InputVecOrFloat<Vector3> moveInput { get; private set; } = new InputVecOrFloat<Vector3>();
    [field: SerializeField, NonEditable] public InputVecOrFloat<Vector3> viewPointInput = new InputVecOrFloat<Vector3>();

    public void Initialize()
    {
        playerInput = GetComponent<PlayerInput>();

        moveInput.Initialize();
        foreach(var i in vInputs)
        {
            i.Initialize();
        }
        foreach (var i in fInputs)
        {
            i.Initialize();
        }

    }
    protected virtual void Update()
    {

        moveInput.Update();
        foreach (var i in vInputs)
        {
            i.Update();
        }
        foreach (var i in fInputs)
        {
            i.Update(); 
        }
    }

    public void SetInputsList()
    {

        TypeFinder t = gameObject.AddComponent<TypeFinder>();
        vInputs = t.GetAndInList<InputVecOrFloat<Vector3>>(GetType());

        fInputs = t.GetAndInList<InputVecOrFloat<float>>(GetType());
        Destroy(t);
    }

    public EntityAndPlan<Vector3> MoveInput
    {
        get { return moveInput.input; }
    }

    public float SumVector
    {
        get { return Mathf.Abs(moveInput.entity.x) + Mathf.Abs(moveInput.entity.y) + Mathf.Abs(moveInput.entity.z); }
    }

    #region PlayerInputEvent
    public void OnMove(InputValue value)
    {
        Vector3 newVec = Vector3.zero;
        newVec.x = value.Get<Vector2>().x;
        newVec.z = value.Get<Vector2>().y;

        moveInput.entity = newVec;

    }

    /// <summary>
    /// 自前のTPSViewPointではxとyが反転する
    /// </summary>
    /// <param name="value"></param>
    public void OnMoveViewPoint(InputValue value)
    {
        Vector3 newVec = Vector3.zero;
        newVec.x = -value.Get<Vector2>().y;
        newVec.y = value.Get<Vector2>().x;

        viewPointInput.entity = newVec;
    }
    #endregion

}


[Serializable]
public class Inputting
{
    public enum InputType
    {
        None,
        Vector3,
        Float,
    }
    [field: SerializeField, NonEditable] public bool inputting { get; protected set; }
    public virtual void Initialize() { }
    public virtual void Update() { }
}
/// <summary>
/// TはVector3またはfloat<br/>
/// EntityAndPlanとboolを含む
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable] public class InputVecOrFloat<T> : Inputting where T : struct
{
    [field: SerializeField, NonEditable] public EntityAndPlan<T> input { get; set; } = new EntityAndPlan<T>();
    [field: SerializeField] public ValueInRange floatRange { get; private set; } = new ValueInRange();
    [SerializeField, NonEditable] private InputType thisType;

    public override void Initialize()
    {
        if (typeof(T).Equals(typeof(Vector3)))
        {
            thisType = InputType.Vector3;
        }
        else if (typeof(T).Equals(typeof(float)))
        {
            thisType = InputType.Float;
        }

    }

    public override void Update()
    {
        inputting = false;

        switch (thisType)
        {
            case InputType.Vector3:
                if ((Vector3)(object)input.entity != Vector3.zero) 
                { 
                    inputting = true;
                    return;
                }
                break;

            case InputType.Float:
                if ((float)(object)input.entity != 0.0f) 
                { 
                    if(floatRange.min == 0.0f &&  floatRange.max == 0.0f)
                    {
                        inputting = true;
                    }
                    else
                    {
                        inputting = floatRange.IsInRange((float)(object)input.entity);
                    }
                    return;
                }
                break;

            default:
                break;
        }
    }

    public void Assign()
    {
        input.Assign();
    }

    public T entity
    {
        get { return input.entity; }
        set { input.entity = value; }
    }
    public T plan
    {
        get { return input.plan; }
        set { input.plan = value; }
    }
    
}