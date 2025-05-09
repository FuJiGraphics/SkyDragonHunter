using SkyDragonHunter.Entities;
using SkyDragonHunter.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter
{

    public class CrewSkillAction : ActionNode<NewCrewControllerBT>
    {
        // Protected Field
        private SkillExecutor skillExecutor;
        private SkillType skillType;


        public CrewSkillAction(NewCrewControllerBT context) : base(context)
        {
            skillExecutor = context.skillExecutor;
            skillType = SkillType.Undefined;
            if (skillExecutor != null)
            {
                skillType = skillExecutor.SkillType;
            }
        }

        protected override void OnStart()
        {
            base.OnStart();
            //Debug.LogError($"[{m_Context.gameObject.name}] used skill!");
        }

        protected override NodeStatus OnUpdate()
        {
            //if(m_Context.IsTargetRequiredForSkill && !m_Context.IsTargetAllocated)
            //    return NodeStatus.Failure;
            if (!m_Context.skillExecutor.IsAutoExecute)
                return NodeStatus.Failure;

            if (!m_Context.skillExecutor.IsCooldownComplete)
            {
                m_Context.animController.PlaySkillAnimation();
                if (skillType == SkillType.Damage)
                    m_Context.damageTypeSpellCasted = true;

                return NodeStatus.Success;
            }
            return NodeStatus.Running;
        }
        protected override void OnEnd()
        {
            base.OnEnd();
            //Debug.LogError($"[{m_Context.gameObject.name}] finished using skill!");
        }
    } // Scope by class CrewSkillAction

} // namespace Root