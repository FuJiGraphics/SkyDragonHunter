using SkyDragonHunter.Entities;
using SkyDragonHunter.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter {

    public class MonsterAttackAction : ActionNode<NewMonsterControllerBT>
    {
        private float lastAttackTime;

        public MonsterAttackAction(NewMonsterControllerBT context) : base(context)
        {
            lastAttackTime = 0f;
        }

        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override NodeStatus OnUpdate()
        {
            if (!m_Context.IsTargetInAttackRange)
            {
                return NodeStatus.Failure;
            }

            if (Time.time >= lastAttackTime + m_Context.AttackInterval)
            {
                lastAttackTime = Time.time;
                m_Context.animController.OnAttck();
                m_Context.monsterStatus.inventory.CurrentWeapon.Execute(m_Context.gameObject, m_Context.Target.gameObject);
                return NodeStatus.Success;
            }

            return NodeStatus.Running;
        }

        protected override void OnEnd()
        {
            base.OnEnd();
            m_Context.ResetTarget();
        }
    } // Scope by class MonsterAttackAction

} // namespace Root