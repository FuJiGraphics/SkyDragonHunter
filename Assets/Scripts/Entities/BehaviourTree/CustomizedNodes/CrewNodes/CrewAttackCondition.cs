using SkyDragonHunter.Entities;
using SkyDragonHunter.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter {

    public class CrewAttackCondition : ConditionNode<NewCrewControllerBT>
    {  
        public CrewAttackCondition(NewCrewControllerBT context) : base(context)
        {

        }

        protected override NodeStatus OnUpdate()
        {
            if(m_Context.skillExecutor != null && m_Context.skillExecutor.IsCooldownComplete)
                return NodeStatus.Failure;

            return m_Context.IsTargetInAttackRange ? NodeStatus.Success : NodeStatus.Failure;
        }
    } // Scope by class CrewAttackCondition

} // namespace Root