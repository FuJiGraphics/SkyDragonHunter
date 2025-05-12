using SkyDragonHunter.Entities;
using SkyDragonHunter.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter {
    
    public class CrewSkillCondition : ConditionNode<NewCrewControllerBT>
    {        
        public CrewSkillCondition(NewCrewControllerBT context) : base(context)
        {

        }

        protected override NodeStatus OnUpdate()
        {
            if (m_Context.skillExecutor == null)
                return NodeStatus.Failure;

            if (!m_Context.skillExecutor.IsAutoExecute)
                return NodeStatus.Failure;

            if (m_Context.IsTargetRequiredForSkill && !m_Context.IsTargetAllocated)
                return NodeStatus.Failure;

            return m_Context.skillExecutor.IsCooldownComplete 
                ? NodeStatus.Success : NodeStatus.Failure;
        }
    } // Scope by class CrewSkillCondition

} // namespace Root