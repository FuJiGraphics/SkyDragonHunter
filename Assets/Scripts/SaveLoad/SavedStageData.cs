using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkyDragonHunter.SaveLoad 
{
    public class SavedStageData
    {
        public int currentStage;
        public int currentZone;

        public int clearedStage;
        public int clearedZone;

        public void InitData()
        {
            currentStage = 1;
            currentZone = 1;

            clearedStage = 0;
            clearedZone = 0;
        }
        public void UpdateData()
        {

        }
    } // Scope by class SavedStageData

} // namespace Root