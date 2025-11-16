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
    
    public class Chara : NetworkBehaviour
    {
        [field: SerializeField] public Parameter hp;
        [field: SerializeField] public Parameter speed;
        public float assignSpeed { get; protected set; }
        [field: SerializeField] public Parameter pow;
        public Engine engine { get; set; }
        [SerializeField] protected EntityAndPlan<Vector3> moveVelocity;
        [SerializeField] protected Quaternion rotatePlan;
        [field: SerializeField] public Chara lastAttacker { get; protected set; }
        public override void Spawned()
        {
            Initialize();
            engine = GetComponent<Engine>();
            engine.velocityPlanAction += AddVelocityPlan;

        }


        /// <summary>
        /// ParameterÇÃUpdate<br/>
        /// StateãÏìÆèàóù<br/>
        /// moveVelocityÇÃÉäÉZÉbÉg
        /// </summary>
        public override void FixedUpdateNetwork()
        {
            hp.Update();
            speed.Update();
            pow.Update();

        }
        public void Initialize()
        {
            hp.AssingEntity_Max();
            speed.AssingEntity_Max();
            pow.AssingEntity_Max();
            moveVelocity.plan = Vector3.zero;
            rotatePlan = Quaternion.identity;
        }


        /// <summary>
        /// valueÇÕï˚å¸
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
            moveVelocity.plan += GetAssignedSpeedVelocity(value);
        }

        /// <summary>
        /// Engineë§Ç…é©ìÆÇ≈ìoò^Ç≥ÇÍÇÈ
        /// </summary>
        public void AddVelocityPlan()
        {
            engine.velocityPlan = moveVelocity.plan;
            engine.rotatePlan = rotatePlan;
            moveVelocity.plan = Vector3.zero;
            rotatePlan = Quaternion.identity;
        }


    }


}