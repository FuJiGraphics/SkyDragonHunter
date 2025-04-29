using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.SaveLoad
{
    public class SavedUpgradeCounts
    {
        public int atkUpgradedCounts;
        public int hpUpgradedCounts;
        public int critRateUpgradedCounts;
        public int critDmgUpgradedCounts;

        public void InitData()
        {
            atkUpgradedCounts = 0;
            hpUpgradedCounts = 0;
            critRateUpgradedCounts = 0;
            critDmgUpgradedCounts = 0;
        }
        public void UpdateData()
        {

        }
        public void ApplySavedData()
        {

        }
    } // Scope by class SavedUpgradeCount

} // namespace Root