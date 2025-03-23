using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Scriptables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Entities {

    public enum EnemyType
    {
        Melee,
        Ranged,
        Stationary // Boss
    }

    public class EnemyControllerBT : MonoBehaviour
    {       
        // 필드 (Fields)
        [SerializeField] private EnemyType m_Type;
        [SerializeField] private float m_Speed;
        [SerializeField] private float m_AggroRange;

        private BehaviourTree<EnemyControllerBT> m_BehaviourTree;

        public AttackDefinition attackDefinition;
        public CharacterStatus status;

        private Animator m_Animator;

        [SerializeField] private Transform m_Target;

        // 속성 (Properties)
        public bool IsTargetInAttackRange
        {
            get
            {                                
                return TargetDistance < attackDefinition.range;
            }
        }

        public bool IsTargetInAggroRange
        {
            get
            {
                return TargetDistance < m_AggroRange;
            }
        }

        public float TargetDistance
        {
            get
            {
                if(m_Target == null)
                {
                    return float.MaxValue;
                }
                var distance = Mathf.Abs(m_Target.position.x - transform.position.x);
                return distance;
            }
        }

        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Start()
        {
            InitBehaviourTree();
        }
    
        private void Update()
        {
            
        }
    
        // Public 메서드
        public void SetAnimTrigger(string triggerName)
        {
            var triggerHash = Animator.StringToHash(triggerName);
            Debug.Log($"Recommended to use trigger hash({triggerHash}) instead of trigger name({triggerName})");
            SetAnimTrigger(triggerHash);
        }

        public void SetAnimTrigger(int triggerHash)
        {
            m_Animator.SetTrigger(triggerHash);
        }

        // Private 메서드
        private void InitBehaviourTree()
        {
            switch (m_Type)
            {
                case EnemyType.Melee:
                    InitMeleeBT();
                    break;
                case EnemyType.Ranged:
                    break;
                case EnemyType.Stationary:
                    break;
            }
        }

        private void InitMeleeBT()
        {
            m_BehaviourTree = new BehaviourTree<EnemyControllerBT>(this);

            var rootSelector = new SelectorNode<EnemyControllerBT>(this);

            var attackSequence = new SequenceNode<EnemyControllerBT>(this);
            attackSequence.AddChild(new EnemyAttackableCondition(this));
            attackSequence.AddChild(new EnemyAttackAction(this));
            rootSelector.AddChild(attackSequence);

            var chaseSequence = new SequenceNode<EnemyControllerBT>(this);
            chaseSequence.AddChild(new EnemyChasableCondition(this));
            chaseSequence.AddChild(new EnemyChaseAction(this));
            rootSelector.AddChild(chaseSequence);

            m_BehaviourTree.SetRoot(rootSelector);
        }

        // Others

    } // Scope by class EnemyControllerBT

} // namespace Root