using SkyDragonHunter.Entities;
using SkyDragonHunter.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter {

    public class BossSkillAction : ActionNode<BossControllerBT> 
    {
        // 필드 (Fields)

        // Public 메서드
        public BossSkillAction(BossControllerBT context) : base(context)
        {
            
        }

        // Protected 메서드

        protected override NodeStatus OnUpdate()
        {
            if(!m_Context.IsTargetInAttackRange)
            {
                return NodeStatus.Failure;
            }

            if (m_Context.IsSkillAvailable)
            {
                m_Context.UseSkill();
                return NodeStatus.Success;
            }

            Debug.LogError($"Boss Skill Action Node Running (malfunctioning)");


            return NodeStatus.Running;
        }

        // Private 메서드
        
    } // Scope by class BossSkillAction

} // namespace Root