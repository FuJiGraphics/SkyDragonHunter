using SkyDragonHunter.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Entities {

    public class EntityChaseAction<T> : ActionNode<T> where T : BaseControllerBT<T>
    {
        // Public 메서드
        public EntityChaseAction(T context) : base(context)
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
            if (m_Context.IsSkillAvailable)
            {
                return NodeStatus.Failure;
            }

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