using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Scriptables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Entities {

    public enum CrewType
    {
        OnBoard,
        OnField,
    }


    public class CrewControllerBT : MonoBehaviour
    {
        // 필드 (Fields)
        [SerializeField] private CrewType m_Type;
        [SerializeField] private float m_Speed;
        [SerializeField] private float m_AggroRange;

        private BehaviourTree<CrewControllerBT> m_BehaviourTree;

        public AttackDefinition attackDefinition;
        public CharacterStatus status;

        private Animator m_Animator;

        [SerializeField] private Transform m_Target;

        private static readonly string s_PlayerTag = "Player";
        private static readonly string s_EnemyTag = "Enemy";

        public bool isMoving = false;
        public bool isChasing = false;
        public bool isDirectionToRight = false;

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
                if (m_Target == null)
                {
                    return float.MaxValue;
                }

                var distance = m_Target.position.x - transform.position.x;
                if (distance > 0)
                {
                    isDirectionToRight = true;
                }
                else
                {
                    isDirectionToRight = false;
                }

                var sr = m_Target.gameObject.GetComponent<SpriteRenderer>();
                var halfwidth = sr.bounds.size.x * 0.5f;

                if (isDirectionToRight)
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
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, m_AggroRange);

            Gizmos.color = Color.green;
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
            attackDefinition.Execute(gameObject, m_Target.gameObject);
        }

        public void ResetTarget()
        {
            bool resetRequired = false;
            bool isPrevTargetNull = false;
            if (m_Target == null)
            {
                resetRequired = true;
                isPrevTargetNull = true;
            }
            else if (m_Target.gameObject.CompareTag(s_PlayerTag))
            {
                resetRequired = true;
            }

            if (!resetRequired)
                return;

            var enemyLayer = LayerMask.GetMask(s_EnemyTag);
            var collider = Physics2D.OverlapCircle(transform.position, m_AggroRange, enemyLayer);
            if (collider != null)            
            {
                m_Target = collider.transform;
                if(isPrevTargetNull)
                {
                    ResetBehaviourTree();
                }
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
                case CrewType.OnBoard:
                    break;
                case CrewType.OnField:
                    break;
            }
        }

        private void InitOnFieldCrewBT()
        {
            m_BehaviourTree = new BehaviourTree<CrewControllerBT>(this);
            var rootSelector = new SelectorNode<CrewControllerBT>(this);

            var attackSequence = new SequenceNode<CrewControllerBT>(this);            
            rootSelector.AddChild(attackSequence);

            var chaseSequence = new SequenceNode<CrewControllerBT>(this);            
            rootSelector.AddChild(chaseSequence);

            var moveIdleSequence = new SequenceNode<CrewControllerBT>(this);            
            rootSelector.AddChild(moveIdleSequence);

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

    } // Scope by class OnFieldCrewControllerBT

} // namespace Root