using SkyDragonHunter.Entities;
using SkyDragonHunter.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter {

    public class NewBossSkillCondition : ConditionNode<NewBossControllerBT>
    {
        public NewBossSkillCondition(NewBossControllerBT context) : base(context)
        {
        }

        protected override NodeStatus OnUpdate()
        {
            if (!m_Context.IsTargetInAttackRange)
            {
                return NodeStatus.Failure;
            }

            return m_Context.IsSkillAvailable ? NodeStatus.Success : NodeStatus.Failure;
        }
    } // Scope by class NewBossSkillCondition

} // namespace Root