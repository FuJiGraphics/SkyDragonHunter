using SkyDragonHunter.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Entities {

    public class EnemyMoveAction<T> : ActionNode<T> where T : BaseControllerBT<T>
    {
        // Public 메서드
        public EnemyMoveAction(T context) : base(context)
        {

        }

        // Protected 메서드
        protected override void OnStart()
        {
            base.OnStart();
            m_Context.isMoving = true;
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
            m_Context.isMoving = false;
            m_Context.ResetTarget();
        }
    } // Scope by class EnemyMove

} // namespace Root