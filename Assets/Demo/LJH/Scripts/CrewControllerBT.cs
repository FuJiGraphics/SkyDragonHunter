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


    public class CrewControllerBT : BaseControllerBT<CrewControllerBT>
    {
        // 필드 (Fields)
        [SerializeField] private CrewType m_Type;

        private static readonly string s_PlayerTag = "Player";
        private static readonly string s_EnemyTag = "Enemy";

        public bool isMoving = false;
        public bool isChasing = false;

        // 유니티 (MonoBehaviour 기본 메서드)
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
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, m_AggroRange);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, attackDefinition.range);
        }


        // Public 메서드

        public override void ResetTarget()
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

        // Protected 메서드
        protected override void InitBehaviourTree()
        {
            switch (m_Type)
            {
                case CrewType.OnBoard:
                    break;
                case CrewType.OnField:
                    InitOnFieldCrewBT();
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