using UnityEngine;

public class Chara_LookAtBar : MonoBehaviour
{
    public Transform viewPoint;
    public Transform rotateBar;
    public Transform rotateChara;
    private void Start()
    {
        
    }
    private void Update()
    {
        // yŽ²‚Ì‚ÝLookAt“K—p
        Vector3 _originAngle = rotateChara.eulerAngles;
        rotateChara.LookAt(viewPoint.position);
        _originAngle.y = rotateChara.eulerAngles.y;
        rotateChara.eulerAngles = _originAngle;

    }

    public void AssignAngle(float _newAngle)
    {
        if (_newAngle != 0)
        {
            Vector3 _newEulerAngle = rotateBar.eulerAngles;
            _newEulerAngle.y = _newAngle;
            rotateBar.eulerAngles = _newEulerAngle;
        }
    }
}
