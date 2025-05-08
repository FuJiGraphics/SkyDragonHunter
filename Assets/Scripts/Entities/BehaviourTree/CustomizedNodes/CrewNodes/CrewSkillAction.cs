using SkyDragonHunter.Entities;
using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Gamplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter {

    public class CrewSkillAction : ActionNode<NewCrewControllerBT>
    {
        // Protected Field
        private SkillExecutor skillExecutor;


        public CrewSkillAction(NewCrewControllerBT context) : base(context)
        {
            skillExecutor = context.skillExecutor;
        }

        protected override void OnStart()
        {
            base.OnStart();
            //Debug.LogError($"[{m_Context.gameObject.name}] used skill!");
        }

        protected override NodeStatus OnUpdate()
        {
            if(m_Context.IsTargetRequiredForSkill && !m_Context.IsTargetAllocated)
                return NodeStatus.Failure;

            if(m_Context.IsSkillAvailable)
            {
                skillExecutor.Execute(0);
                m_Context.animController.PlaySkillAnimation();
                return NodeStatus.Success;
            }
            return NodeStatus.Failure;
        }
        protected override void OnEnd()
        {
            base.OnEnd();
            //Debug.LogError($"[{m_Context.gameObject.name}] finished using skill!");
        }
    } // Scope by class CrewSkillAction

} // namespace Root