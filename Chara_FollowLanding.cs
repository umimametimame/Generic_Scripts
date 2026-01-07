using AddUnityClass;
using System.Collections.Generic;
using UnityEngine;

public class Chara_FollowLanding : MonoBehaviour
{
    private Engine engine;
    private Vector3 enginePos;
    public Transform foot;
    public Collider footCollider;

    /// <summary>
    /// ^‰º•ûŒü‚ÌRay
    /// </summary>
    public RayCastAction landJudgeRay;
    /// <summary>
    /// ˆÚ“®æ‚ÌŒXÎ‚É‰ˆ‚Á‚Ä“®‚¯‚é‹——£
    /// </summary>
    public float followSlopeDistance;
    /// <summary>
    /// s‚«~‚Ü‚è”»’è
    /// </summary>
    public List<RayCastAction> deadEndJudgeRay = new List<RayCastAction>();
    [ContextMenu(nameof(AssignListDistance))]
    public void AssignListDistance()
    {
        for (int i = 0; i < deadEndJudgeRay.Count; ++i)
        {
            deadEndJudgeRay[i].distance = deadEndJudgeRay[0].distance;
        }
    }

    private float beforeHitDistance;

    /// <summary>
    /// ˆÚ“®•ûŒü+Î‚ß‰º•ûŒü‚ÌRay
    /// </summary>
    public RayCastAction slopeRay;


    [field: SerializeField, NonEditable] public bool isLanding { get; private set; }
    [field: SerializeField, NonEditable] public bool isDeadEnd { get; private set; }
    public float FootToRayDistance
    {
        get
        {
            float _ret = 0.0f;
            _ret = Vector3.Distance(landJudgeRay.ray.origin, foot.position);

            return _ret;
        }
    }
    private void Start()
    {
        engine = GetComponent<Engine>();
        engine.rbPositionFunc += GetFollowLandPos;

        if (footCollider != null)
        {

            //footCollider.onCollisionEnterEvent += OnStayLand;
            //footCollider.onTriggerExitEvent += OnExitLand;
        }

        landJudgeRay.excludeColliders.Add(footCollider);
        landJudgeRay.onHitClosestRayAction += OnLandRayAction;
        landJudgeRay.nonHitAction += noLandRayAction;
    }
    private void Update()
    {
        UpdateSwitchIsLanding();
    }
    private void UpdateSwitchIsLanding()
    {
        if (isLanding == true)
        {
            footCollider.isTrigger = true;
            engine.DisableGravity();
        }
        else if (isLanding == false)
        {
            footCollider.isTrigger = false;
            engine.EnableGravity();
        }
    }

    private void OnLandRayAction(RaycastHit _hit)
    {
        enginePos = Vector3.zero;
        if (_hit.collider != footCollider)
        {
            float _footToHitDistance = _hit.distance - FootToRayDistance;

            Debug.Log($"{_hit.distance} - {FootToRayDistance} = {_footToHitDistance}");
            if (_footToHitDistance <= FootToRayDistance)
            {
                isLanding = true;
            }
            else
            {
                isLanding = false;
            }


            if (isLanding == true)
            {
                // ŒXÎ‚ª‹–—e”ÍˆÍ“à‚È‚ç
                if (Mathf.Abs(_footToHitDistance) <= followSlopeDistance)
                {
                    if (AddFunction.GetUpDot(foot, _hit.transform) == false)
                    {
                        _footToHitDistance = -_footToHitDistance;
                    }

                    enginePos.y = _footToHitDistance;
                }
                beforeHitDistance = _hit.distance;
            }
        }
        else
        {
            isLanding = false;
        }
    }


    private void noLandRayAction()
    {
        enginePos = Vector3.zero;
        isLanding = false;
    }
    private void OnStayLand(Collision _col)
    {
        Collider _collider = _col.collider;
        isLanding = true;
        footCollider.isTrigger = true;

        if (_collider is TerrainCollider)
        {
        }
    }
    private void OnExitLand(Collider _col)
    {
        isLanding = false;
        footCollider.isTrigger = false;
        if (_col is TerrainCollider)
        {
        }
    }

    private void OnDeadEndRayAction_Head(RaycastHit _hit)
    {
        if (_hit.collider is TerrainCollider)
        {

        }
    }



    private Vector3 GetFollowLandPos()
    {
        Vector3 _ret = Vector3.zero;
        _ret = enginePos;

        return _ret;
    }
}
