using SkyDragonHunter.Entities;
using SkyDragonHunter.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter
{

    public class BossMoveAction : ActionNode<NewBossControllerBT>
    {
        public BossMoveAction(NewBossControllerBT context) : base(context)
        {
        }

        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override NodeStatus OnUpdate()
        {
            if (m_Context.IsTargetInAggroRange || m_Context.IsTargetInAttackRange)
            {
                return NodeStatus.Failure;
            }

            UpdatePos();

            return NodeStatus.Running;
        }

        private void UpdatePos()
        {
            var newPos = m_Context.transform.position;
            var scale = m_Context.transform.localScale;

            scale.x = -Mathf.Abs(scale.x);

            newPos.x -= Time.deltaTime * m_Context.Speed;

            m_Context.transform.localScale = scale;
            m_Context.transform.position = newPos;
        }
    } // Scope by class BossMoveAction

} // namespace Root