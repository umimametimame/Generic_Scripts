using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自身を中心に移動範囲を円形に制限
/// </summary>
public class Clamp_Circle : MonoBehaviour
{

    private Transform center;
    [field: SerializeField] public Transform moveObject { get; set; }
    public Vector3 angle;
    [field: SerializeField] public float radius { get; private set; }

    /// <summary>
    /// 常に制限値
    /// </summary>
    public bool surface;
    private void Start()
    {
        center = transform;
        angle = angle.normalized;
    }
    private void Update()
    {
        Limit();
    }
    public void AssignPositionByCenter()
    {
        moveObject.transform.position = center.transform.position;
    }

    public void Limit()
    {
        Vector3 _newPos = moveObject.position;
        Vector3 _clamped = Vector3.ClampMagnitude(center.position, radius);

        //Vector3 _nor = moveObject.transform.position - center.transform.position;
        //_nor = _nor.normalized;
        //moveObject.transform.position = center.transform.position + _nor * radius;


    }

}