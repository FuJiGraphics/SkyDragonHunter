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

        private static readonly string s_PlayerTag = "Player";
        private static readonly string s_CrewTag = "Crew";
        private static readonly string s_CreatureTag = "Creature";

        public bool isMoving = false;
        public bool isChasing = false;
        public bool isDirectionToRight = false;

        private float m_InitialYPos;
        [SerializeField] private float m_YaxisMovementAmplitude = 0.5f;
        [SerializeField] private float m_YaxisMovementPeriod = 2f;

        private float rand;

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
                                
                var distance = m_Target.position.x - transform.position.x;
                if(distance > 0)
                {
                    isDirectionToRight = true;
                }
                else
                {
                    isDirectionToRight = false;
                }

                var sr = m_Target.gameObject.GetComponent<SpriteRenderer>();
                var halfwidth = sr.bounds.size.x * 0.5f;

                if(isDirectionToRight)
                {
                    distance -= halfwidth;
                }
                else
                {
                    distance += halfwidth;
                }

                return Mathf.Abs(distance);
            }
        }

        // 이벤트 (Events)
        // 유니티 (MonoBehaviour 기본 메서드)
        private void Start()
        {            
            m_InitialYPos = transform.position.y;
            rand = Random.Range(0f, 1f) * 10f;
            InitBehaviourTree();
        }

        private void Update()
        {
            UpdatePosition();
            m_BehaviourTree.Update();
        }

        private void FixedUpdate()
        {
            ResetTarget();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, m_AggroRange);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, attackDefinition.range);
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
            //m_Animator.SetTrigger(triggerHash);
            //Debug.Log($"Attacked! at {Time.time}");
        }

        public void ResetTarget()
        {
            bool resetRequired = false;
            if (m_Target == null)
                resetRequired = true;            
            else if (m_Target.gameObject.CompareTag(s_PlayerTag))
            {
                resetRequired = true;
            }

            if (!resetRequired)
                return;

            var onFieldObjectLayer = LayerMask.GetMask(s_CrewTag, s_CreatureTag);
            var collider = Physics2D.OverlapCircle(transform.position, m_AggroRange, onFieldObjectLayer);
            if (collider == null)
            {
                m_Target = GameObject.FindWithTag(s_PlayerTag).transform;
            }
            else
            {
                m_Target = collider.transform;
            }
        }

        public void ResetBehaviourTree()
        {
            m_BehaviourTree.Reset();
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

            var moveSequence = new SequenceNode<EnemyControllerBT>(this);
            moveSequence.AddChild(new EnemyMoveCondition(this));
            moveSequence.AddChild(new EnemyMoveAction(this));
            rootSelector.AddChild(moveSequence);

            m_BehaviourTree.SetRoot(rootSelector);
        }
                
        private void UpdatePosition()
        {
            var newPos = transform.position;
            
            //float newYPos = Mathf.Sin((Time.time + rand) * (2 * Mathf.PI / m_YaxisMovementPeriod)) * m_YaxisMovementAmplitude;
            //
            //newPos.y = m_InitialYPos + newYPos;                        

            if (isChasing || isMoving)
            {
                int toRight = 1;
                if (isChasing)
                    toRight *= 3;
                if (!isDirectionToRight)
                    toRight *= -1;
                newPos.x += Time.deltaTime * m_Speed * toRight;                
            }

            // Debug.Log($"new Position: {newPos}");
            transform.position = newPos;
        }

        // Others

    } // Scope by class EnemyControllerBT

} // namespace Root