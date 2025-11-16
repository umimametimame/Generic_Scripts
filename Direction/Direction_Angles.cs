using UnityEngine;

public class Direction_Angles : Direction
{
    public Vector3 speedPerSec;
    private Vector3 initialValue;

    private void Start()
    {
        initialValue = gameObject.transform.eulerAngles;
    }

    private void Update()
    {
        Vector3 _newAngle = gameObject.transform.eulerAngles;
        _newAngle += speedPerSec * Time.deltaTime;
        gameObject.transform.eulerAngles = _newAngle;
    }

    private void OnEnable()
    {
        gameObject.transform.eulerAngles = initialValue;
    }

    private void OnDisable()
    {
        gameObject.transform.eulerAngles = initialValue;
    }
}
