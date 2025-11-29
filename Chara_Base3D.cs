using UnityEngine;
using GenericChara;
using UnityEngine.InputSystem;

public class Chara_Base3D : Chara
{
    [SerializeField] Param_Base inputParam;
    private void Update()
    {
    }


    public Vector3 GetMoveInput(Vector2 _leftStickValue)
    {
        Vector3 _ret = Vector3.zero;
        _ret.x = _leftStickValue.x;
        _ret.z = _leftStickValue.y;

        return _ret;
    }


    public Vector3 GetLookInput(Vector2 _rightStickValue)
    {
        Vector3 _ret = Vector3.zero;
        _ret.x = _rightStickValue.y * -1;
        _ret.y = _rightStickValue.x;

        return _ret;
    }
}
