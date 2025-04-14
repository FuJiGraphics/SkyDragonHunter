using SkyDragonHunter.Entities;
using SkyDragonHunter.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter {

    public class MonsterAttackCondition : ConditionNode<NewMonsterControllerBT>
    {
        public MonsterAttackCondition(NewMonsterControllerBT context) : base(context)
        {
        }

        protected override NodeStatus OnUpdate()
        {
            return m_Context.IsTargetInAttackRange ? NodeStatus.Success : NodeStatus.Failure;
        }
    } // Scope by class MonsterAttackCondition

} // namespace Root