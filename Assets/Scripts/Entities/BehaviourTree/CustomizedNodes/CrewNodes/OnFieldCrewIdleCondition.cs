using SkyDragonHunter.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Entities {

    public class OnFieldCrewIdleCondition : ConditionNode<CrewControllerBT>
    {
        // Public 메서드
        public OnFieldCrewIdleCondition(CrewControllerBT context) : base(context)
        {
        }

        // Protected 메서드
        protected override NodeStatus OnUpdate()
        {
            if(m_Context.IsTargetInAggroRange)
            {
                return NodeStatus.Failure;
            }

            return NodeStatus.Success;
        }

    } // Scope by class OnFieldCrewMoveCondition

} // namespace Root