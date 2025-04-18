using SkyDragonHunter.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Entities {

    public class NewCrewIdleAction : ActionNode<NewCrewControllerBT>
    {
        public NewCrewIdleAction(NewCrewControllerBT context) : base(context)
        {
        }

        protected override void OnStart()
        {
            base.OnStart();
            m_Context.floater.enabled = true;
        }

        protected override NodeStatus OnUpdate()
        {
            if(m_Context.IsTargetAllocated)
            {
                return NodeStatus.Failure;
            }

            return NodeStatus.Running;
        }

        protected override void OnEnd()
        {
            base.OnEnd();
            m_Context.floater.enabled = false;
        }
    } // Scope by class NewScript

} // namespace Root