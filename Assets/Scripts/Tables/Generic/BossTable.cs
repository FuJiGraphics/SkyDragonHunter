using SkyDragonHunter.Entities;
using SkyDragonHunter.Tables.Generic;
using System.Text;
using UnityEngine;

namespace SkyDragonHunter.Tables
{
    public class BossData : DataTableData
    {
        public string Name { get; set; }
        public BossAttackType Type { get; set; }
        public double HP { get; set; }
        public double ATK { get; set; }
        public int DEF { get; set; }
        public int REG { get; set; }
        public int ProjectileID { get; set; }
        public float AttackInterval { get; set; }
        public float AttackRange { get; set; }
        public float AggroRange { get; set; }
        public float Speed { get; set; }
        public float ChaseSpeed { get; set; }
        public int SkillID { get; set; }
        public float SkillCooltime { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"'{Name}'({Type}),ATK: {ATK}, HP: {HP}, DEF: {DEF}, REG: {REG}" +
                $"\nSpeed: (Normal: {Speed}, Chasing: {ChaseSpeed})" +
                $"\nRange: (Aggro: {AggroRange}, ATK RNG: {AttackRange})");
            return sb.ToString();
        }
    }

    public class BossTable : DataTable<BossData>
    {

    } // Scope by class CrystalLevelTable

} // namespace Root