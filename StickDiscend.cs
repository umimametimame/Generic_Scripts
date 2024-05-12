using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 接地判定のあるColliderを持ったオブジェクトにアタッチ
/// </summary>
public class StickDiscend : MonoBehaviour
{
    [field: SerializeField] public Vector3 gravityVec { get; set; }
    [field: SerializeField] public Vector3 moveDirrection { get; set; }
    [field: SerializeField] public LayerMask mask { get; set; }

    private void Update()
    {
        
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if(collision.gameObject.layer == mask)
        {
            Vector3 normal = collision.contacts[0].normal;
            Vector3 dirrection = moveDirrection - Vector3.Dot(moveDirrection, normal) * normal;

            moveDirrection = dirrection.normalized;
        }
    }
}
