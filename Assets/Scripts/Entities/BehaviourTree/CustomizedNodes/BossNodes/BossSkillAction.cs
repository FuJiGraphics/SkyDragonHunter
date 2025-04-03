using SkyDragonHunter.Entities;
using SkyDragonHunter.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter {

    public class BossSkillAction : ActionNode<BossControllerBT> {
    {
        // 필드 (Fields)
        private Vector3 targetPos;
        private readonly float m_threshold = 0.15f;

        // Public 메서드
        public BossSkillAction(BossControllerBT context) : base(context)
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

            if (m_Context.OriginDistance < m_threshold)
            {
                var newPos = new Vector3(m_Context.onFieldOriginPosition.x, m_Context.transform.position.y, 0);
                m_Context.transform.position = newPos;
                m_Context.floatingEffect.StartY = m_Context.onFieldOriginPosition.y;
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
            if (Time.deltaTime * m_Context.Speed > m_Context.OriginDistance)
            {
                newPos.x = m_Context.onFieldOriginPosition.x;
                Debug.Log($"Transition amount '{Time.deltaTime * m_Context.Speed}' exceeded target distance '{m_Context.OriginDistance}', Force repositioned");
            }

            m_Context.transform.position = newPos;
            m_Context.floatingEffect.StartY += direction.y * Time.deltaTime * m_Context.Speed;
        }

    } // Scope by class BossSkillAction

} // namespace Root