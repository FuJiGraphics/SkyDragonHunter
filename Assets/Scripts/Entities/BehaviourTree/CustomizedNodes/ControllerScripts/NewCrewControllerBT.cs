using UnityEngine;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Entities;
using SkyDragonHunter.Managers;

namespace SkyDragonHunter.Entities 
{
    public enum CrewType
    {
        OnBoard,
        OnField,
    }

    public class NewCrewControllerBT : MonoBehaviour, ISlowable
    {
        // Static Fields
        private static readonly string s_EnemyTag = "Monster";

        // Private Fields
        [SerializeField] int m_Id;
        [SerializeField] private CrewType m_CrewType;
        [SerializeField] private Transform m_Target;
        private BehaviourTree<NewCrewControllerBT> m_BehaviourTree;
        private float slowMultiplier;
        private float skillTimer;
        private float exhaustionTime = 10f;
        private Vector2 airshipPos;
        [SerializeField] private bool isMounted;

        // Public Fields
        public bool IsMounted => isMounted;
        public Vector2 aggroBox;
        public Vector2 onFieldOriginPosition;
        public float targetPosY;
        public float exhaustionRemainingTime;

        // External Dependencies Field
        public CrewStats crewStatus;
        public FloatingEffect floater;
        public CrewAnimationController animController;

        // TODO: ChangeName
        public MountableSlot m_MountSlot;

        // Properties
        public int ID => m_Id;
        public float ExhaustionTime => 5f;
        public Vector2 MountSlotPosition => m_MountSlot.transform.position;
        public Vector2 AdjustedPosition => transform.position;
        public Transform Target => m_Target;
        public bool IsTargetRequiredForSkill => true;
        public bool IsSkillAvailable => !(skillTimer > 0);
        public bool IsTargetInAttackRange => TargetDistance < crewStatus.attackRange;
        public bool IsTargetInAggroRange => TargetDistance < crewStatus.aggroRange;
        public bool IsTargetAllocated => m_Target != null;
        public float Speed => crewStatus.speed;
        public float ChaseSpeed => crewStatus.chaseSpeed;
        public float AttackInterval => crewStatus.attackInterval;
        public float AggroRange => crewStatus.aggroRange;
        public float AttackRange => crewStatus.attackRange;
        


        public float TargetDistance
        {
            get
            {
                if (m_Target == null)
                {
                    return float.MaxValue;
                }
                var distanceX = Mathf.Abs(m_Target.position.x - transform.position.x);

                Vector2 targetPos = new Vector2(m_Target.position.x, targetPosY);
                Vector2 selfPos = transform.position;

                return Vector2.Distance(targetPos,selfPos);
            }
        }

        // Unity Methods
        private void Awake()
        {
            Init();
            GetAirshipPosX();
        }

        private void Update()
        {
            if (m_CrewType == CrewType.OnField && isMounted)
            {
                exhaustionRemainingTime -= Time.deltaTime;
                if(exhaustionRemainingTime <= 0)
                {
                    exhaustionRemainingTime = exhaustionTime;
                    crewStatus.status.ResetAll();
                    MountAction(false);
                    m_BehaviourTree.Reset();
                }
            }
            else
            {
                m_BehaviourTree.Update();
            }
        }
        private void FixedUpdate()
        {
            ResetTarget();
        }

        private void OnDrawGizmos()
        {
            //Gizmos.color = Color.yellow;
            //Gizmos.DrawWireCube(transform.position, new Vector3(crewStatus.aggroRange, 300, 1));
            //
            //Gizmos.color = Color.green;
            //Gizmos.DrawWireCube(airshipPos, new Vector3(70f, 300, 0f));
        }

        // Public Methods
        public void SetTarget(Transform targetTransform)
        {
            m_Target = targetTransform;
        }

        public void ResetTarget()
        {
            if (m_Target != null)
                return;
            else
                targetPosY = 0f;

            CrewResetTargetInBox();

            //if (m_CrewType == CrewType.OnBoard)
            //{
            //    OnBoardCrewResetTarget();
            //}        
            //if (m_CrewType == CrewType.OnField)
            //{
            //    OnFieldCrewResetTarget();
            //}
        }

        public void AllocateMountSlot(MountableSlot slot)
        {
            animController.PlayIdleAnimation();
            m_MountSlot = slot;
            isMounted = false;
            MountAction(true);
            transform.position = m_MountSlot.transform.position;
            exhaustionRemainingTime = 0.3f;
        }

        public void OnSlowBegin(float slowMultiplier)
        {
            this.slowMultiplier = slowMultiplier;
        }

        public void OnSlowEnd(float slowMultiplier)
        {
            this.slowMultiplier = 1f;
        }
        public void MountAction(bool mounted)
        {
            if (isMounted != mounted)
            {
                if (mounted)
                {
                    floater.enabled = false;
                    m_MountSlot.Mounting(gameObject);
                }
                else
                {
                    floater.enabled = true;
                    m_MountSlot.Dismounting();
                }
            }
            isMounted = mounted;
        }

