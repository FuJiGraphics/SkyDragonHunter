using SkyDragonHunter.Entities;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Tables.Generic;
using System.Text;
using UnityEngine;

namespace SkyDragonHunter.Tables
{
    public class CrewData : DataTableData
    {
        public string Name { get; set; }
        public CrewType Type { get; set; }
        public int ActiveSkillID { get; set; }
        public float ActiveSkillCooltime { get; set; }
        public float ActiveSkillInitialDelay { get; set; }
        public int PassiveSkillID { get; set; }
        public float PassiveSkillCooltime { get; set; }
        public int BasicHP { get; set; }
        public int BasicATK { get; set; }
        public int BasicDEF { get; set; }
        public int BasicREG { get; set; }
        public float CritRate { get; set; }
        public float CritMultiplier { get; set; }
        public float AttackInterval { get; set; }
        public float AttackRange { get; set; }
        public double LevelBracketHP { get; set; }
        public double LevelBracketATK { get; set; }
        public double LevelBracketDEF { get; set; }
        public double LevelBracketREG { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"'{Name}'({Type}),ATK: {BasicATK} + {LevelBracketATK}, HP: {BasicHP} + {LevelBracketHP}" +
                $", DEF: {BasicDEF} + {LevelBracketDEF}, REG: {BasicREG} + {LevelBracketREG}" + 
                $"\nActive: {ActiveSkillID}({ActiveSkillCooltime},{ActiveSkillInitialDelay})"+
                $"\nPassive: {PassiveSkillID}({PassiveSkillCooltime})");
            return sb.ToString();
        }        
    }

    public class CrewTable : DataTable<CrewData>
    {

    } // Scope by class CrystalLevelTable

} // namespace Root