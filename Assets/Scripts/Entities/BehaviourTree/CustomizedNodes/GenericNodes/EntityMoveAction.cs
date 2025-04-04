using SkyDragonHunter.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Entities {

    public class EntityMoveAction<T> : ActionNode<T> where T : BaseControllerBT<T>
    {
        // Public 메서드
        public EntityMoveAction(T context) : base(context)
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
            if (m_Context.IsSkillAvailable)
            {
                return NodeStatus.Failure;
            }

            if (m_Context.IsTargetInAggroRange)
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