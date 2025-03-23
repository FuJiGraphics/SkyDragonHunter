using SkyDragonHunter.Entities;
using SkyDragonHunter.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Entities {

    public class EnemyAttackableCondition : ConditionNode<EnemyControllerBT>
    {      
        // Public 메서드
        public EnemyAttackableCondition(EnemyControllerBT context) : base(context)
        {
        }

        // Protected 메서드
        protected override NodeStatus OnUpdate()
        {
            return m_Context.IsTargetInAttackRange ? NodeStatus.Success : NodeStatus.Failure;
        }
    } // Scope by class AttackableCondition

} // namespace Root