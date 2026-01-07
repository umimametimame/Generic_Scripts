using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// 別のオブジェクトにアタッチされたコライダーで関数を実行する
/// </summary>
public class ColliderChecker : MonoBehaviour
{
    public Collider thisCollider;
    [field: SerializeField] public Action<Collider> onTiggerEnterEvent;
    [field: SerializeField] public Action<Collider> onTriggerStayEvent;
    [field: SerializeField] public Action<Collider> onTriggerExitEvent;


    [field: SerializeField] public Action<Collision> onCollisionEnterEvent;
    [field: SerializeField] public Action<Collision> onCollisionStayEvent;
    [field: SerializeField] public Action<Collision> onCollisionExitEvent;

    private void Start()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        onTiggerEnterEvent?.Invoke(other); 
        
    }

    private void OnTriggerStay(Collider other)
    {
        onTriggerStayEvent?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        onTriggerExitEvent?.Invoke(other);
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.LogWarning($"OnEnter{transform.name} {other.transform.name}");
        onCollisionEnterEvent?.Invoke(other);
    }

    private void OnCollisionStay(Collision other)
    {
        Debug.LogWarning($"OnStay{transform.name} {other.transform.name}");
        onCollisionStayEvent?.Invoke(other);

    }

    private void OnCollisionExit(Collision other)
    {

        onCollisionExitEvent?.Invoke(other);
    }

}
