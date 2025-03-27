using SkyDragonHunter.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Entities {

    public class EntityMoveCondition<T> : ConditionNode<T> where T : BaseControllerBT<T>
    {
        // Public 메서드
        public EntityMoveCondition(T context) : base(context)
        {
        }

        // Protected 메서드
        protected override NodeStatus OnUpdate()
        {
            return !m_Context.IsTargetInAttackRange ? NodeStatus.Success : NodeStatus.Failure;
        }
    } // Scope by class EnemyMoveCondition

} // namespace Root