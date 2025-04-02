using SkyDragonHunter.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        [SerializeField] private BossAttackType attackType;
        public int skillId;
        public float skillCooltime;

        // Unity Methods
        public void Awake()
        {
            
        }



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
            status.MaxHealth = data.HP;
            status.MaxDamage = data.ATK;
            status.MaxArmor = data.DEF;
            status.MaxResilient = data.REG;
            projectileId = data.ProjectileID;
            characterInventory.CurrentWeapon.WeaponData.coolDown = data.AttackInterval;
            characterInventory.CurrentWeapon.WeaponData.range = data.AttackRange;
            m_AggroRange = data.AggroRange;
            m_Speed = data.Speed;
            m_ChaseSpeed = data.ChaseSpeed;
            skillId = data.SkillID;
            skillCooltime = data.SkillCooltime;
        }

        public override void ResetTarget()
        {
            
        }

        protected override void InitBehaviourTree()
        {
            
        }
    } // Scope by class BossControllerBT

} // namespace Root