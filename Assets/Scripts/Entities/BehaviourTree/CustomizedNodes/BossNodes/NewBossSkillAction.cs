using SkyDragonHunter.Entities;
using SkyDragonHunter.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter {

    public class NewBossSkillAction : ActionNode<NewBossControllerBT>
    {
        public NewBossSkillAction(NewBossControllerBT context) : base(context)
        {
        }

        protected override NodeStatus OnUpdate()
        {
            if (!m_Context.IsTargetInAttackRange)
            {
                return NodeStatus.Failure;
            }

            if (m_Context.IsSkillAvailable)
            {                
                UseSkill();
                return NodeStatus.Success;
            }

            Debug.LogError($"Boss Skill Action Node Running (malfunctioning)");


            return NodeStatus.Running;
        }

        private void UseSkill()
        {
            // Skill Logic
        }
    } // Scope by class NewBossSkillAction

} // namespace Root