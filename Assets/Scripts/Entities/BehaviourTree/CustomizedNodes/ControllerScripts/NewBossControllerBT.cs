using SkyDragonHunter.Entities;
using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Test;
using UnityEngine;

namespace SkyDragonHunter.Entities
{
    public enum BossAttackType
    {
        Melee,
        Ranged,
        Stationary,
    }

    public class NewBossControllerBT : MonoBehaviour, ISlowable
    {
        // Static Fields
        protected static readonly string s_PlayerTag = "Player";
        protected static readonly string s_CrewTag = "Crew";

        // Private Fields
        [SerializeField] private BossAttackType m_AttackType;
        [SerializeField] private Transform m_Target;
        [SerializeField] private NewCrewControllerBT m_CrewTarget;
        [SerializeField] private Transform m_AirshipTarget;
        private BehaviourTree<NewBossControllerBT> m_BehaviourTree;
        private float slowMultiplier;

        private string projectileId;



        // Public Fields
        public int ID;

        // External Dependencies Field
        public BossStats bossStatus;
        public FloatingEffect floater;
        public TestAniController animController;

        // Properties
        public BigNum HP
        {
            get => bossStatus.status.Health;
        }
        public BigNum MaxHP
        {
            get => bossStatus.status.MaxHealth;
            set
            {
                bossStatus.status.MaxHealth = value;
                bossStatus.status.Health = value;
                bossStatus.status.ResetAll();
            }
        }

        public BigNum MaxATK
        {
            get => bossStatus.status.MaxDamage;
            set
            {
                bossStatus.status.MaxDamage = value;
                bossStatus.status.Damage = value;
                bossStatus.status.ResetAll();
            }
        }

        public bool IsSkillAvailable => false;
        //public Vector2 AdjustedPosition => transform.position;
        public Transform Target => m_Target;
        public bool IsTargetNull => m_Target == null;
        public bool IsTargetInAttackRange => TargetDistance < bossStatus.attackRange;
        public bool IsTargetInAggroRange => TargetDistance < bossStatus.aggroRange;
        public bool IsTargetAllocated => m_Target != null;
        public float Speed => bossStatus.speed;
        public float ChaseSpeed => bossStatus.chaseSpeed;
        public float AggroRange => bossStatus.aggroRange;
        public float AttackRange => bossStatus.attackRange;
        public float AttackInterval => bossStatus.attackInterval;
        public float TargetDistance
        {
            get
            {
                if (m_Target == null)
                {
                    return float.MaxValue;
                }

                var colliderInfo = m_Target.GetComponent<ColliderInfoProvider>();
                float halfWidth = 0f;
                if (colliderInfo != null)
                {
                    halfWidth = colliderInfo.ColliderHalfWidth;
                }
                else
                {
                    Debug.LogWarning($"[{gameObject.name}] Could not find ColliderInfo from target {m_Target.name}");
                }

                var distance = Mathf.Abs(m_Target.position.x - transform.position.x);
                //var sr = m_Target.gameObject.GetComponentInChildren<SpriteRenderer>();
                //var halfwidth = sr.bounds.size.x * 0.5f;               
                return distance - halfWidth;
            }
        }

        // Unity Methods
        private void Awake()
        {
            Init();
        }
        private void Start()
        {
            InitBehaviourTree();
        }

        private void Update()
        {
            m_BehaviourTree.Update();
        }
        private void FixedUpdate()
        {
            ResetTarget();
        }

        private void OnDrawGizmos()
        {
            //Gizmos.color = Color.yellow;
            //Gizmos.DrawWireCube(transform.position, new Vector3(bossStatus.aggroRange, 300, 1));
            //
            //Gizmos.color = Color.red;
            //Gizmos.DrawWireCube(transform.position, new Vector3(bossStatus.attackRange, 300, 1));
        }

        // Public Methods
        public void SetTarget(Transform targetTransform)
        {
            m_Target = targetTransform;
        }

        public void OnSlowBegin(float slowMultiplier)
        {
            this.slowMultiplier = slowMultiplier;
        }

        public void OnSlowEnd(float slowMultiplier)
        {
            this.slowMultiplier = 1f;
        }

        // Private Methods
        public void ResetTarget()
        {
            if (m_CrewTarget != null && m_CrewTarget.isActiveAndEnabled && !m_CrewTarget.IsMounted)
            {
                m_Target = m_CrewTarget.transform;
                return;
            }
            else
            {
                m_CrewTarget = null;
                if (m_Target == null || !m_Target.CompareTag(s_PlayerTag))
                {
                    m_Target = GameMgr.FindObject("Airship").transform;
                }
            }

            var colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(bossStatus.aggroRange, 300f), 0f);
            if (colliders.Length > 0)
            {
                foreach (var collider in colliders)
                {
                    if (collider.CompareTag(s_CrewTag))
                    {
                        var crewBT = collider.GetComponent<NewCrewControllerBT>();
                        if (crewBT == null)
                        {
                            Debug.LogError($"Missing NewCrewControllerBT");
                            return;
                        }
                        if (crewBT.IsMounted)
                            continue;
                        if (DistanceToTargetTransform(collider.transform) > DistanceToTargetTransform(m_AirshipTarget))
                            continue;

                        m_CrewTarget = crewBT;
                        m_Target = m_CrewTarget.transform;
                        return;
                    }
                }
            }
            m_CrewTarget = null;
        }

