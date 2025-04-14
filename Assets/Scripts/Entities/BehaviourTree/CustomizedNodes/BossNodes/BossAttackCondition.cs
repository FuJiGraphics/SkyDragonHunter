using SkyDragonHunter.Entities;
using SkyDragonHunter.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter
{

    public class BossAttackCondition : ConditionNode<NewBossControllerBT>
    {
        public BossAttackCondition(NewBossControllerBT context) : base(context)
        {
        }

        protected override NodeStatus OnUpdate()
        {
            return m_Context.IsTargetInAttackRange ? NodeStatus.Success : NodeStatus.Failure;
        }
    } // Scope by class BossAttackCondition

} // namespace Root