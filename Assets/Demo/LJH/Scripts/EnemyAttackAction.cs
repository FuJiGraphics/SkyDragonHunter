using SkyDragonHunter.Entities;
using SkyDragonHunter.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.Entities {

    public class EnemyAttackAction : ActionNode<EnemyControllerBT>
    {
        // 필드 (Fields)
        private static readonly int s_AttackHash = Animator.StringToHash("Attack");
        private float lastAttackTime;

        // Public 메서드
        public EnemyAttackAction(EnemyControllerBT context) : base(context)
        {
            lastAttackTime = 0f;
        }

        // Protected 메서드
        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override NodeStatus OnUpdate()
        {
            if(!m_Context.IsTargetInAttackRange)
            {
                return NodeStatus.Failure;
            }

            if(Time.time < lastAttackTime + m_Context.attackDefinition.coolDown)
            {
                return NodeStatus.Running;
            }
            else
            {
                lastAttackTime = Time.time;
                m_Context.SetAnimTrigger(s_AttackHash);
                return NodeStatus.Success;
            }
        }        
    } // Scope by class AttackAction

} // namespace Root