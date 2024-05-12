using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// �ʂ̃I�u�W�F�N�g�ɃA�^�b�`���ꂽ�R���C�_�[�Ŋ֐������s�ł���
/// </summary>
public class ColliderChecker : MonoBehaviour
{
    [field: SerializeField] public Collider thisCollider;
    [field: SerializeField] public UnityEvent<Collider> triggerEnterEvent;
    [field: SerializeField] public UnityEvent<Collider> triggerStayEvent;
    [field: SerializeField] public UnityEvent<Collider> triggerExitEvent;


    /// <summary>
    /// Collision�������ɂ���ꍇ�ARigidbody��Collider�Ɠ����ꏊ�ɕK�v
    /// </summary>
    [field: SerializeField] public UnityEvent<Collision> collisionEnterEvent;
    [field: SerializeField] public UnityEvent<Collision> collisionStayEvent;
    [field: SerializeField] public UnityEvent<Collision> collisionExitEvent;
    private void OnTriggerEnter(Collider other)
    {
        if (thisCollider.isTrigger == false) { return; }
        triggerEnterEvent.Invoke(other); 
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (thisCollider.isTrigger == false) { return; }
        triggerStayEvent.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {

        if (thisCollider.isTrigger == false) { return; }
        triggerExitEvent.Invoke(other);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (thisCollider.isTrigger == true) { return; }
        collisionEnterEvent.Invoke(other);
    }

    private void OnCollisionStay(Collision other)
    {
        if (thisCollider.isTrigger == true) { return; }
        collisionStayEvent.Invoke(other);

    }

    private void OnCollisionExit(Collision other)
    {

        if (thisCollider.isTrigger == true) { return; }
        collisionExitEvent.Invoke(other);
    }
}
