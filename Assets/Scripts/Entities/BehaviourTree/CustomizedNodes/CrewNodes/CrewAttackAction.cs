using SkyDragonHunter.Entities;
using SkyDragonHunter.Gameplay;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter {

    public class CrewAttackAction : ActionNode<NewCrewControllerBT>
    {
        float lastAttackTime;

        public CrewAttackAction(NewCrewControllerBT context) : base(context)
        {
            lastAttackTime = 0f;
        }

        protected override NodeStatus OnUpdate()
        {
            if (m_Context.skillExecutor != null &&
                m_Context.skillExecutor.IsAutoExecute &&
                m_Context.skillExecutor.SkillType == SkillType.Damage &&
                !m_Context.skillExecutor.IsChaseMode && 
                m_Context.skillExecutor.IsCooldownComplete)
                return NodeStatus.Failure;
            if (!m_Context.IsTargetInAttackRange)
                return NodeStatus.Failure;
            
            if(m_Context.damageTypeSpellCasted)
            {
                m_Context.damageTypeSpellCasted = false;
                lastAttackTime = Time.time;
                return NodeStatus.Failure;
            }

            float attackSpeedOffset = m_Context.characterStatus.AttackSpeedOffset <= 0f ? 
                0f : 1f - m_Context.characterStatus.AttackSpeedOffset;
            float attackInterval = Math.Max(m_Context.crewStatus.attackInterval + attackSpeedOffset, 0.25f);
            if (lastAttackTime + attackInterval < Time.time)
            {
                Attack();
                return NodeStatus.Success;
            }
            
            return NodeStatus.Running;
        }

        protected override void OnEnd()
        {
            base.OnEnd();            
        }

        private void Attack()
        {
            lastAttackTime = Time.time;
            m_Context.crewStatus.inventory.CurrentWeapon.Execute(m_Context.gameObject, m_Context.Target.gameObject);
            m_Context.animController.PlayAttackAnimation();
        }        
    } // Scope by class CrewAttackAction

} // namespace Root