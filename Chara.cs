using AddUnityClass;
using System;
using UnityEngine;
using UnityEngine.InputSystem.Users;
using Fusion;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GenericChara
{
    [RequireComponent(typeof(Engine))]
    public class Chara : NetworkBehaviour
    {
        [field: SerializeField] public Parameter speed;
        public float assignSpeed { get; protected set; }
        public Engine engine { get; set; }
        [SerializeField, NonEditable] protected EntityAndPlan<Vector3> moveVelocity;
        [SerializeField, NonEditable] protected Quaternion rotatePlan;
        [field: SerializeField, NonEditable] public Chara lastAttacker { get; protected set; }

        public void Initialize_BaseChara()
        {
            engine = GetComponent<Engine>();
            engine.velocityPlanAction += GetVelocityPlan;

            speed.AssingEntity_Max();
            moveVelocity.plan = Vector3.zero;
            rotatePlan = Quaternion.identity;
        }

        public void Update_Parameter()
        {
            speed.Update();

        }


        /// <summary>
        /// value‚Í•ûŒü
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Vector3 GetAssignedSpeedVelocity(Vector3 value)
        {
            return value * assignSpeed;
        }

        public void AddAssignedMoveVelocity(Vector3 value)
        {
            moveVelocity.plan += GetAssignedSpeedVelocity(value);
        }
        public void AddMoveVelocity(Vector3 value)
        {
            moveVelocity.plan += value;
        }

        /// <summary>
        /// Engine‘¤‚ÉŽ©“®‚Å“o˜^‚³‚ê‚é
        /// </summary>
        public Vector3 GetVelocityPlan()
        {
            Vector3 _ret = Vector3.zero;
            _ret += moveVelocity.plan;
            moveVelocity.plan = Vector3.zero;

            return _ret;
        }


    }


}