using SkyDragonHunter.Entities;
using SkyDragonHunter.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter
{

    public class BossChaseCondition : ConditionNode<NewBossControllerBT>
    {
        public BossChaseCondition(NewBossControllerBT context) : base(context)
        {
        }

        protected override NodeStatus OnUpdate()
        {
            if (m_Context.IsTargetInAttackRange)
                return NodeStatus.Failure;

            return m_Context.IsTargetInAggroRange ? NodeStatus.Success : NodeStatus.Failure;
        }
    } // Scope by class BossChaseCondition

} // namespace Root