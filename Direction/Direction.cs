using UnityEngine;

public class Direction : MonoBehaviour
{
    public Interval interval;

    private void Start()
    {
        interval.Initialize(false, false);
    }

    private void Update()
    {
        interval.Update();
    }
}
