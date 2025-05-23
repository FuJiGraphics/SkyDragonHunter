using MathNet.Numerics.Providers.SparseSolver;
using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Test;
using SkyDragonHunter.UI;
using UnityEngine;

namespace SkyDragonHunter.Entities 
{
    public enum MonsterType
    {
        Melee,
        Ranged,
    }

    public class NewMonsterControllerBT : MonoBehaviour, ISlowable
    {
        // Static Fields
        protected static readonly string s_PlayerTag = "Player";
        protected static readonly string s_CrewTag = "Crew";

        // Private Fields
        [SerializeField] private MonsterType m_MonsterType;
        [SerializeField] private Transform m_Target;
        [SerializeField] private NewCrewControllerBT m_CrewTarget;
        [SerializeField] private Transform m_AirshipTarget;
        private BehaviourTree<NewMonsterControllerBT> m_BehaviourTree;
        private UIHealthBar[] m_UIHealthBars;
        private float slowMultiplier;

        private string projectileId;

        // Public Fields
        public int ID;

        // External Dependencies Field
        public MonsterStats monsterStatus;
        public FloatingEffect floater;
        public TestAniController animController;

        // Properties
        public UIHealthBar[] HealthBars => m_UIHealthBars;
        public BigNum HP
        {
            get => monsterStatus.status.Health;
        }
        public BigNum MaxHP
        {
            get => monsterStatus.status.MaxHealth;
            set
            {
                monsterStatus.status.MaxHealth = value;
                monsterStatus.status.Health = monsterStatus.status.MaxHealth;
                monsterStatus.status.ResetAll();
            }
        }
        public BigNum Damage
        {
            get => monsterStatus.status.MaxDamage;
            set
            {
                monsterStatus.status.MaxDamage = value;
                monsterStatus.status.ResetAll();
            }
        }
        public Transform Target => m_Target;
        public bool IsTargetNull => m_Target == null;
        public bool IsTargetInAttackRange => TargetDistance < monsterStatus.attackRange;
        public bool IsTargetInAggroRange => TargetDistance < monsterStatus.aggroRange;
        public bool IsTargetAllocated => m_Target != null;
        public float Speed => monsterStatus.speed;
        public float ChaseSpeed => monsterStatus.chaseSpeed;
        public float AggroRange => monsterStatus.aggroRange;
        public float AttackRange => monsterStatus.attackRange;
        public float AttackInterval => monsterStatus.attackInterval;
        public float m_TargetDistance;
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
                if(colliderInfo != null)
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
                m_TargetDistance = Mathf.Max((distance - halfWidth), 0f);
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
            //Gizmos.DrawWireCube(transform.position, new Vector3(monsterStatus.aggroRange, 300, 1));
            //
            //Gizmos.color = Color.red;
            //Gizmos.DrawWireCube(transform.position, new Vector3(monsterStatus.attackRange, 300, 1));
        }

        // Public Methods
        public void SetTarget(Transform targetTransform)
        {
            m_Target = targetTransform;
        }

        public void OnSlowBegin(float slowMultiplier)
        {
            this.slowMultiplier = slowMultiplier;
            monsterStatus.speed *= slowMultiplier;
            monsterStatus.chaseSpeed *= slowMultiplier;
            GetComponent<SpriteRenderer>().color = Color.yellow;
        }

        public void OnSlowEnd(float slowMultiplier)
        {
            this.slowMultiplier = 1f;
            monsterStatus.speed *= (1 / slowMultiplier);
            monsterStatus.chaseSpeed *= (1 / slowMultiplier);
            GetComponent<SpriteRenderer>().color = Color.white;
        }

        // Private Methods
        public void ResetTarget()
        {
            if(m_CrewTarget != null && m_CrewTarget.isActiveAndEnabled && !m_CrewTarget.IsMounted)
            {
                m_Target = m_CrewTarget.transform;
                return;
            }
            else
            {
                m_CrewTarget = null;
                if(m_Target == null || !m_Target.CompareTag(s_PlayerTag))
                {
                    m_Target = GameMgr.FindObject("Airship").transform;
                }
            }

            var colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(monsterStatus.aggroRange, 300f), 0f);
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
            m_UIHealthBars = GetComponentsInChildren<UIHealthBar>();
            monsterStatus = GetComponent<MonsterStats>();
            monsterStatus.ForceInit();
            animController = GetComponent<TestAniController>();
            floater = GetComponent<FloatingEffect>();
            floater.enabled = false;
            var airshipGo = GameMgr.FindObject($"Airship");
            if( airshipGo != null )
            {
                m_AirshipTarget = airshipGo.transform;
            }
            else
            {
                Debug.LogError($"Airship Null");
            }
            if (animController != null)
                this.ID = animController.ID;
            //SetDataFromTable(ID);
            InitBehaviourTree();
        }

        public void SetDataFromTable(int id)
            => SetDataFromTable(id, 1, 1);

        public void SetDataFromTable(int id, BigNum hpMultiply, BigNum atkMultiply)
        {
            ID = id;
            var data = DataTableMgr.MonsterTable.Get(id);
            if (data == null)
            {
                Debug.LogError($"Set Monster Data Failed : ID '{id}' not found in monster table.");
                return;
            }

            name = data.Name;
            m_MonsterType = data.Type;
            monsterStatus.status.MaxHealth = data.HP * hpMultiply;
            monsterStatus.status.MaxDamage = data.ATK * atkMultiply;
            projectileId = data.ProjectileID;
            monsterStatus.attackInterval = data.AttackInterval;
            monsterStatus.attackRange = data.AttackRange;
            monsterStatus.aggroRange = data.AggroRange;
            monsterStatus.speed = data.Speed;
            monsterStatus.chaseSpeed = data.ChaseSpeed;
            monsterStatus.status.ResetAll();            
            // Debug.Log($"Monster '{name}' Data Set. HP: ({monsterStatus.status.Health}/{monsterStatus.status.MaxHealth}), ATK: ({monsterStatus.status.MaxDamage})");
        }

        private void InitBehaviourTree()
        {
            switch (m_MonsterType)
            {
                case MonsterType.Melee:
                case MonsterType.Ranged:
                    InitRangedBT();
                    break;
            }
        }

        private void InitMeleeBT()
        {

        }

        private void InitRangedBT()
        {
            floater.enabled = true;
            m_BehaviourTree = new BehaviourTree<NewMonsterControllerBT>(this);

            var rootSelector = new SelectorNode<NewMonsterControllerBT>(this);

            var attackSequence = new SequenceNode<NewMonsterControllerBT>(this);
            attackSequence.AddChild(new MonsterAttackCondition(this));
            attackSequence.AddChild(new MonsterAttackAction(this));
            rootSelector.AddChild(attackSequence);

            var chaseSequence = new SequenceNode<NewMonsterControllerBT>(this);
            chaseSequence.AddChild(new MonsterChaseCondition(this));
            chaseSequence.AddChild(new MonsterChaseAction(this));
            rootSelector.AddChild(chaseSequence);

            var moveSequence = new SequenceNode<NewMonsterControllerBT>(this);
            moveSequence.AddChild(new MonsterMoveCondition(this));
            moveSequence.AddChild(new MonsterMoveAction(this));
            rootSelector.AddChild(moveSequence);

            m_BehaviourTree.SetRoot(rootSelector);
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