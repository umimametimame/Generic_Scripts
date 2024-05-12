using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmosRayCast : MonoBehaviour
{
    [field: SerializeField, NonEditable] public bool hitting { get; private set; }
    [field: SerializeField] public Transform pos { get; set; }
    [field: SerializeField] public float size { get; set; }
    [field: SerializeField] public Vector3 direction { get; set; }
    [field: SerializeField] public float distance { get; set; }
    [field: SerializeField] public LayerMask targetLayer { get; set; }
    public RaycastHit hit;

    [field: SerializeField] public Color color { get; set; }
    [field: SerializeField] public Color hitColor { get; set; }
    [field: SerializeField] public bool penetrait { get; set; }

    public enum DrawType
    {
        Ray,
        Box,
        Sphere,
    }
    [field: SerializeField] public DrawType drawType { get; set; }

    private void OnDrawGizmos()
    {
        Vector3 norDirection = direction.normalized;   // ŠÛ‚ß‚é
        Ray ray = new Ray(pos.position, norDirection);


        if (Physics.SphereCast(ray, size, out hit, distance, targetLayer))
        {
            hitting = true;
            if (penetrait == true)
            {
                DrawFullyExtendedGizmos();
            }
            DrawGizmos();
        }
        else
        {
            hitting = false;
            DrawFullyExtendedGizmos();

        }

        void DrawGizmos()
        {
            Gizmos.color = hitColor;

            switch (drawType)
            {
                case DrawType.Ray:

                    Gizmos.DrawRay(ray.origin, ray.direction * hit.distance);
                    break;

                case DrawType.Box:

                    Gizmos.DrawRay(ray.origin, ray.direction * hit.distance);
                    Gizmos.DrawWireCube(ray.origin + ray.direction * hit.distance, new Vector3(size, size, size) * 2.0f);
                    break;

                case DrawType.Sphere:

                    Gizmos.DrawRay(ray.origin, ray.direction * hit.distance);
                    Gizmos.DrawWireSphere(ray.origin + ray.direction * hit.distance, size);
                    break;
            }
        }


        void DrawFullyExtendedGizmos()
        {
            Gizmos.color = color;

            switch (drawType)
            {
                case DrawType.Ray:

                    Gizmos.DrawRay(ray.origin, ray.direction * distance);
                    break;

                case DrawType.Box:

                    Gizmos.DrawRay(ray.origin, ray.direction * distance);
                    Gizmos.DrawWireCube(ray.origin + ray.direction * distance, new Vector3(size, size, size) * 2.0f);
                    break;

                case DrawType.Sphere:

                    Gizmos.DrawRay(ray.origin, ray.direction * distance);
                    Gizmos.DrawWireSphere(ray.origin + ray.direction * distance, size);
                    break;
            }
        }
    }


}

