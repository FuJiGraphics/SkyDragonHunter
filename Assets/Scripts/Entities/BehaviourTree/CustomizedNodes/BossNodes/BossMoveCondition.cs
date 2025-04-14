using SkyDragonHunter.Entities;
using SkyDragonHunter.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter
{

    public class BossMoveCondition : ConditionNode<NewBossControllerBT>
    {
        public BossMoveCondition(NewBossControllerBT context) : base(context)
        {
        }

        protected override NodeStatus OnUpdate()
        {
            return !m_Context.IsTargetInAggroRange ? NodeStatus.Success : NodeStatus.Failure;
        }
    } // Scope by class BossMoveCondition

} // namespace Root