        // Private Methods
        private void Init()
        {
            slowMultiplier = 1f;
            crewStatus = GetComponent<CrewStats>();
            floater = GetComponent<FloatingEffect>();
            animController = GetComponent<CrewAnimationController>();
            SetAggroBox();
            InitBehaviourTree();

            // TODO: LJH Temp Skill Timer
            skillTimer = 1f;            
            // ~TODO
        }

        private void SetAggroBox()
        {
            var airshipGO = GameMgr.FindObject("Airship");
            if (airshipGO != null)
                Debug.Log($"Found Airship");
            var airshipPosX = airshipGO.transform.position.x;

            aggroBox = new Vector2(Mathf.Abs(airshipPosX) + 10, 300f);
        }

        private void InitBehaviourTree()
        {
            switch(m_CrewType)
            {
                case CrewType.OnBoard:
                    InitOnBoardCrewBT();
                    break;
                case CrewType.OnField:
                    InitOnFieldCrewBT();
                    break;
            }
        }

        private void InitOnBoardCrewBT()
        {
            floater.enabled = false;
            var rootSelector = new SelectorNode<NewCrewControllerBT>(this);

            var attackSequence = new SequenceNode<NewCrewControllerBT>(this);
            attackSequence.AddChild(new CrewAttackCondition(this));
            attackSequence.AddChild(new CrewAttackAction(this));
            rootSelector.AddChild(attackSequence);

            m_BehaviourTree = new BehaviourTree<NewCrewControllerBT>(this);
            m_BehaviourTree.SetRoot(rootSelector);
        }

        private void InitOnFieldCrewBT()
        {
            floater.enabled = true;
            var rootSelector = new SelectorNode<NewCrewControllerBT>(this);

            //var skillSequence = new SequenceNode<NewCrewControllerBT>(this);
            //skillSequence.AddChild(new CrewSkillCondition(this));
            //skillSequence.AddChild(new CrewSkillAction(this));
            //rootSelector.AddChild(skillSequence);

            var attackSequence = new SequenceNode<NewCrewControllerBT>(this);
            attackSequence.AddChild(new CrewAttackCondition(this));
            attackSequence.AddChild(new CrewAttackAction(this));
            rootSelector.AddChild(attackSequence);

            var chaseSequence = new SequenceNode<NewCrewControllerBT>(this);
            chaseSequence.AddChild(new CrewChaseCondition(this));
            chaseSequence.AddChild(new CrewChaseAction(this));
            rootSelector.AddChild(chaseSequence);

            var returnSequence = new SequenceNode<NewCrewControllerBT>(this);
            returnSequence.AddChild(new NewCrewReturnCondition(this));
            returnSequence.AddChild(new NewCrewReturnAction(this));
            rootSelector.AddChild(returnSequence);

            m_BehaviourTree = new BehaviourTree<NewCrewControllerBT>(this);
            m_BehaviourTree.SetRoot(rootSelector);
        }

        private void OnBoardCrewResetTarget()
        {
            var colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(crewStatus.aggroRange, 300f), 0);
            if (colliders.Length > 0)
            {
                foreach (var collider in colliders)
                {
                    if (collider.CompareTag(s_EnemyTag))
                    {
                        if (m_Target == null || !m_Target.Equals(collider.transform))
                        {
                            m_Target = collider.transform;
                            targetPosY = m_Target.position.y;
                        }
                    }
                }
            }
            else
            {
                //Debug.Log($"crew target None");
            }
        }
        private void OnFieldCrewResetTarget()
        {
            var colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(crewStatus.aggroRange, 300f), 0);
            if (colliders.Length > 0)
            {
                foreach (var collider in colliders)
                {
                    if (collider.CompareTag(s_EnemyTag))
                    {
                        if (m_Target == null || !m_Target.Equals(collider.transform))
                        {
                            m_Target = collider.transform;
                            targetPosY = m_Target.position.y;
                        }
                    }
                }
            }
            else
            {
                Debug.Log($"crew target None");
            }
        }

        private void GetAirshipPosX()
        {
            var airship = GameMgr.FindObject("Airship");
            airshipPos = new Vector2(airship.transform.position.x, 0f);
        }

        private void CrewResetTargetInBox()
        {
            var colliders = Physics2D.OverlapBoxAll(airshipPos, new Vector2(70f, 300f), 0);
            if (colliders.Length > 0)
            {
                foreach (var collider in colliders)
                {
                    if (collider.CompareTag(s_EnemyTag))
                    {
                        if (m_Target == null || !m_Target.Equals(collider.transform))
                        {
                            m_Target = collider.transform;
                            targetPosY = m_Target.position.y;
                        }
                    }
                }
            }
            else
            {
                Debug.Log($"crew target None");
            }
        }


    } // Scope by class NewCrewControllerBT
} // namespace Root