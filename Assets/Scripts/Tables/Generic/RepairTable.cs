using SkyDragonHunter.Entities;
using SkyDragonHunter.Gameplay;
using SkyDragonHunter.Tables.Generic;
using System.Collections.Generic;
using System;
using System.Text;

namespace SkyDragonHunter.Tables
{
    public class RepairTableData : DataTableData
    {
        public string RepName { get; set; }
        public int RepGrade { get; set; }
        public string RepEqHP { get; set; }
        public string RepEqREC { get; set; }
        public string RepHoldHP { get; set; }
        public string RepHoldREC { get; set; }
        public string RepLvUpCost { get; set; }
        public string RepEqHPup { get; set; }
        public string RepEqRECup { get; set; }
        public string RepHoldHPup { get; set; }
        public string RepHoldRECup { get; set; }
        public float RepHpMultiplier { get; set; }
        public float RepRecMultiplier { get; set; }
        public int RepEffectType { get; set; } // 0: Normal, 1: Shield, 2: Divine
        public string RepShield { get; set; }
        public float RepDivineStride { get; set; }
        public int RepUpgradeID { get; set; }
        public string RepIconResourceName { get; set; }
    }

    public class RepairTableTemplate : DataTable<RepairTableData>
    {

        public static RepairDummy[] GetAllRepairDummyTypes()
        {
            var allRepairDummys = new List<RepairDummy>();

            foreach (RepairGrade grade in Enum.GetValues(typeof(RepairGrade)))
            {
                foreach (RepairType type in Enum.GetValues(typeof(RepairType)))
                {
                    RepairDummy dummy = new RepairDummy
                    {
                        Grade = grade,
                        Type = type,
                        Count = 0,
                        Level = 1
                    };

                    allRepairDummys.Add(dummy);
                }
            }

            return allRepairDummys.ToArray();
        }
    }

} // namespace Root