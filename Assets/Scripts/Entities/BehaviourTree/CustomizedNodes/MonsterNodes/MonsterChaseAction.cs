using SkyDragonHunter.Entities;
using SkyDragonHunter.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter {

    public class MonsterChaseAction : ActionNode<NewMonsterControllerBT>
    {
        public MonsterChaseAction(NewMonsterControllerBT context) : base(context)
        {
        }

        protected override void OnStart()
        {
            base.OnStart();
            m_Context.animController.OnChase(true);
        }

        protected override NodeStatus OnUpdate()
        {
            if (m_Context.IsTargetNull)
            {
                return NodeStatus.Failure;
            }

            UpdatePos();

            if (m_Context.IsTargetInAttackRange)
            {
                return NodeStatus.Success;
            }            
            return NodeStatus.Running;            
        }

        protected override void OnEnd()
        {
            base.OnEnd();
            m_Context.animController.OnChase(false);
        }

        private void UpdatePos()
        {
            var newPos = m_Context.transform.position;
            var targetPosX = m_Context.Target.position.x;
            var scale = m_Context.transform.localScale;
            int toRight = 1;

            if (newPos.x > targetPosX)
            {
                toRight = -1;
            }
            scale.x = Mathf.Abs(scale.x) * toRight;

            newPos.x += Time.deltaTime * m_Context.ChaseSpeed * toRight;

            foreach(var healthBar in m_Context.HealthBars)
            {
                healthBar.transform.localScale = scale;
            }

            m_Context.transform.localScale = scale;
            m_Context.transform.position = newPos;
        }
    } // Scope by class MonsterChaseAction

} // namespace Root