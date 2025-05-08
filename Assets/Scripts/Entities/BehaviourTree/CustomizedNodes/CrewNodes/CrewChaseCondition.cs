using SkyDragonHunter.Entities;
using SkyDragonHunter.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter {

    public class CrewChaseCondition : ConditionNode<NewCrewControllerBT>
    {
        public CrewChaseCondition(NewCrewControllerBT context) : base(context)
        {
        }

        protected override NodeStatus OnUpdate()
        {
            if (m_Context.skillExecutor != null && m_Context.skillExecutor.IsCooldownComplete)
                return NodeStatus.Failure;
            if (m_Context.IsTargetInAttackRange)
                return NodeStatus.Failure;

            return m_Context.IsTargetAllocated ? NodeStatus.Success : NodeStatus.Failure;
        }
    } // Scope by class CrewChaseCondition

} // namespace Root