using UnityEngine;
using SkyDragonHunter.Interfaces;
using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Test;
using JetBrains.Annotations;

namespace SkyDragonHunter.Entities 
{
    public enum CrewType
    {
        OnBoard,
        OnField,
    }
    public enum PossessionBonusStatType
    {
        ATK,
        HP,
        DEF,
        REG,
        CritMultiplier,
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
        private float exhaustionTime = 10f;
        private Vector2 airshipPos;
        [SerializeField] private bool isMounted;
        [SerializeField] private int mountedSlotIndex = -1;

        // Public Fields
        public bool IsMounted => isMounted;
        public Vector2 aggroBox;
        public Vector2 onFieldOriginPosition;
        public float exhaustionRemainingTime;

        // External Dependencies Field
        public CrewStats crewStatus;
        public FloatingEffect floater;
        public CrewAnimationController animController;
        public SkillExecutor skillExecutor;
        public CharacterStatus characterStatus;

        // TODO: ChangeName
        public MountableSlot m_MountSlot;
        public bool damageTypeSpellCasted;

        // Properties
        public int ID => m_Id;
        public float ExhaustionTime => 5f;
        public Vector2 MountSlotPosition => m_MountSlot.transform.position;

        // TODO: CCJ
        public int CurrentMountedSlotIndex => mountedSlotIndex;

        //public Vector2 AdjustedPosition => transform.position;
        public Transform Target => m_Target;
        public bool IsTargetRequiredForSkill => true;
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
                var distance = Vector2.Distance(m_Target.position, transform.position);

                var colliderInfo = m_Target.GetComponent<ColliderInfoProvider>();
                float halfWidth = 0f;
                if (colliderInfo != null)
                {
                    halfWidth = colliderInfo.ColliderHalfWidth;
                }
                else
                {
                   //  Debug.LogWarning($"[{gameObject.name}] Could not find ColliderInfo from target {m_Target.name}");
                }

                return distance - halfWidth;
            }
        }

        // Unity Methods
        private void Awake()
        {
            Init();
            GetAirshipPosX();
        }

        private void Start()
        {
            //Init();
            //GetAirshipPosX();
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

            CrewResetTargetInBox();
        }

        public void AllocateMountSlot(MountableSlot slot, int slotIndex)
        {
            animController.PlayIdleAnimation();
            m_MountSlot = slot;
            mountedSlotIndex = slotIndex;
            onFieldOriginPosition = new Vector2(0f, 6f - slotIndex * 4f);
            transform.position = m_MountSlot.transform.position;
            isMounted = false;
            MountAction(true);
            transform.position = m_MountSlot.transform.position;
            exhaustionRemainingTime = 0.3f;

            var crewData = DataTableMgr.CrewTable.Get(ID);
            GameObject loadedSkillGo = null;

            if (crewData.SkillPrefabName != "0")
            {
                loadedSkillGo = ResourcesMgr.Load<GameObject>(crewData.SkillPrefabName);
                // Debug.LogWarning($"[{gameObject.name}] skill [{crewData.SkillPrefabName}] allocated.");
            }
            else
            {
                // Debug.LogWarning($"[{gameObject.name}] skill not set, skill prefab name '0'");
            }
            SkillBase loadedSkill = null;
            if (loadedSkillGo != null)
            {
                loadedSkill = loadedSkillGo.GetComponent<SkillBase>();
            }

            // skillExecutor.SetSkillSlotUI(slotIndex, loadedSkill);
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
                    m_MountSlot.Dismounting();
                }
            }
            isMounted = mounted;
        }

        public void SetDataFromTableWithExistingIDTemp(int level)
        {
            // Debug.LogWarning($"Starting Crew SetData ID/LVL[{ID}/{level}]");
            var lvlBonus = Mathf.Min(0, level - 1);

            var crewData = DataTableMgr.CrewTable.Get(ID);

            // Debug.LogWarning($"CrewData = {crewData}");

            gameObject.name = crewData.UnitName;

            var health = crewData.UnitbasicHP;
            health += crewData.LevelBracketHP * lvlBonus;
            crewStatus.status.MaxHealth = health;

            var atk = crewData.UnitbasicATK;
            atk += crewData.LevelBracketATK * lvlBonus;
            crewStatus.status.MaxDamage = atk;

            var def = crewData.UnitbasicDEF;
            def += crewData.LevelBracketDEF * lvlBonus;
            crewStatus.status.MaxArmor = def;

            var reg = crewData.UnitbasicREC;
            reg += crewData.LevelBracketREC * lvlBonus;
            crewStatus.status.MaxResilient = reg;
            
            crewStatus.status.ResetAll();
        }

        // Private Methods
        private void Init()
        {
            mountedSlotIndex = -1;
            slowMultiplier = 1f;
            crewStatus = GetComponent<CrewStats>();
            floater = GetComponent<FloatingEffect>();
            floater.enabled = false;
            animController = GetComponent<CrewAnimationController>();
            skillExecutor = GetComponent<SkillExecutor>();
            characterStatus = GetComponent<CharacterStatus>();
            SetAggroBox();
            InitBehaviourTree();
        }

        private void SetAggroBox()
        {
            var airshipGO = GameMgr.FindObject("Airship");
            // if (airshipGO != null)
                // Debug.Log($"Found Airship");
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
            var rootSelector = new SelectorNode<NewCrewControllerBT>(this);

            var skillSequence = new SequenceNode<NewCrewControllerBT>(this);
            skillSequence.AddChild(new CrewSkillCondition(this));
            skillSequence.AddChild(new CrewSkillAction(this));
            rootSelector.AddChild(skillSequence);

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
            returnSequence.AddChild(new NewCrewIdleAction(this));
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
                        }
                    }
                }
            }
            else
            {
                // Debug.Log($"crew target None");
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
                        }
                    }
                }
            }
            else
            {
                // Debug.Log($"crew target None");
            }
        }


    } // Scope by class NewCrewControllerBT
} // namespace Root