using SkyDragonHunter.Entities;
using SkyDragonHunter.Gameplay;

namespace SkyDragonHunter {

    public class CrewChaseCondition : ConditionNode<NewCrewControllerBT>
    {
        public CrewChaseCondition(NewCrewControllerBT context) : base(context)
        {

        }

        protected override NodeStatus OnUpdate()
        {
            if (m_Context.skillExecutor != null && 
                m_Context.skillExecutor.IsAutoExecute &&
                m_Context.skillExecutor.SkillType == SkillType.Damage &&
                !m_Context.skillExecutor.IsChaseMode &&
                m_Context.skillExecutor.IsCooldownComplete)
                return NodeStatus.Failure;
            if (m_Context.IsTargetInAttackRange)
                return NodeStatus.Failure;

            return m_Context.IsTargetAllocated ? NodeStatus.Success : NodeStatus.Failure;
        }
    } // Scope by class CrewChaseCondition

} // namespace Root