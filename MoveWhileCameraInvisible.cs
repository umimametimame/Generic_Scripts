using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWhileCameraInvisible : MonoBehaviour
{
    [SerializeField] private Vector3 moveVector;
    [SerializeField] private float speed;
    [SerializeField] private bool finished;
    private Loader loader;

    private void Start()
    {
        finished = false;
        loader = GetComponent<Loader>();
    }
    private void Update()
    {
        if (finished == false)
        {
            transform.position += moveVector.normalized * speed;
        }
    }

    private void OnBecameInvisible()
    {
        finished = true;
        transform.position -= moveVector.normalized * speed;
        loader.FinishLoad();
    }

}
