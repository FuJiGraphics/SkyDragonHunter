using Newtonsoft.Json;
using SkyDragonHunter.Entities;
using SkyDragonHunter.Managers;
using SkyDragonHunter.SaveLoad;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables.Generic;
using System.Collections.Generic;
using System.Linq;
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
        private static readonly Dictionary<int, GameObject> s_PrefabStringTable = new()
        {
            { 14101, ResourcesMgr.Load<GameObject>("Eliya") },
            { 14102, ResourcesMgr.Load<GameObject>("Sigmund") },
            { 14103, ResourcesMgr.Load<GameObject>("Nox") },
            { 14201, ResourcesMgr.Load<GameObject>("Ranea") },
            { 14202, ResourcesMgr.Load<GameObject>("Isabella") },
            { 14203, ResourcesMgr.Load<GameObject>("Omega") },
            { 14204, ResourcesMgr.Load<GameObject>("Elena") },
            { 14205, ResourcesMgr.Load<GameObject>("Karna") },
            { 14302, ResourcesMgr.Load<GameObject>("Iris") },
            { 14303, ResourcesMgr.Load<GameObject>("Carlo") },
            { 14304, ResourcesMgr.Load<GameObject>("Valentine") },
            // 다른 ID와 프리팹 추가 가능
        };


        public string                       UnitName { get; set; }
        public CrewGrade                    UnitGrade { get; set; }
        public string                       UnitType { get; set; }
        public string                       SkillPrefabName { get; set; }
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

        private Dictionary<int, GameObject> m_InstanceMap = new();

        public GameObject GetInstance()
        {
            if (!m_InstanceMap.ContainsKey(base.ID))
            {
                var instance = GameObject.Instantiate(GetPrefab());
                m_InstanceMap.Add(base.ID, instance);
            }
            return m_InstanceMap[base.ID];
        }

        private GameObject GetPrefab()
        {
            int id = base.ID;
            return s_PrefabStringTable.TryGetValue(id, out var prefab) ? prefab : null;
        }
    }

    public class CrewTable : DataTable<CrewTableData>
    {
        
    } // Scope by class CrystalLevelTable

} // namespace Root