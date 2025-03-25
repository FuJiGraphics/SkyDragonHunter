using NPOI.SS.Formula.Functions;
using SkyDragonHunter.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Entities {

    public class OnFieldCrewIdleAction : ActionNode<CrewControllerBT>
    {
        // 필드 (Fields)
        private float idleTime;

        // Public 메서드
        public OnFieldCrewIdleAction(CrewControllerBT context) : base(context)
        {
            idleTime = 2f;
        }

        // Protected 메서드
        protected override void OnStart()
        {
            base.OnStart();
            m_Context.isIdle = true;
            m_Context.lastIdleTime = Time.time;
        }

        protected override NodeStatus OnUpdate()
        {
            if (m_Context.IsTargetInAggroRange)
                return NodeStatus.Failure;

            if (Time.time < m_Context.lastIdleTime + idleTime)
            {
                return NodeStatus.Running;
            }
            else
            {
                return NodeStatus.Success;
            }
        }

        protected override void OnEnd()
        {
            base.OnEnd();
            m_Context.isIdle = false;
            m_Context.ResetTarget();
        }

    } // Scope by class OnFieldCrewMoveAction

} // namespace Root