using Newtonsoft.Json;
using SkyDragonHunter.Entities;
using SkyDragonHunter.Managers;
using SkyDragonHunter.SaveLoad;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables.Generic;
using System.Text;
using UnityEngine;

namespace SkyDragonHunter.Tables
{
    public enum CrewGrade
    {
        Rare,
        Epic,
        Unique,
        Legend
    }

    [JsonConverter(typeof(CrewTableDataConverter))]
    public class CrewTableData : DataTableData
    {
        public string                       UnitName { get; set; }
        public CrewGrade                    UnitGrade { get; set; }
        public string                       UnitType { get; set; }
        public string                       ActiveSkillID { get; set; }
        public float                        ActiveSkillNormalcooltime { get; set; }
        public float                        ActiveSkillStartingcooltime { get; set; }
        public string                       PassiveSkillID { get; set; }
        public float                        PassiveSkillNormalcooltime { get; set; }
        public BigNum                       UnitbasicHP { get; set; }
        public BigNum                       UnitbasicATK { get; set; }
        public BigNum                       UnitbasicDEF { get; set; }
        public BigNum                       UnitbasicREC { get; set; }
        public PossessionBonusStatType      PassiveStatType1 { get; set; }
        public int                          PassiveStatValue1 { get; set; }
        public PossessionBonusStatType      PassiveStatType2 { get; set; }
        public int                          PassiveStatValue2 { get; set; }
        public float                        CriticalPercent { get; set; }
        public float                        CriticalDamage { get; set; }
        public float                        AttackSpeed { get; set; }
        public float                        AttackRange { get; set; }
        public BigNum                       LevelBracketHP { get; set; }
        public BigNum                       LevelBracketATK { get; set; }
        public BigNum                       LevelBracketDEF { get; set; }
        public BigNum                       LevelBracketREC { get; set; }
        public int                          LevelBracketPassiveStat1 { get; set;}
        public int                          LevelBracketPassiveStat2 { get; set;}

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"'{UnitName}'({UnitType}),ATK: {UnitbasicATK} + {LevelBracketATK}, HP: {UnitbasicHP} + {LevelBracketHP}" +
                $", DEF: {UnitbasicDEF} + {LevelBracketDEF}, REG: {UnitbasicREC} + {LevelBracketREC}" + 
                $"\nActive: {ActiveSkillID}({ActiveSkillNormalcooltime},{ActiveSkillStartingcooltime})"+
                $"\nPassive: {PassiveSkillID}({PassiveSkillNormalcooltime})");
            return sb.ToString();
        }
    }

    public class CrewTable : DataTable<CrewTableData>
    {
        
    } // Scope by class CrystalLevelTable

} // namespace Root