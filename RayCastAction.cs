using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RayCastAction : MonoBehaviour
{
    [field: SerializeField] public Vector3 normalizedRayAngle {  get; set; }
    public List<Collider> excludeColliders;
    public Color rayColor;
    public Color hitRayColor;

    /// <summary>
    /// •‰‚Ìê‡Ray’·‚Í–³ŒÀ‚Æ‚·‚é
    /// </summary>
    public float distance;
    [field: SerializeField, NonEditable] public bool isHit { get; private set; }
    public Action<RaycastHit> onHitClosestRayAction;
    public Action nonHitAction;
    public Ray ray { get; private set; }
    private RaycastHit hit;
    private void Start()
    {
        UpdateRay();
    }

    private void Update()
    {
        UpdateRay();
    }

    private void UpdateRay()
    {
        if (normalizedRayAngle == Vector3.zero)
        {
            Debug.LogError("Ray‚Ì•ûŒü‚ªŒˆ‚ß‚ç‚ê‚Ä‚¢‚Ü‚¹‚ñ");
        }

        ray = new Ray(transform.position, normalizedRayAngle.normalized);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        List<RaycastHit> _hits = Physics.RaycastAll(ray, distance).ToList();


        isHit = false;
        bool _isClosest = false;
        for (int i = 0; i < _hits.Count; ++i)
        {
            bool _isContain = false;
            _isContain = excludeColliders.Contains(_hits[i].collider);
            if (_isContain == false)
            {
                isHit = true;
                if (_isClosest == false)
                {
                    _isClosest = true;
                    onHitClosestRayAction?.Invoke(_hits[i]);
                }

            }
            else
            {

            }
        }

        if (isHit == false)
        {
            nonHitAction?.Invoke();
        }
    }
}
