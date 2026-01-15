using UnityEngine;
using GenericChara;
using UnityEngine.InputSystem;
using AddUnityClass;
public class Chara_Base3D : Chara
{
    private void Start()
    {
    }

    protected void Initialize_Base3D()
    {
        Initialize_BaseChara();
    }

    private void Update()
    {
    }

    protected void Update_Base3D()
    {
        Update_Parameter();

        assignSpeed = speed.entity;

    }

}
