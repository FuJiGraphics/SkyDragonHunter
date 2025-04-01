using SkyDragonHunter.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Entities {

    public class CrewReturnAction : ActionNode<CrewControllerBT>
    {
        // 필드 (Fields)
        private Vector3 targetPos;
        private readonly float m_threshold = 0.05f;

        // Public 메서드
        public CrewReturnAction(CrewControllerBT context) : base(context)
        {
            targetPos = m_Context.onFieldOriginPosition;
        }

        // Protected 메서드
        protected override void OnStart()
        {
            base.OnStart();
            //Debug.Log($"{m_Context.name} entered Return Action");        
        }

        protected override NodeStatus OnUpdate()
        {
            if (m_Context.IsTargetInAggroRange)
            {
                return NodeStatus.Failure;
            }

            if (m_Context.DistanceToOrigin < m_threshold)
            {
                return NodeStatus.Failure;
            }

            UpdatePos();

            return NodeStatus.Running;
        }

        protected override void OnEnd()
        {
            base.OnEnd();
            //Debug.Log($"{m_Context} exited Return Action");
        }

        // Private 메서드
        private void UpdatePos()
        {
            var contextPos = Vector3.zero;
            contextPos.x = m_Context.transform.position.x;
            contextPos.y = m_Context.floatingEffect.StartY;

            var direction = (m_Context.onFieldOriginPosition - contextPos).normalized;

            var newPos = contextPos + direction * Time.deltaTime * m_Context.Speed;
            newPos.y = m_Context.transform.position.y;
            newPos.z = 0f;

            m_Context.transform.position = newPos;
            m_Context.floatingEffect.StartY += direction.y * Time.deltaTime * m_Context.Speed;
        }

    } // Scope by class CrewReturnAction

} // namespace Root