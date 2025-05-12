using CsvHelper.TypeConversion;
using Newtonsoft.Json;
using SkyDragonHunter.Managers;
using SkyDragonHunter.Structs;
using SkyDragonHunter.Tables;
using System;
using System.Collections.Generic;

namespace SkyDragonHunter.SaveLoad
{
    public class SavedTutorialData
    {
        public bool tutorialCleared;

        public void InitData()
        {
            tutorialCleared = false;
        }

        public void UpdateSavedData()
        {

        }

        public void ApplySavedData()
        {

        }

    } // Scope by class SavedItemData

} // namespace Root