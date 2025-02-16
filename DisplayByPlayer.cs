using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayByPlayer : MonoBehaviour
{
    public Camera localCamera;
    public Canvas canvas;

    public int TargetDisplay
    {
        set
        {
            localCamera.targetDisplay = value;
            canvas.targetDisplay = value;
        }
    }
}
