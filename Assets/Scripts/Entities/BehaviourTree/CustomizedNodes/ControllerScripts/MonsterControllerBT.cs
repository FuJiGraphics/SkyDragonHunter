using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Managers;
using UnityEngine;

namespace SkyDragonHunter.Entities 
{
    public enum AttackType
    {
        Melee,
        Ranged,
    }

    public class MonsterControllerBT : BaseControllerBT<MonsterControllerBT>
    {
        // 필드 (Fields)
        [SerializeField] private AttackType m_AttackType;
        [SerializeField] private MonsterPrefabLoader monsterPrefabLoader;
        [SerializeField] private TestAniController tempAnimController;
        
        public override bool IsChasing
        { 
            get => base.IsChasing;
            set
            {
                if(value != IsChasing)
                {
                    tempAnimController.OnChase();
                }
                base.IsChasing = value;
            }
        }

        private CrewControllerBT m_CrewTarget;

        // 유니티 (MonoBehaviour 기본 메서드)
        protected void OnEnable()
        {
            monsterPrefabLoader = GameMgr.FindObject("MonsterPrefabLoader").GetComponent<MonsterPrefabLoader>();
        }

        protected override void Start()
        {
            base.Start();
        }

        private void Update()
        {
            UpdatePosition();
            m_BehaviourTree.Update();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, m_AggroRange);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, m_AttackRange);
        }

        // Public 메서드
        public override void SetDataFromTable(int id)
        {
            ID = id;
            var data = DataTableMgr.MonsterTable.Get(id);
            if (data == null)
            {
                Debug.LogError($"Set Monster Data Failed : ID '{id}' not found in monster table.");
                return;
            }

            name = data.Name;
            m_AttackType = data.Type;
            status.MaxHealth = data.HP;
            status.MaxDamage = data.ATK;
            status.MaxArmor = data.DEF;
            status.MaxResilient = data.REG;
            projectileId = data.ProjectileID;
            m_AttackInterval = data.AttackInterval;
            m_AttackRange = data.AttackRange;
            m_AggroRange = data.AggroRange;
            m_Speed = data.Speed;
            m_ChaseSpeed = data.ChaseSpeed;
            status.ResetAll();

            var animController = monsterPrefabLoader.GetMonsterAnimController(id);
            var instantiatedAnimController = Instantiate(animController, transform);
            Vector3 localScale = instantiatedAnimController.transform.localScale;
            localScale.x = -1;
            instantiatedAnimController.transform.localScale = localScale;
            tempAnimController = instantiatedAnimController;
        }

        public override void TriggerAttack()
        {
            base.TriggerAttack();
            tempAnimController.OnAttck();
        }

        public override void ResetHealth()
        {
            status.Health = status.MaxHealth;
        }

        public override void ResetTarget()
        {            
            if(m_CrewTarget != null && !m_CrewTarget.isMounted)
            {
                m_Target = m_CrewTarget.transform;
                return;
            }
            else
            {
                m_CrewTarget = null;
                if (m_Target == null)
                {
                    m_Target = GameObject.FindWithTag($"Player").transform;
                }
                else if (!m_Target.CompareTag(s_PlayerTag))
                {
                    m_Target = GameObject.FindWithTag($"Player").transform;
                }
            }

            var colliders = Physics2D.OverlapCircleAll(transform.position, m_AggroRange);
            if (colliders.Length > 0)
            {
                foreach (var collider in colliders)
                {
                    if (collider.CompareTag(s_CrewTag))
                    {
                        var crewBT = collider.GetComponent<CrewControllerBT>();
                        if (crewBT != null)
                        {
                            var onBoard = crewBT.isMounted;
                            if (onBoard)
                            {
                                continue;
                            }
                            else
                            {
                                m_CrewTarget = crewBT;
                                return;
                            }
                        }
                    }
                    m_CrewTarget = null;
                    return;
                }
            }
        }

        public void NullTarget()
        {
            m_Target = null;
        }

        // Protected 메서드
        protected override void InitBehaviourTree()
        {
            switch (m_AttackType)
            {
                case AttackType.Melee:     
                case AttackType.Ranged:
                    InitMeleeBT();
                    break;
            }
        }

        private void InitMeleeBT()
        {
            m_BehaviourTree = new BehaviourTree<MonsterControllerBT>(this);

            var rootSelector = new SelectorNode<MonsterControllerBT>(this);

            var attackSequence = new SequenceNode<MonsterControllerBT>(this);
            attackSequence.AddChild(new EntityAttackableCondition<MonsterControllerBT>(this));
            attackSequence.AddChild(new EntityAttackAction<MonsterControllerBT>(this));
            rootSelector.AddChild(attackSequence);

            var chaseSequence = new SequenceNode<MonsterControllerBT>(this);
            chaseSequence.AddChild(new EntityChasableCondition<MonsterControllerBT>(this));
            chaseSequence.AddChild(new EntityChaseAction<MonsterControllerBT>(this));
            rootSelector.AddChild(chaseSequence);

            var moveSequence = new SequenceNode<MonsterControllerBT>(this);
            moveSequence.AddChild(new EntityMoveCondition<MonsterControllerBT>(this));
            moveSequence.AddChild(new EntityMoveAction<MonsterControllerBT>(this));
            rootSelector.AddChild(moveSequence);

            m_BehaviourTree.SetRoot(rootSelector);
        }
        
        private void UpdatePosition()
        {
            var newPos = transform.position;              

            if (IsChasing || isMoving)
            {                
                int toRight = 1;
                var yRotation = 0;
                if (!isDirectionToRight)
                {
                    toRight *= -1;
                    yRotation += 180;
                }
                if (IsChasing)
                    toRight *= 3;

                //Debug.Log($"yRotation {yRotation}");
                //var localRotation = (tempAnimController.transform.localRotation).eulerAngles;
                //localRotation.y = yRotation;
                //tempAnimController.transform.localRotation = Quaternion.Euler(localRotation);
                //tempAnimController.transform.localScale = Vector3.one;
                //Debug.Log($"{tempAnimController.transform.localRotation.eulerAngles}, {yRotation}");

                newPos.x += Time.deltaTime * m_Speed * toRight;           
            }

            transform.position = newPos;
        }
        // Others
    } // Scope by class EnemyControllerBT

} // namespace Root