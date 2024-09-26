using AddClass;
using System;
using UnityEngine;
using UnityEngine.InputSystem.Users;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GenericChara
{
    
    public class Chara : MonoBehaviour
    {
        public enum CharaState
        {
            Spawn,
            Alive,
            Death,
            ReSpawn,
            None,
        }
        [field: SerializeField, NonEditable] public CharaState charaState { get; private set; }
        protected Action spawnAction;
        protected Action aliveAction;
        protected Action deathAction;
        protected Action reSpawnAction;
        [field: SerializeField] public Parameter hp;
        [field: SerializeField] public Parameter speed;
        protected float assignSpeed;
        [field: SerializeField] public Parameter pow;
        public Engine engine { get; set; }
        [field: SerializeField, NonEditable] public bool alive { get; protected set; }  //  生存
        [SerializeField] private Interval respawnInterval;
        [SerializeField] protected EntityAndPlan<Vector3> moveVelocity;
        [SerializeField] protected Quaternion rotatePlan;
        protected Action<UnderAttackType> underAttackAction;
        [SerializeField] private Interval spawnInvincible;
        [SerializeField] protected Interval invincible;
        [field: SerializeField] public Chara lastAttacker { get; private set; }
        protected virtual void Start()
        {
            Initialize();
            engine = GetComponent<Engine>();
            engine.velocityPlanAction += AddVelocityPlan;
            alive = true;
            spawnAction += Spawn;

            deathAction += () => respawnInterval.Update();
            respawnInterval.reachAction += () => StateChange(CharaState.ReSpawn);
        }

        /// <summary>
        /// SpawnStateに行われる
        /// </summary>
        protected virtual void Spawn()
        {

            respawnInterval.Initialize(false);
            spawnInvincible.Reset();
        }

        /// <summary>
        /// ParameterのUpdate<br/>
        /// State駆動処理<br/>
        /// moveVelocityのリセット
        /// </summary>
        protected virtual void Update()
        {
            hp.Update();
            speed.Update();
            pow.Update();
            spawnInvincible.Update();

            switch (charaState)
            {
                case CharaState.Spawn:
                    spawnAction?.Invoke();
                    StateChange(CharaState.Alive);

                    break;
                case CharaState.Alive:
                    aliveAction?.Invoke();
                    break;

                case CharaState.Death:
                    deathAction?.Invoke();
                    break;
                case CharaState.ReSpawn:
                    reSpawnAction?.Invoke();
                    respawnInterval.Initialize(false);
                    spawnInvincible.Reset();
                    StateChange(CharaState.Alive);
                    break;
            }
        }
        public void Initialize()
        {
            hp.Initialize();
            speed.Initialize();
            pow.Initialize();
            moveVelocity.plan = Vector3.zero;
            rotatePlan = Quaternion.identity;
        }

        protected virtual void Reset()
        {
            lastAttacker = null;
        }

        /// <summary>
        /// valueは方向
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

        /// <summary>
        /// Engine側に自動で登録される
        /// </summary>
        public void AddVelocityPlan()
        {
            engine.velocityPlan = moveVelocity.plan;
            engine.rotatePlan = rotatePlan;
            moveVelocity.plan = Vector3.zero;
            rotatePlan = Quaternion.identity;
        }

        /// <summary>
        /// 引数はダメージ量と被弾モーションを行うかどうか
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="damageMotion"></param>
        public bool UnderAttack(float damage, UnderAttackType type = UnderAttackType.None, Chara attacker = null)
        {
            if (alive == false) { return false; }
            else if (spawnInvincible.active == false) { return false; }
            else if (invincible.active == false) { return false; }

            hp.entity -= damage;

            underAttackAction?.Invoke(type);

            if (attacker != null) { lastAttacker = attacker; }

            return true;
        }

        public void StateChange(CharaState state)
        {
            charaState = state;
        }

    }


}