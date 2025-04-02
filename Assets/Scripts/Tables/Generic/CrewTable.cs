using SkyDragonHunter.Entities;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Tables.Generic;
using System.Text;
using UnityEngine;

namespace SkyDragonHunter.Tables
{
    public enum PossessionBonusStatType
    {
        ATK,
        HP,
        DEF,
        REG,
        CritRate,
        CritMultiplier,
        GoldBonus,
        ExpBonus,
    }

    public class CrewData : DataTableData
    {
        public string                       Name { get; set; }
        public CrewType                     Type { get; set; }
        public int                          ActiveSkillID { get; set; }
        public float                        ActiveSkillCooltime { get; set; }
        public float                        ActiveSkillInitialDelay { get; set; }
        public int                          PassiveSkillID { get; set; }
        public float                        PassiveSkillCooltime { get; set; }
        public int                          BasicHP { get; set; }
        public int                          BasicATK { get; set; }
        public int                          BasicDEF { get; set; }
        public int                          BasicREG { get; set; }
        public PossessionBonusStatType      PossessionBonus1Type { get; set; }
        public int                          PossessionBonus1Value { get; set; }
        public PossessionBonusStatType      PossessionBonus2Type { get; set; }
        public int                          PossessionBonus2Value { get; set; }
        public float                        CritRate { get; set; }
        public float                        CritMultiplier { get; set; }
        public float                        AttackInterval { get; set; }
        public float                        AttackRange { get; set; }
        public double                       IncreasingHP { get; set; }
        public double                       IncreasingATK { get; set; }
        public double                       IncreasingDEF { get; set; }
        public double                       IncreasingREG { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"'{Name}'({Type}),ATK: {BasicATK} + {IncreasingATK}, HP: {BasicHP} + {IncreasingHP}" +
                $", DEF: {BasicDEF} + {IncreasingDEF}, REG: {BasicREG} + {IncreasingREG}" + 
                $"\nActive: {ActiveSkillID}({ActiveSkillCooltime},{ActiveSkillInitialDelay})"+
                $"\nPassive: {PassiveSkillID}({PassiveSkillCooltime})");
            return sb.ToString();
        }        
    }

    public class CrewTable : DataTable<CrewData>
    {

    } // Scope by class CrystalLevelTable

} // namespace Root