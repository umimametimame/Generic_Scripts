using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterCameraAndFourCorners : MonoBehaviour
{
    [field: SerializeField] public Camera center {  get; private set; }
    [field: SerializeField] public Transform top { get; private set; }
    [field: SerializeField] public Transform end { get; private set; }
    [field: SerializeField] public Transform right { get; private set; }
    [field: SerializeField] public Transform left { get; private set; }

    public void Uniform()
    {

    }
}
