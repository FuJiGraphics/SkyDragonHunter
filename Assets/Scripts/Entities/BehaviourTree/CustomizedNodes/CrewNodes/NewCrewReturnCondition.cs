using SkyDragonHunter.Entities;
using SkyDragonHunter.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter {

    public class NewCrewReturnCondition : ConditionNode<NewCrewControllerBT>
    {
        public NewCrewReturnCondition(NewCrewControllerBT context) : base(context)
        {

        }

        protected override NodeStatus OnUpdate()
        {
            if (m_Context.Target != null)
            {
                return NodeStatus.Failure;
            }

            return NodeStatus.Success;
        }
    } // Scope by class NewCrewReturnCondition

} // namespace Root