        private void Init()
        {
            bossStatus = GetComponent<BossStats>();
            bossStatus.ForceInit();
            animController = GetComponent<TestAniController>();
            floater = GetComponent<FloatingEffect>();
            floater.enabled = false;
            if (animController != null)            
                this.ID = animController.ID;
            var airshipGo = GameMgr.FindObject($"Airship");
            if (airshipGo != null)
            {
                m_AirshipTarget = airshipGo.transform;
            }
            else
            {
                Debug.LogError($"Airship Null");
            }
        }

        public void SetDataFromTable(int id)
            => SetDataFromTable(id, 1, 1);

        public void SetDataFromTable(int id, BigNum hpMultipler, BigNum atkMultiplier)
        {
            ID = id;

            var data = DataTableMgr.BossTable.Get(id);
            if (data == null)
            {
                Debug.Log($"Set Boss Data Failed : ID '{id}' not found in Boss table.");
                return;
            }

            name = "(Boss)"+data.Name;
            m_AttackType = data.Type;
            bossStatus.status.MaxHealth = data.HP * hpMultipler;
            bossStatus.status.MaxDamage = data.ATK * atkMultiplier;
            projectileId = data.ProjectileID;
            bossStatus.attackInterval = data.AttackInterval;
            bossStatus.attackRange = data.AttackRange;
            bossStatus.aggroRange = data.AggroRange;
            bossStatus.speed = data.Speed;
            bossStatus.chaseSpeed = data.ChaseSpeed;
            bossStatus.status.ResetAll();
        }

        private void InitBehaviourTree()
        {
            switch (m_AttackType)
            {
                case BossAttackType.Melee:
                case BossAttackType.Ranged:
                    InitRangedBT();
                    break;
                case BossAttackType.Stationary:
                    InitStationaryBT();
                    break;
            }
        }

        private void InitMeleeBT()
        {

        }

        private void InitRangedBT()
        {
            floater.enabled = true;

            var rootSelector = new SelectorNode<NewBossControllerBT>(this);

            var skillSequence = new SequenceNode<NewBossControllerBT>(this);
            skillSequence.AddChild(new NewBossSkillCondition(this));
            skillSequence.AddChild(new NewBossSkillAction(this));
            rootSelector.AddChild(skillSequence);

            var attackSequence = new SequenceNode<NewBossControllerBT>(this);
            attackSequence.AddChild(new BossAttackCondition(this));
            attackSequence.AddChild(new BossAttackAction(this));
            rootSelector.AddChild(attackSequence);

            var chaseSequence = new SequenceNode<NewBossControllerBT>(this);
            chaseSequence.AddChild(new BossChaseCondition(this));
            chaseSequence.AddChild(new BossChaseAction(this));
            rootSelector.AddChild(chaseSequence);

            var moveSequence = new SequenceNode<NewBossControllerBT>(this);
            moveSequence.AddChild(new BossMoveCondition(this));
            moveSequence.AddChild(new BossMoveAction(this));
            rootSelector.AddChild(moveSequence);

            m_BehaviourTree = new BehaviourTree<NewBossControllerBT>(this);
            m_BehaviourTree.SetRoot(rootSelector);
        }

        private void InitStationaryBT()
        {
            var rootSelector = new SelectorNode<NewBossControllerBT>(this);

            var skillSequence = new SequenceNode<NewBossControllerBT>(this);
            skillSequence.AddChild(new NewBossSkillCondition(this));
            skillSequence.AddChild(new NewBossSkillAction(this));
            rootSelector.AddChild(skillSequence);

            m_BehaviourTree = new BehaviourTree<NewBossControllerBT>(this);
            m_BehaviourTree.SetRoot(rootSelector);
        }

        private void InitBossBT()
        {

        }

        private float DistanceToTargetTransform(Transform target)
        {
            if (target == null)
            {
                return float.MaxValue;
            }

            var colliderInfo = target.GetComponent<ColliderInfoProvider>();
            float halfWidth = 0f;
            if (colliderInfo != null)
            {
                halfWidth = colliderInfo.ColliderHalfWidth;
            }
            else
            {
                Debug.LogWarning($"[{gameObject.name}] Could not find ColliderInfo from target {target.name}");
            }

            var distance = Mathf.Abs(target.position.x - transform.position.x);
            //var sr = m_Target.gameObject.GetComponentInChildren<SpriteRenderer>();
            //var halfwidth = sr.bounds.size.x * 0.5f;
            return distance - halfWidth;
        }
    } // Scope by class NewMonsterControllerBT

} // namespace Root