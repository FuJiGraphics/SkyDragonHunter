using SkyDragonHunter.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Entities {

    public class EnemyChasableCondition<T> : ConditionNode<T> where T : BaseControllerBT<T>
    {
        // Public 메서드
        public EnemyChasableCondition(T context) : base(context)
        {
        }

        // Protected 메서드
        protected override NodeStatus OnUpdate()
        {
            if (m_Context.IsTargetInAttackRange)
                return NodeStatus.Failure;

            return m_Context.IsTargetInAggroRange ? NodeStatus.Success : NodeStatus.Failure;
        }
    } // Scope by class EnemyChasableCondition

} // namespace Root