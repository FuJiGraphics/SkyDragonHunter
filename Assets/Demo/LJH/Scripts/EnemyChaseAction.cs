using SkyDragonHunter.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Entities {

    public class EnemyChaseAction : ActionNode<EnemyControllerBT>
    {
        // Public 메서드
        public EnemyChaseAction(EnemyControllerBT context) : base(context)
        {
            
        }

        // Protected 메서드
        protected override void OnStart()
        {            
            base.OnStart();
            m_Context.isChasing = true;
        }

        protected override NodeStatus OnUpdate()
        {
            if (!m_Context.IsTargetInAttackRange)
            {
                return NodeStatus.Running;
            }
            else
            {
                return NodeStatus.Success;
            }
        }

        protected override void OnEnd()
        {
            base.OnEnd();
            m_Context.isChasing = false;
            m_Context.ResetTarget();
        }

    } // Scope by class EnemyChaseAction

} // namespace Root