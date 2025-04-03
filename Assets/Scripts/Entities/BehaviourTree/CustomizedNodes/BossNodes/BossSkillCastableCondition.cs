using SkyDragonHunter.Entities;
using SkyDragonHunter.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter {

    public class BossSkillCastableCondition : ConditionNode<BossControllerBT>
    {
        // Public 메서드
        public BossSkillCastableCondition(BossControllerBT context) : base(context)
        {

        }

        // Protected 메서드
        protected override NodeStatus OnUpdate()
        {           
            if(!m_Context.IsTargetInAttackRange)
            {
                return NodeStatus.Failure;
            }

            return m_Context.IsSkillAvailable ? NodeStatus.Success : NodeStatus.Failure;
        }


    } // Scope by class BossSkillCastableCondition

} // namespace Root