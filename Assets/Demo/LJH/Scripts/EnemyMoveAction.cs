using SkyDragonHunter.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Entities {

    public class EnemyMoveAction : ActionNode<EnemyControllerBT>
    {
        // Public 메서드
        public EnemyMoveAction(EnemyControllerBT context) : base(context)
        {

        }

        // Protected 메서드
        protected override void OnStart()
        {
            base.OnStart();
            m_Context.isMoving = true;
            Debug.LogWarning($"Entered Move Action");
        }

        protected override NodeStatus OnUpdate()
        {
            if(m_Context.IsTargetInAggroRange)
            {
                return NodeStatus.Failure; 
            }

            return NodeStatus.Running;
        }

        protected override void OnEnd()
        {
            base.OnEnd();
            Debug.Log($"Ended Move Action");
            m_Context.isMoving = false;
            m_Context.ResetTarget();
        }
    } // Scope by class EnemyMove

} // namespace Root