using SkyDragonHunter.Entities;
using SkyDragonHunter.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Entities {

    public class CrewMountCondition : ConditionNode<CrewControllerBT>
    {
        // Public 메서드
        public CrewMountCondition(CrewControllerBT context) : base(context)
        {
        }

        // Protected 메서드
        protected override NodeStatus OnUpdate()
        {
            if (m_Context.Target != null)
            {                
                return NodeStatus.Failure;
            }
                        
            return NodeStatus.Success;
        }

    } // Scope by class CrewMountCondition

} // namespace Root