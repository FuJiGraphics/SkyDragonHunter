using SkyDragonHunter.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkyDragonHunter.Gameplay;

namespace SkyDragonHunter.Entities 
{
    public enum BossAttackType
    {
        Melee,
        Ranged,
    }

    public class BossControllerBT : BaseControllerBT<BossControllerBT>
    {
        // Static Fields
        private static int s_InstanceNo = 0;

        // Fields
        [SerializeField] private BossAttackType m_AttackType;
        public int skillId;
        private float m_skillCooltime;
        public float lastSkillCasted;

        // Properties
        public override bool IsSkillAvailable
        {
            get
            {
                return Time.time > lastSkillCasted + m_skillCooltime;
            }
        }

        // Unity Methods

        // Public Methods
        public override void SetDataFromTable(int id)
        {
            ID = id;
            var data = DataTableManager.BossTable.Get(id);
            if (data == null)
            {
                Debug.LogError($"Set Boss Data Failed : ID '{id}' not found in boss table.");
                return;
            }

            name = data.Name;
            m_AttackType = data.Type;
            status.MaxHealth = data.HP;
            status.MaxDamage = data.ATK;
            status.MaxArmor = data.DEF;
            status.MaxResilient = data.REG;
            projectileId = data.ProjectileID;
            m_AttackInterval = data.AttackInterval;
            m_AttackRange = data.AttackRange;
            m_AggroRange = data.AggroRange;
            m_Speed = data.Speed;
            m_ChaseSpeed = data.ChaseSpeed;
            skillId = data.SkillID;
            m_skillCooltime = data.SkillCooltime;
        }

        public override void ResetTarget()
        {
            
        }

        protected override void InitBehaviourTree()
        {
            m_BehaviourTree = new BehaviourTree<BossControllerBT>(this);

            var rootSelector = new SelectorNode<BossControllerBT>(this);

            var skillSequence = new SequenceNode<BossControllerBT>(this);
            

            var attackSequence = new SequenceNode<BossControllerBT>(this);
            attackSequence.AddChild(new EntityAttackableCondition<BossControllerBT>(this));
            attackSequence.AddChild(new EntityAttackAction<BossControllerBT>(this));
            rootSelector.AddChild(attackSequence);

            var chaseSequence = new SequenceNode<BossControllerBT>(this);
            chaseSequence.AddChild(new EntityChasableCondition<BossControllerBT>(this));
            chaseSequence.AddChild(new EntityChaseAction<BossControllerBT>(this));
            rootSelector.AddChild(chaseSequence);

            var moveSequence = new SequenceNode<BossControllerBT>(this);
            moveSequence.AddChild(new EntityMoveCondition<BossControllerBT>(this));
            moveSequence.AddChild(new EntityMoveAction<BossControllerBT>(this));
            rootSelector.AddChild(moveSequence);

            m_BehaviourTree.SetRoot(rootSelector);
        }
    } // Scope by class BossControllerBT

} // namespace Root