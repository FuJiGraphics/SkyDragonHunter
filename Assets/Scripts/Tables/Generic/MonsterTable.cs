using SkyDragonHunter.Entities;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Tables.Generic;
using System.Text;
using UnityEngine;

namespace SkyDragonHunter.Tables
{    
    public class MonsterData : DataTableData
    {
        public string Name { get; set; }
        public double HP { get; set; }
        public double ATK { get; set; }
        public float AttackInterval { get; set; }
        public MonsterType Type { get; set; }
        public string ProjectileID { get; set; }
        public float AttackRange { get; set; }
        public float AggroRange { get; set; }
        public float Speed { get; set; }
        public float ChaseSpeed { get; set; }

        public int[] AilmentImmunity { get; set; }


        public Sprite Icon
        {
            get
            {
                return ResourcesMgr.Load<Sprite>(ID.ToString());
            }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"'{Name}'({Type}),ATK: {ATK}, HP: {HP}" +
                $"\nSpeed: (Normal: {Speed}, Chasing: {ChaseSpeed})" +
                $"\nRange: (Aggro: {AggroRange}, ATK RNG: {AttackRange})");
            return sb.ToString();
        }
    }

    public class MonsterTable : DataTable<MonsterData>
    {

    }

} // namespace Root