using SkyDragonHunter.Entities;
using SkyDragonHunter.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter {

    public class CrewChaseAction : ActionNode<NewCrewControllerBT>
    {
        
        public CrewChaseAction(NewCrewControllerBT context) : base(context)
        {
        }

        protected override NodeStatus OnUpdate()
        {
            if (m_Context.IsSkillAvailable)
                return NodeStatus.Failure;
            if (!m_Context.IsTargetAllocated)
                return NodeStatus.Failure;

            UpdatePosition();

            if (m_Context.IsTargetInAttackRange)
            {
                return NodeStatus.Success;
            }
            
            return NodeStatus.Running;
        }

        private void UpdatePosition()
        {            
            var newPos = m_Context.transform.position;
            var direction = Vector2.zero;

            // direction.y = m_Context.targetPosY - m_Context.floater.StartY;
            direction.x = m_Context.Target.position.x - m_Context.transform.position.x;
            direction.Normalize();

            newPos.x += direction.x * Time.deltaTime * m_Context.crewStatus.chaseSpeed;
            // m_Context.floater.StartY += direction.y * Time.deltaTime * m_Context.crewStatus.chaseSpeed;
            m_Context.transform.position = newPos;
        }
    } // Scope by class CrewChaseAction

} // namespace Root