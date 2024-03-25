using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWhileCameraInvisible : MonoBehaviour
{
    [SerializeField] private Vector3 normalizedMoveVector;
    [SerializeField] private float speed;
    [field: SerializeField] public bool finished { get; private set; }

    private void Start()
    {
        finished = false;
    }
    private void Update()
    {
        if (finished == false)
        {
            transform.position += normalizedMoveVector.normalized * speed;
        }
    }

    private void OnBecameInvisible()
    {
        finished = true;
        transform.position -= normalizedMoveVector.normalized * speed;
    }



}
