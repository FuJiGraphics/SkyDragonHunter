using SkyDragonHunter.Entities;
using SkyDragonHunter.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Entities {

    public class CrewMountAction : ActionNode<CrewControllerBT>
    {
        // 필드 (Fields)
        
        // Public 메서드
        public CrewMountAction(CrewControllerBT context) : base(context)
        {
            
        }

        // Protected 메서드
        protected override void OnStart()
        {
            base.OnStart();
            m_Context.MountAction(true);
            Debug.LogError($"{m_Context.name} entered Mount Action");            
        }

        protected override NodeStatus OnUpdate()
        {
            if (m_Context.Target != null)
            {
                //m_Context.ResetBehaviourTree();
                return NodeStatus.Failure;
            }

            return NodeStatus.Running;
        }

        protected override void OnEnd()
        {
            base.OnEnd();
            m_Context.MountAction(false);
            Debug.LogError($"{m_Context} exited Mount Action");
        }

    } // Scope by class CrewMountAction

} // namespace Root