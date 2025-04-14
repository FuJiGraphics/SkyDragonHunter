using SkyDragonHunter.Entities;
using SkyDragonHunter.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter {

    public class CrewSkillAction : ActionNode<NewCrewControllerBT>
    {
        public CrewSkillAction(NewCrewControllerBT context) : base(context)
        {
        }

        protected override NodeStatus OnUpdate()
        {
            if(m_Context.IsTargetRequiredForSkill && !m_Context.IsTargetAllocated)
                return NodeStatus.Failure;

            if(m_Context.IsSkillAvailable)
            {
                m_Context.animController.PlaySkillAnimation();
                return NodeStatus.Success;
            }

            return NodeStatus.Failure;
        }
    } // Scope by class CrewSkillAction

} // namespace Root