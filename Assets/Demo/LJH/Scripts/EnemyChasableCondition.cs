using SkyDragonHunter.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Entities {

    public class EnemyChasableCondition : ConditionNode<EnemyControllerBT>
    {
        // Public 메서드
        public EnemyChasableCondition(EnemyControllerBT context) : base(context)
        {
        }

        // Protected 메서드
        protected override NodeStatus OnUpdate()
        {
            return m_Context.IsTargetInAggroRange ? NodeStatus.Success : NodeStatus.Failure;
        }
    } // Scope by class EnemyChasableCondition

} // namespace Root