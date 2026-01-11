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
    /// 真下方向のRay
    /// </summary>
    public RayCastAction landJudgeRay;
    /// <summary>
    /// 移動先の傾斜に沿って動ける距離
    /// </summary>
    public float followSlopeDistance;
    /// <summary>
    /// 行き止まり判定
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
    /// 移動方向+斜め下方向のRay
    /// </summary>
    public RayCastAction slopeRay;

    /// <summary>
    /// 足が地についている
    /// </summary>
    [field: SerializeField, NonEditable] public bool isLanding { get; private set; }
    /// <summary>
    /// 地面に埋まっている
    /// </summary>
    [field: SerializeField, NonEditable] public bool isBurying { get; private set; }
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
        float _footToHitDistance = Mathf.Abs(_hit.distance - FootToRayDistance);


        // 足元から向きを計算
        isBurying = AddFunction.GetUpDot(foot.position, _hit.point);
        if (isBurying == true)
        {
            isLanding = true;
        }
        else
        {
            _footToHitDistance = -_footToHitDistance;
            if (Mathf.Abs(_footToHitDistance) <= followSlopeDistance)
            {
                isLanding = true;
            }
            else
            {
                isLanding = false;
            }
        }
        //Debug.Log($"{_hit.distance} - {FootToRayDistance} = {_footToHitDistance}");



        if (isLanding == true)
        {

            if (isBurying == true)
            {
                enginePos.y = _footToHitDistance;
            }
            else if (isBurying == false)
            {
                enginePos.y = _footToHitDistance;
            }

            beforeHitDistance = _hit.distance;
        }
        
    }


    private void noLandRayAction()
    {
        enginePos = Vector3.zero;
        isLanding = false;
        isBurying = false;
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
