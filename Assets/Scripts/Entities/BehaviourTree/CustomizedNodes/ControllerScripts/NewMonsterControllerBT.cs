using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Managers;
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
        [SerializeField] private AttackType m_MonsterType;
        [SerializeField] private Transform m_Target;
        [SerializeField] private NewCrewControllerBT m_CrewTarget;
        private BehaviourTree<NewMonsterControllerBT> m_BehaviourTree;
        private float slowMultiplier;

        private int projectileId;

        // Public Fields
        public int ID;

        // External Dependencies Field
        public MonsterStats monsterStatus;
        public FloatingEffect floater;
        public TestAniController animController;

        // Properties
        public Vector2 AdjustedPosition => transform.position;
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

                var distance = Mathf.Abs(m_Target.position.x - transform.position.x);
                var sr = m_Target.gameObject.GetComponentInChildren<SpriteRenderer>();
                var halfwidth = sr.bounds.size.x * 0.5f;
                m_TargetDistance = distance - halfwidth;
                return distance - halfwidth;
            }
        }

        // Unity Methods
        private void Awake()
        {
            Init();
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
        private void ResetTarget()
        {
            if(m_CrewTarget != null && !m_CrewTarget.IsMounted)
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
                        //if (crewBT.IsMounted)
                        //    continue;
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
            monsterStatus = GetComponent<MonsterStats>();
            monsterStatus.ForceInit();
            animController = GetComponent<TestAniController>();
            floater = GetComponent<FloatingEffect>();
            this.ID = animController.ID;
            SetDataFromTable(ID);
            InitBehaviourTree();
        }

        private void SetDataFromTable(int id)
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
            monsterStatus.status.MaxHealth = data.HP;
            monsterStatus.status.MaxDamage = data.ATK;
            monsterStatus.status.MaxArmor = data.DEF;
            monsterStatus.status.MaxResilient = data.REG;
            projectileId = data.ProjectileID;
            monsterStatus.attackInterval = data.AttackInterval;
            monsterStatus.attackRange = data.AttackRange;
            monsterStatus.aggroRange = data.AggroRange;
            monsterStatus.speed = data.Speed;
            monsterStatus.chaseSpeed = data.ChaseSpeed;
            monsterStatus.status.ResetAll();
        }

        private void InitBehaviourTree()
        {
            switch (m_MonsterType)
            {
                case AttackType.Melee:
                case AttackType.Ranged:
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

        private void InitBossBT()
        {

        }
    } // Scope by class NewMonsterControllerBT

} // namespace Root